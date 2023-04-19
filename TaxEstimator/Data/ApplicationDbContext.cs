using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Models;

namespace TaxEstimator.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<TaxBracket> TaxBrackets { get; set; }

        public DbSet<Threshold> TaxThresholds { get; set; }

        public DbSet<Rebate> TaxRebates { get; set; }
    }
}