namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Data.Models;
    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Administration.Suppliers;

    public class SuppliersController : AdministrationController
    {
        private readonly ISuppliersService suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            this.suppliersService = suppliersService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierInputViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var supplier = AutoMapperConfig.MapperInstance.Map<Supplier>(model);
            await this.suppliersService.Create(supplier);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var suppliers = this.suppliersService.All();

            var supplierViewModels = AutoMapperConfig.MapperInstance.Map<IEnumerable<SupplierViewModel>>(suppliers);

            return this.View(supplierViewModels);
        }

        public async Task<IActionResult> MakeDafault(int id)
        {
            await this.suppliersService.MakeDafault(id);

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            Supplier supplier = this.suppliersService.GetById(id);

            var editViewModel = AutoMapperConfig.MapperInstance.Map<EditSupplierViewModel>(supplier);

            return this.View(editViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSupplierViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.suppliersService.Edit(model.Id, model.Name, model.PriceToHome, model.PriceToOffice);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.suppliersService.Delete(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
