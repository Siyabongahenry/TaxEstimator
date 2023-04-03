using Microsoft.AspNetCore.Mvc;
using TaxEstimator.Models;

namespace TaxEstimator.Controllers
{
    public class PSfundsController : Controller
    {
        public IActionResult Index(PSFund fund)
        {
            fund ??= new PSFund();

            return View(fund);
        }

       
    }
}
