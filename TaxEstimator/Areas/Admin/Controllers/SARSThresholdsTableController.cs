using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Data;
using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "write")]
    public class SARSThresholdsTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public SARSThresholdsTableController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var thresholdLst = await _db.TaxThresholds.ToListAsync();

            return View(thresholdLst);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null) return View(new Threshold());
            
            Threshold threshold = await _db.TaxThresholds.FirstAsync(t=>t.Id == id);

            return View(new Threshold());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Threshold threshold)
        {
            bool exist = await _db.TaxThresholds.AnyAsync(b=>b.Year == threshold.Year);

            if (exist)
            {

            }
            else
            {
                _db.Add(threshold);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
