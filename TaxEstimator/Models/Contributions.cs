using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Contributions
    {
        [Display(Name = "UIF(1%)")]
        [DataType(DataType.Currency)]
        public decimal UIF { get; set; }

        [Display(Name = "Provident Fund/Pension/RAF")]
        [DataType(DataType.Currency)]
        public decimal ProvidentFund { get; set; } = 0;

        [Display(Name = "Travel Allowance")]
        [DataType(DataType.Currency)]
        public decimal TravelAllowance { get; set; } = 0;
    }
}
