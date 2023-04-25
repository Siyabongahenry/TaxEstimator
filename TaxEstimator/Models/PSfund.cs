using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public class PSFund
    {
        [Display(Name = "Date of joining Public Sector funds")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = new DateTime(2010,1,1);
        [Display(Name = "Date of leaving Public Sector funds")]
        [DataType(DataType.Date)]
        public DateTime LeavingDate { get; set; } = DateTime.Now;
        [Display(Name = "Public Sector funds lump sum transferred")]
        public decimal LumpSumTransfer { get; set; } = 0;
        [Display(Name = "Total number of years in Public Sector funds")]
        public decimal YearsInPSfunds
        {
            get
            {
                return GetYearAsDecimal(JoiningDate,LeavingDate);
            }
        }
        [Display(Name = "Total number of  years post 1 March 1998")]
        public decimal YearsPostOneMarch { 
            get {
                return JoiningDate.Year < 1998?GetYearAsDecimal(new DateTime(1998,3,1),LeavingDate): YearsInPSfunds;
            } 
        }

        [Display(Name = "Approximate Taxable portion(Pre-1998)")]
        [DataType(DataType.Currency)]
        public decimal TaxablePortion
        {
            get
            {
                return YearsPostOneMarch * (LumpSumTransfer / YearsInPSfunds);
            }
        }
        [Display(Name = "Approximate Tax-free portion")]
        [DataType(DataType.Currency)]
        public decimal TaxFreePortion { 
            get {
                return (LumpSumTransfer - TaxablePortion);
            }
        }

        private decimal GetYearAsDecimal(DateTime date1,DateTime date2)
        {
            decimal year = (date2.Year - date1.Year)
                + (decimal)Math.Abs(date2.Month - date1.Month) / 12
                + (decimal)Math.Abs(date2.Day - date1.Day) / 365;

            return year;
        }

    }
}
