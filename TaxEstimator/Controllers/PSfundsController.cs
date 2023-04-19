using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxEstimator.Models;
using TaxEstimator.Services;
using TaxEstimator.ViewModels;

namespace TaxEstimator.Controllers
{
    [AllowAnonymous]
    public class PSfundsController : Controller
    {
        private ISARSDataExtractor _dataExtractor;

        public PSfundsController(ISARSDataExtractor dataExtractor)
        {
            _dataExtractor = dataExtractor;
        }
        public IActionResult Index()
        {

            return View(new PSFundsViewModel());
        }
        [HttpPost]
        public IActionResult DoCalculations(PSFundsViewModel PSFundsVM)
        {
            PSFundsVM.PSFundWithdrawals.PSFund = PSFundsVM.PSFund;

            decimal taxableIncome = PSFundsVM.PSFundWithdrawals.WidthdrawalAmount - PSFundsVM.PSFund.TaxFreePortion;

            PSFundsVM.PSFundWithdrawals.TaxBracket = _dataExtractor
                .GetPSFundTaxBracket(PSFundsVM.PSFundWithdrawals.TaxYear, taxableIncome);

            return View("Index",PSFundsVM);
        }
       
    }
}
