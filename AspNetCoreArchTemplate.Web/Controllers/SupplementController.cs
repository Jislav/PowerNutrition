namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Category;
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
        public async Task<IActionResult> Index(int? categoryFilter)
        {
            if (categoryFilter.HasValue)
            {
                bool categoryExists = await this.categoryService
                    .CheckIfCategoryExists(categoryFilter);

                if (categoryExists == false)
                {
                    return this.RedirectToAction(nameof(Index));
                }
            }

            SupplementsPageViewModel allSupplements = await this.supplementService
                     .GetAllSupplementsAsync(categoryFilter);

            allSupplements.Categories = await this.categoryService
              .GetAllCategoriesAsync();

            return this.View(allSupplements);
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

            return this.RedirectToAction("Error", "Home", new { statusCode = 404 });
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add()
        {

            ICollection<CategoriesListViewmodel> categories = await this.categoryService
           .GetAllCategoriesAsync();

            AddSupplementInputModel inputModel = new AddSupplementInputModel()
            {
                Categories = categories
            };

            return this.View(inputModel);

        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add(AddSupplementInputModel inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    inputModel.Categories = await this.categoryService.GetAllCategoriesAsync();
                    return this.View(inputModel);
                }

                Guid? supplementId = await this.supplementService.PersistAddSupplementAsync(inputModel);

                if (supplementId == null)
                {
                    inputModel.Categories = await this.categoryService.GetAllCategoriesAsync();
                    return this.View(inputModel);
                }
                return this.RedirectToAction(nameof(Details), controllerName: "Supplement", new { id = supplementId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                inputModel.Categories = await this.categoryService.GetAllCategoriesAsync();
                return this.View(inputModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string? supplementId)
        {
            try
            {
                SupplementDeleteInputModel? inputModel = await this.supplementService
               .GetSupplementToDelete(supplementId);

                if (inputModel != null)
                {
                    return this.View(inputModel);
                }

                return this.RedirectToAction("Error", "Home", new { statusCode = 404 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(SupplementDeleteInputModel inputModel)
        {
            try
            {
                bool taskResult = await this.supplementService
                .DeleteSupplement(inputModel);

                if (taskResult == false)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                return this.RedirectToAction("Delete", "Manage");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string? supplementId)
        {
            try
            {
                SupplementEditInputModel? supplementViewmodel = await this.supplementService
                    .GetSupplementForEditAsync(supplementId);

                if (supplementViewmodel != null)
                {
                    supplementViewmodel.Categories = await this.categoryService
                    .GetAllCategoriesAsync();

                    return this.View(supplementViewmodel);
                }
                return this.RedirectToAction("Error", "Home", new { statusCode = 404 });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return this.RedirectToAction("Error", "Home", new { statusCode = 500 });
            }

        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(SupplementEditInputModel inputModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    inputModel.Categories = await this.categoryService
                        .GetAllCategoriesAsync();

                    return this.View(inputModel);
                }

                Guid? editedSupplementId = await this.supplementService
                    .PersistEditSupplementAsync(inputModel);

                if (editedSupplementId == null)
                {
                    return this.RedirectToAction("Error", "Home", new { statusCode = 404 });
                }
                return this.RedirectToAction(nameof(Details), new { id = editedSupplementId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this.RedirectToAction("Error", "Home", new {statusCode = 500});
            }
        }
    }
}
