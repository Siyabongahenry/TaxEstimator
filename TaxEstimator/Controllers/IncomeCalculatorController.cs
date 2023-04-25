using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxEstimator.Models;
using TaxEstimator.Services;

namespace TaxEstimator.Controllers
{
    [AllowAnonymous]
    public class IncomeCalculatorController : Controller
    {
        private ISARSDataExtractor _dataExtractor;
        public IncomeCalculatorController(ISARSDataExtractor dataExtractor)
        {
            _dataExtractor = dataExtractor;
        }
        public IActionResult Index()
        {
            return View("index",new PaySlip());
        }
        [HttpPost]
        public IActionResult GeneratePaySlip(WorkerIncome incomeInfo)
        {
           
            PaySlip paySlip = new()
            {
                Income = incomeInfo,
                TaxBracket = _dataExtractor.GetIncomeTaxBracket(incomeInfo.TaxYear, incomeInfo.AnnualTaxableIncome),
                Rebate = _dataExtractor.GetEmployeeTaxRebate(incomeInfo.TaxYear,incomeInfo.Employee.Age),
                Threshold = _dataExtractor.GetEmployeeThreshold(incomeInfo.TaxYear, incomeInfo.Employee.Age),
            };

            return View("index",paySlip);
        }
    }
}
