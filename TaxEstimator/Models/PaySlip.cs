using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class PaySlip
    {
        [Display(Name ="Date:")]
        [DataType(DataType.Date)]
        public DateTime Date { get {  return DateTime.Now; } }
        public TaxBracket TaxBracket { get; set; } = new();
        public IncomeInfo IncomeInfo { get; set; } = new();

        [DataType(DataType.Currency)]
        public decimal Rebate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Threshold { get; set; }

        [Display(Name = "Annual PAYE")]
        [DataType(DataType.Currency)]
        public decimal AnnualPAYE
        {
            get
            {

                if (IncomeInfo.AnnualIncome <= Threshold) return 0;


                decimal taxBeforeRebate = TaxBracket.Base
                    + TaxBracket.Margin * 
                    ((IncomeInfo.TaxableIncome)  - (TaxBracket.From - 1));

                return taxBeforeRebate - Rebate;
            }
        }

        public decimal AnnualTakeHome
        {
            get
            {
                return IncomeInfo.AnnualIncome
                  - IncomeInfo.AnnualUIF
                  - IncomeInfo.AnnualProvidentFund;
            }
        }
        [Display(Name = "Take Home")]
        [DataType(DataType.Currency)]
        public decimal TakeHome
        {
            get
            {
                return AnnualTakeHome / IncomeInfo.TotalIncomesPerAnnum;
            }
        }
        [Display(Name ="PAYE")]
        [DataType(DataType.Currency)]
        public decimal PAYE
        {
            get
            {
                return AnnualPAYE/IncomeInfo.TotalIncomesPerAnnum;
            }
        }
    }
}
