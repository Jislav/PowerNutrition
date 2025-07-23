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

            if(supplement != null)
            {
                return this.View(supplement);
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
