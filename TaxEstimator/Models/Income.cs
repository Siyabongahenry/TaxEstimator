using OpenQA.Selenium.DevTools.V108.Browser;
using System.ComponentModel.DataAnnotations;
using TaxEstimator.Utility;

namespace TaxEstimator.Models
{
    public class Income
    {
        public List<Contribution> Contributions { get; set; } = new List<Contribution>()
        {
            new()
            {
                Name = "UIF",
                Amount=0,
                TaxablePercent = 100,
                IsDeductable=true
            },
            new()
            {
                Name = "Travel Allowance",
                Amount=0,
                TaxablePercent = 80,
                IsDeductable = false
            },
            new()
            {
                Name = "Provident Fund or Pension or RAF",
                Amount=0,
                TaxablePercent=0,
                IsDeductable=true
            }
        };
        [Display(Name = "Type of Income")]
        public string Type { get; set; } = IncomeType.Monthly;
        [Display(Name = "Tax Year")]
        public int TaxYear { get; set; } = DateTime.Now.Year + 1;
        [Display(Name = "Income before tax")]
        [DataType(DataType.Currency)]
        public decimal IncomeAmount { get; set; } = 0;

        public decimal TotalIncomesPerAnnum
        {
            get {

                if (Type == IncomeType.Weekly) return (int)(365 / 7);

                if (Type == IncomeType.AfterTwoWeeks) return (int)(365 / 14);

                if (Type == IncomeType.Monthly) return 12;

                return 1;
            }
        }

       
        

        [Display(Name = "Annual Income")]
        [DataType(DataType.Currency)]
        public decimal GrossIncome
        {
            get
            {
               
                return IncomeAmount * TotalIncomesPerAnnum;
            }
        }
       
        public decimal AnnualTaxableIncome
        {
            get
            {
                decimal taxableIncome = GrossIncome;

                for(int i = 0;i< Contributions.Count;i++)
                {
                    taxableIncome -= Contributions[i].TaxExempt*TotalIncomesPerAnnum;
                }
                return taxableIncome;
            }
        }

        public Employee Employee { get; set; } = new();

    }
}
