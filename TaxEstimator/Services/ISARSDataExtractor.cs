using HtmlAgilityPack;
using Microsoft.Build.Framework;
using TaxEstimator.Models;

namespace TaxEstimator.Services
{
    public interface ISARSDataExtractor
    {
        public Task<bool> RetrieveSARSDataAsync();
        public TaxBracket GetIncomeTaxBracket(int taxYear, decimal income);
        public decimal GetEmployeeTaxRebate(int taxYear,int age);
        public decimal GetEmployeeThreshold(int age, int taxYear);

    }
}
