using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.ViewModels
{
    public class TaxBracketsViewModel
    {
        public List<TaxBracket> TaxBracketList { get; set; } = new();
        public TaxBracket TaxBracket { get; set; } = new();

        public int TaxYear { get; set; } = DateTime.Now.Year - 1;
    }
}
