using System.ComponentModel.DataAnnotations;
using TaxEstimator.Utility;

namespace TaxEstimator.Models
{
    public abstract class Income
    {
        public decimal Amount { get; set; }
        public string? Type { get; set; }
        public int TaxYear { get; set; }
        public decimal TotalIncomesPerAnnum
        {
            get
            {

                if (Type == IncomeType.Weekly) return (int)(365 / 7);

                if (Type == IncomeType.AfterTwoWeeks) return (int)(365 / 14);

                if (Type == IncomeType.Monthly) return 12;

                if (Type == IncomeType.Quartely) return 4;

                if (Type == IncomeType.HalfYearly) return 2;

                return 1;
            }
        }
        [Display(Name = "Tax Rebate")]
        public decimal Rebate { get; set; }
        public decimal Threshold { get; set; }
        public decimal PAYE { get; }
        public decimal AnnualTaxableIncome { get; set; }


    }
    
    
}
