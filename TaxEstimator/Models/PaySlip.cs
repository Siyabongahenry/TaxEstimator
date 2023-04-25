using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class PaySlip
    {
        [Display(Name ="Date:")]
        [DataType(DataType.Date)]
        public DateTime Date { get {  return DateTime.Now; } }
        public TaxBracket TaxBracket { get; set; } = new();
        public WorkerIncome Income { get; set; } = new();

        [DataType(DataType.Currency)]
        public decimal Rebate { get; set; }

        [DataType(DataType.Currency)]
        public decimal Threshold { get; set; }

        [Display(Name = "PAYE")]
        [DataType(DataType.Currency)]
        public decimal PAYE
        {
            get
            {

                if (Income.AnnualTaxableIncome <= Threshold) return 0;


                decimal taxBeforeRebate = TaxBracket.Base
                    + (TaxBracket.Margin/100) *
                    ((Income.AnnualTaxableIncome) - (TaxBracket.From - 1));

                return (taxBeforeRebate - Rebate) / Income.TotalIncomesPerAnnum;
            }
        }

        [Display(Name = "Take Home")]
        [DataType(DataType.Currency)]
        public decimal TakeHome
        {
            get
            {
                decimal takeHome = Income.IncomeAmount;

                for(int i = 0;i< Income.Contributions.Count;i++)
                {
                    if (!Income.Contributions[i].IsDeductable) {
                        continue;
                    }

                    takeHome -= Income.Contributions[i].Amount;
                }

                return takeHome - PAYE;
            }
        }
        
       
        
    }
}
