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
        public int YearsInPSfunds
        {
            get
            {
                return LeavingDate.Year - JoiningDate.Year;
            }
        }
        [Display(Name = "Total number of years post 1 March 1998")]
        public int YearsPostOneMarch { 
            get {
                return JoiningDate.Year < 1998?(LeavingDate.Year - 1998): YearsInPSfunds;
            } 
        }
        [Display(Name = "Approximate Taxable portion")]
        [DataType (DataType.Currency)]
        public decimal TaxablePortion { 
            get { 
                return YearsPostOneMarch*(LumpSumTransfer/YearsInPSfunds);
            } 
        }
        [Display(Name = "Approximate Tax-free portion")]
        [DataType(DataType.Currency)]
        public decimal TaxFreePortion { 
            get {
                return LumpSumTransfer - TaxablePortion;
            }
        }

    }
}
