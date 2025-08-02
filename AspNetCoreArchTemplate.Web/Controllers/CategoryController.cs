namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Category;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoriesListViewmodel> categories = await this.categoryService
                .GetAllCategoriesAsync();

            return this.View(categories);
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add()
        {
            AddCategoryInputModel inputModel = new AddCategoryInputModel();

            return this.View(inputModel);
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Add(AddCategoryInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            bool taskResult = await this.categoryService
                .AddCategoryAsync(inputModel);

            if (taskResult == false)
            {
                return this.View(inputModel);
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string? id)
        {
            CategoryDeleteInputModel inputModel = await this.categoryService
                .GetCategoryForDeletingAsync(id);

            return this.View(inputModel);
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(CategoryDeleteInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            bool taskResult = await this.categoryService
                .PersistCategoryDelete(inputModel);

            if(taskResult == false)
            {
                return this.View(inputModel);
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
