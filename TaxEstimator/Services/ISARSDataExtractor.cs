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
        public TaxBracket GetPSFundTaxBracket(int taxYear,decimal income);

        public Task<List<TaxBracket>> GetTaxBracketListAsync(int year);
        public Task<List<Rebate>> GetTaxRebateListAsync();
        public Task<List<Threshold>> GetTaxThresholdListAsync();


    }
}
