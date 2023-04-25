using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.ViewModels
{
    public class TaxThresholdViewModel
    {
        public List<Threshold> ThresholdList { get; set; } = new();
        public Threshold Threshold { get; set; } = new();
    }
}
