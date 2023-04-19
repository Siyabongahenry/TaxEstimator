using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class PSFundWithdrawals
    {
        

        public PSFund PSFund { get; set; } = new();
        [Display(Name ="Withdrawal Amount")]
        [DataType(DataType.Currency)]
        public decimal WidthdrawalAmount
        {
            get;set;     
        }

        public decimal TaxableAmount
        {
            get {

                if (PSFund.TaxFreePortion >= WidthdrawalAmount) return 0;

                return WidthdrawalAmount - PSFund.TaxFreePortion; 
            }
        }

        public TaxBracket TaxBracket { get; set; } = new();

        [Display(Name = "Tax Charges")]
        [DataType(DataType.Currency)]
        public decimal TaxCharges
        {
            get
            {
                if (TaxableAmount == 0) return 0;

               //calculating tax
                return TaxBracket.Base
                    + (TaxBracket.Margin / 100) *
                    (TaxableAmount - (TaxBracket.From - 1));
            }
        }
        [Display(Name = "Take Home Amount")]
        [DataType(DataType.Currency)]
        public decimal TakeHomeAmount
        {
            get
            {
                return WidthdrawalAmount - TaxCharges;
            }
        }
        [Display(Name = "Remaining Balance")]
        [DataType(DataType.Currency)]
        public decimal RemainingBalance
        {
            get {
                return PSFund.LumpSumTransfer - WidthdrawalAmount;
            }
        }
        [Display(Name ="Monthly Income(10 years)")]
        [DataType(DataType.Currency)]
        public decimal AnnuityMonthlyIncome
        {
            get
            {
                return RemainingBalance / 120;
            }
        }

        [Display(Name ="Tax Year")]
        public int TaxYear { get; set; }
    }
}
