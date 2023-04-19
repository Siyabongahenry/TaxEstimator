using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Threshold
    {
        [Key]
        public int? Id { get; set; }
        [Display(Name = "Under 65")]
        [DataType(DataType.Currency)]
        public decimal LowAge { get; set; }

        [Display(Name = "65 and older")]
        [DataType(DataType.Currency)]
        public decimal MidAge { get; set; }

        [Display(Name = "75 and older")]
        [DataType(DataType.Currency)]
        public decimal OldAge { get; set; }
        [Display(Name = "Tax Year")]
        public int Year { get; set; }
    }
}
