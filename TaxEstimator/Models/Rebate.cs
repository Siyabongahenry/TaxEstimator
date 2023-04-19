using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Rebate
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [Display(Name = "Tax Year")]
        public int Year { get; set; }
        [Display(Name = "Primary Rebate")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Primary { get; set; }
        [Display(Name ="Secondary Rebate(65 and older)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Secondary { get; set; }
        [Display(Name ="Tertiary Rebate (75 and older)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Tertiary { get; set;}
    }
}
