using TaxEstimator.Models;

namespace TaxEstimator.ViewModels
{
    public class PSFundsViewModel
    {
        public PSFund PSFund { get; set; } = new();
        public PSFundWithdrawals PSFundWithdrawals { get; set; } = new();
    }
}
