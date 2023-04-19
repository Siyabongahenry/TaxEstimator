using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Data;
using TaxEstimator.ViewModels;

namespace TaxEstimator.Controllers
{
    [AllowAnonymous]
    public class TaxTablesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaxTablesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int year = 2024)
        {
            TaxTablesViewModel viewModel = new TaxTablesViewModel()
            {
                TaxBrackets = await _db.TaxBrackets.Where(b => b.Year == year).ToListAsync(),
                Rebate = await _db.TaxRebates.FirstOrDefaultAsync(r=>r.Year == year),
                Threshold = await _db.TaxThresholds.FirstOrDefaultAsync(t=>t.Year == year),
                Year = year
                
            };

            return View(viewModel);
        }
    }
}
