using OpenQA.Selenium.DevTools.V110.CSS;
using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class TaxBracket
    {
        [Key]
        public int? Id { get; set; }
        public int Year { get; set; }
        [DataType(DataType.Currency)]
        public decimal From { get; set; } = 0;

        [DataType(DataType.Currency)]
        public decimal To { get; set; } = 0;

        //tax amount added before adding the tax rate for the salary
        [DataType(DataType.Currency)]
        public decimal Base { get; set; } = 0;
        //as decimals not percentage
        public decimal Margin { get; set; } = 0;
    }
}
