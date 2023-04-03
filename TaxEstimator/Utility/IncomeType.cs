namespace TaxEstimator.Utility
{
    public class IncomeType
    {
        public static readonly string Weekly = "Weekly";
        public static readonly string AfterTwoWeeks = "After Two Weeks";
        public static readonly string Monthly = "Monthly";
        public static readonly string Yearly = "Yearly";

        public static IEnumerable<string> GetTypes()
        {
            yield return Weekly;
            yield return AfterTwoWeeks;
            yield return Monthly;
            yield return Yearly;
        }
    }
}
