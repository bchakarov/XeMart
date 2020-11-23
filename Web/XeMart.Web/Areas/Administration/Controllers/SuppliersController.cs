namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
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

            await this.suppliersService.CreateAsync<CreateSupplierInputViewModel>(model);

            this.TempData["Alert"] = "Successfully created supplier.";

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult All()
        {
            var suppliers = this.suppliersService.GetAll<SupplierViewModel>();
            return this.View(suppliers);
        }

        public async Task<IActionResult> MakeDafault(int id)
        {
            var makeDefaultResult = await this.suppliersService.MakeDafaultAsync(id);
            if (makeDefaultResult)
            {
                this.TempData["Alert"] = "Successfully changed default supplier.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem changing the default supplier.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Edit(int id)
        {
            var supplier = this.suppliersService.GetById<EditSupplierViewModel>(id);
            if (supplier == null)
            {
                this.TempData["Error"] = "Supplier not found.";
                return this.RedirectToAction(nameof(this.All));
            }

            return this.View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSupplierViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var editResult = await this.suppliersService.EditAsync<EditSupplierViewModel>(model);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited supplier.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the supplier.";
            }

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.suppliersService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted supplier.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the supplier.";
            }

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
