using Microsoft.AspNetCore.Mvc;
using TaxEstimator.Models;
using TaxEstimator.Services;

namespace TaxEstimator.Controllers
{
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
        public IActionResult GeneratePaySlip(IncomeInfo incomeInfo)
        {
            if (!ModelState.IsValid)
            {
                return  View("index",new PaySlip() { IncomeInfo=incomeInfo} );
            }
           
            PaySlip paySlip = new()
            {
                IncomeInfo = incomeInfo,
                TaxBracket = _dataExtractor.GetIncomeTaxBracket(incomeInfo.TaxYear, incomeInfo.TaxableIncome),
                Rebate = _dataExtractor.GetEmployeeTaxRebate(incomeInfo.TaxYear,incomeInfo.Employee.Age),
                Threshold = _dataExtractor.GetEmployeeThreshold(incomeInfo.TaxYear, incomeInfo.Employee.Age),
            };

            return View("index",paySlip);
        }
    }
}
