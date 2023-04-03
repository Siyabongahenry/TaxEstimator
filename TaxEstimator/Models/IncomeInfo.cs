using System.ComponentModel.DataAnnotations;
using TaxEstimator.Utility;

namespace TaxEstimator.Models
{
    public class IncomeInfo
    {
       
        [Display(Name = "Type of Income")]
        public string Type { get; set; } = IncomeType.Monthly;
        [Display(Name = "Tax Year")]
        public int TaxYear { get; set; } = DateTime.Now.Year + 1;
        [Display(Name = "Income before tax")]
        [DataType(DataType.Currency)]
        public decimal Income { get; set; } = 0;

        public decimal TotalIncomesPerAnnum
        {
            get {

                if (Type == IncomeType.Weekly) return (int)(365 / 7);

                if (Type == IncomeType.AfterTwoWeeks) return (int)(365 / 14);

                if (Type == IncomeType.Monthly) return 12;

                return 1;
            }
        } 

        public Contributions Contributions { get; set; } = new();

        [Display(Name = "Annual Income")]
        [DataType(DataType.Currency)]
        public decimal AnnualIncome
        {
            get
            {
                return Income * TotalIncomesPerAnnum;
            }
        }
        public  decimal AnnualProvidentFund
        {
            get
            {
                return Contributions.ProvidentFund * TotalIncomesPerAnnum;
            }
        }

        public decimal AnnualTravelAllowance
        {
            get
            {
                return Contributions.TravelAllowance * TotalIncomesPerAnnum;
            }
        }
        public decimal TaxableIncome
        {
            get
            {
                decimal AnnualIncomeWith80Travel = AnnualIncome - ((20 / 100) * AnnualTravelAllowance);

                return AnnualIncomeWith80Travel - AnnualProvidentFund;
            }
        }

        public decimal AnnualUIF
        {
            get
            {
                return Contributions.UIF * TotalIncomesPerAnnum;
            }
        }
        public Employee Employee { get; set; } = new();

    }
}
