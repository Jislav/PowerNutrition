using Microsoft.AspNetCore.Mvc;

namespace PowerNutrition.Web.Controllers
{
    public class ManageController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
