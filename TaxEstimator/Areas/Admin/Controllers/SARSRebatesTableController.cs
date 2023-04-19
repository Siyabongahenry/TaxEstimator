using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Data;
using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "write")]
    public class SARSRebatesTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SARSRebatesTableController(ApplicationDbContext db) 
        { 
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var rebateLst = await _db.TaxRebates.ToListAsync();

            return View(rebateLst);
        }

        public async Task<IActionResult> Create(int? id)
        {
            if(id == null) return View(new Rebate());

            Rebate rebate = await _db.TaxRebates.FirstAsync(r=>r.Id == id);

            return View(rebate);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Rebate rebate)
        {
            if(!ModelState.IsValid) return View(rebate);
            
            bool exist= _db.TaxRebates.Any(b=>b.Year == rebate.Year);

            if (exist)
            {
                
            }
            else
            {
                _db.Add(rebate);
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
