namespace PowerNutrition.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Services.Core.Interfaces;
    using PowerNutrition.Web.ViewModels.Manage;
    using PowerNutrition.Web.ViewModels.Supplement;

    public class ManageController : BaseController
    {
        private readonly IManageService manageService;

        public ManageController(IManageService manageService)
        {
            this.manageService = manageService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            IEnumerable<AllSupplementsDeleteViewmodel> supplementsDeleteViewmodel = await this.manageService
                .GetAllSupplementsDeleteListAsync();

            return this.View(supplementsDeleteViewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            IEnumerable<AllSupplemenetsEditViewmodel> supplementsEditViewmodel = await this.manageService
                .GetAllSupplementsEditListAsync();

            return this.View(supplementsEditViewmodel);
        }
        
    }
}
