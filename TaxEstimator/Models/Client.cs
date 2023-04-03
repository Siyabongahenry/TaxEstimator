using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace TaxEstimator.Models
{
    public abstract class Client
    {
        [Display(Name ="First Name")]
        public string FirstName { get; set; } ="";
        [Display(Name ="Last Name")]
        public string LastName { get; set; } = "";
        public int Age { get; set; }
    }
}
