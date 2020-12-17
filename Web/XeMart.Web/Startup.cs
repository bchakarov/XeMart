namespace XeMart.Web
{
    using System;
    using System.Reflection;

    using Azure.Storage.Blobs;

    using CloudinaryDotNet;

    using Hangfire;
    using Hangfire.Dashboard;
    using Hangfire.SqlServer;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.ML;

    using Stripe;

    using XeMart.Common;
    using XeMart.Data;
    using XeMart.Data.Common;
    using XeMart.Data.Common.Repositories;
    using XeMart.Data.Models;
    using XeMart.Data.Repositories;
    using XeMart.Data.Seeding;
    using XeMart.Services;
    using XeMart.Services.CronJobs;
    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Services.Messaging;
    using XeMart.Web.Hubs;
    using XeMart.Web.ViewModels;
    using XeMart.Web.ViewModels.Recommender;

    using Account = CloudinaryDotNet.Account;

    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(this.configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true,
                }));

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddPredictionEnginePool<ProductInfo, ProductPrediction>()
                .FromFile($"{this.webHostEnvironment.WebRootPath}\\Recommender\\model.zip", true);

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                        options.Secure = CookieSecurePolicy.Always;
                    });

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = this.configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheRecords";
            });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            Account cloudinaryCredentials = new Account(
                this.configuration["Cloudinary:CloudName"],
                this.configuration["Cloudinary:ApiKey"],
                this.configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);
            services.AddSingleton(cloudinaryUtility);

            services.AddSingleton(x => new BlobServiceClient(this.configuration["BlobConnectionString"]));

            StripeConfiguration.ApiKey = this.configuration["Stripe:SecretKey"];

            services.AddSignalR();

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddApplicationInsightsTelemetry();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<IStringService, StringService>();
            services.AddTransient<ITimeSpanService, TimeSpanService>();
            services.AddTransient<IImagesService, ImagesService>();

            // Data services
            services.AddTransient<IUserMessagesService, UserMessagesService>();
            services.AddTransient<ISuppliersService, SuppliersService>();
            services.AddTransient<IMainCategoriesService, MainCategoriesService>();
            services.AddTransient<ISubcategoriesService, SubcategoriesService>();
            services.AddTransient<IPartnersService, PartnersService>();
            services.AddTransient<IProductsService, ProductsService>();
            services.AddTransient<IFavouritesService, FavouritesService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<IAddressesService, AddressesService>();
            services.AddTransient<ICountriesService, CountriesService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IHomePageSlidesService, HomePageSlidesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

                recurringJobManager.AddOrUpdate<UpdateRecommenderModelJob>("Update Recommender", x => x.Work(env.WebRootPath), Cron.Daily);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsProduction())
            {
                app.UseHangfireServer();
                app.UseHangfireDashboard(
                    "/Administration/Hangfire",
                    new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
            }

            app.UseSession();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapHub<ChatHub>("/chat");
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }

        private class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var httpContext = context.GetHttpContext();
                return httpContext.User.IsInRole(GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
