namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Services.Core;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Supplement;
    public class SupplementController : BaseController
    {
        private readonly ISupplementService supplementService;
        private readonly ICategoryService categoryService;

        public SupplementController(ISupplementService supplementService, ICategoryService categoryService)
        {
            this.supplementService = supplementService;
            this.categoryService = categoryService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllSupplementsViewmodel> allSupplements = await this.supplementService
                .GetAllSupplementsAsync();

            return View(allSupplements);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? id)
        {
            DetailsSupplementViewmodel? supplement = await this.supplementService
                .GetDetailsForSupplementAsync(id);

            if (supplement != null)
            {
                return this.View(supplement);
            }

            return this.RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ICollection<SupplementCategoryDropDownFilterViewmodel> categories = await this.categoryService
                .GetAllCategoriesAsync();

            AddSupplementInputModel inputModel = new AddSupplementInputModel()
            {
                Categories = categories
            };
            return this.View(inputModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddSupplementInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");
                inputModel.Categories = await this.categoryService.GetAllCategoriesAsync();
                return this.View(inputModel);
            }

            Guid? supplementId = await this.supplementService.PersistAddSupplementAsync(inputModel);

            if (supplementId == null)
            {
                Console.WriteLine("supplement id is null");
                inputModel.Categories = await this.categoryService.GetAllCategoriesAsync();
                return this.View(inputModel);
            }
            Console.WriteLine("everything workds");
            return this.RedirectToAction(nameof(Details), controllerName: "Supplement", new { id = supplementId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? supplementId)
        {
            SupplementDeleteInputModel? inputModel = await this.supplementService
                .GetSupplementToDelete(supplementId);

            if (inputModel != null)
            {
                return this.View(inputModel);
            }

            return this.RedirectToAction("Delete", "Manage");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(SupplementDeleteInputModel inputModel)
        {
            bool taskResult = await this.supplementService
                .DeleteSupplement(inputModel);

            if (taskResult == false)
            {
                //TODO: Handle notifications pop up or something else
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("Delete", "Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? supplementId)
        {
            SupplementEditInputModel? supplementViewmodel = await this.supplementService
                .GetSupplementForEditAsync(supplementId);

            if (supplementViewmodel != null)
            {
                supplementViewmodel.Categories = await this.categoryService
                .GetAllCategoriesAsync();
            }
            return this.View(supplementViewmodel);
        }
        public async Task<IActionResult> Edit(SupplementEditInputModel inputModel)
        {
            if(!ModelState.IsValid)
            {
                inputModel.Categories = await this.categoryService
               .GetAllCategoriesAsync();

                return this.View(inputModel);
            }

            Guid? editedSupplementId = await this.supplementService
                .PersistEditSupplementAsync(inputModel);

            if(editedSupplementId == null)
            {
                inputModel.Categories = await this.categoryService
                .GetAllCategoriesAsync();

                return this.View(inputModel);

            }
            return this.RedirectToAction(nameof(Details), new { id = editedSupplementId });
        }
    }
}
