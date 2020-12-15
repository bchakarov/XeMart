using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(XeMart.Web.Areas.Identity.IdentityHostingStartup))]

namespace XeMart.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
