using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.ViewModels
{
    public class TaxRebatesViewModel
    {
        public List<Rebate> RebateList { get; set; } = new();
        public Rebate Rebate { get; set; }=new();
    }
}
