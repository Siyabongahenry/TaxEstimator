using Microsoft.AspNetCore.Mvc;

namespace TaxEstimator.Controllers
{
    public class TaxTablesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
