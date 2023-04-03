using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Rebate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Year { get; set; }
        [Display(Name = "Primary Rebate")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Primary { get; set; }
        [Display(Name ="Secondary Rebate")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Secondary { get; set; }
        [Display(Name ="Tertiary Rebate")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Tertiary { get; set;}
    }
}
