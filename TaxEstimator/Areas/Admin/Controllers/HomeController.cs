using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaxEstimator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "write")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
