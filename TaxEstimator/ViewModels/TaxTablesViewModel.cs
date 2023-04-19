using TaxEstimator.Models;

namespace TaxEstimator.ViewModels
{
    public class TaxTablesViewModel
    {
        
        public IEnumerable<TaxBracket>? TaxBrackets { get; set; }
        public Rebate? Rebate { get; set; }
        public Threshold? Threshold { get; set; } = new();

        public int Year { get; set; }

    }
}
