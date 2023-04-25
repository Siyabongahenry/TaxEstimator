using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
   
    public class RetireeIncome:Income
    {
        public decimal PSFundBalance { get; set; }

        [Display(Name ="Income draw down (2.5% - 17.5%)")]
        public decimal IncomeDrawDown
        {
            get;set;
        }

        public decimal Annually
        {
            get
            {
                return (IncomeDrawDown / 100) * IncomeDrawDown;
            }
        }
       
       
        
    }
}
