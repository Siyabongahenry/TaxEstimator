using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Contribution
    {
        public string Name { get; set; } = "";
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; } = 0;
        [DataType(DataType.Currency)]
        public decimal TaxablePercent { set;  get; } = 100;
        [DataType(DataType.Currency)]
        public decimal  TaxExempt { get { 
                return Amount - (TaxablePercent/100)*Amount;
            } 
        }
        //is the Ammount deducted from the gross income
        public bool IsDeductable { get; set; } = false;
    }
}
