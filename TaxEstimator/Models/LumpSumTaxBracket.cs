namespace TaxEstimator.Models
{
    public class LumpSumTaxBracket
    {
        public int Id { get; set; }

        public decimal From { get; set; }

        public decimal To { get; set; }
        public decimal Base { get; set; }
        public decimal Margin { get; set; }
    }
}
