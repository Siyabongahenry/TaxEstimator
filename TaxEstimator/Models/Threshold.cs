using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class Threshold
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        public decimal LowAge { get; set; }
        [DataType(DataType.Currency)]
        public decimal MidAge { get; set; }
        [DataType(DataType.Currency)]
        public decimal OldAge { get; set; }
        public int Year { get; set; }
    }
}
