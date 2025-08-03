namespace AspNetCoreArchTemplate.Web.Controllers
{
    using System.Diagnostics;

    using ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using PowerNutrition.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;

    public class HomeController : BaseController
    {
        public HomeController(ILogger<HomeController> logger)
        {

        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }   
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            if(statusCode == 404)
            {
                return this.View("NotFoundError");
            }
            else if(statusCode == 500)
            {
                return this.View("InternalServerError");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
