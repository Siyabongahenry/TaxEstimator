using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Areas.Admin.ViewModels;
using TaxEstimator.Data;
using TaxEstimator.Models;
using TaxEstimator.Services;

namespace TaxEstimator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "write")]
    public class SARSThresholdsTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISARSDataExtractor _dataExtractor;
        public SARSThresholdsTableController(ApplicationDbContext db, ISARSDataExtractor dataExtractor)
        {
            _db = db;
            _dataExtractor = dataExtractor;
        }
        public async Task<IActionResult> Index()
        {
            var thresholdLst = await _db.TaxThresholds.ToListAsync();

            TaxThresholdViewModel vm = new()
            {
                ThresholdList = thresholdLst
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            TaxThresholdViewModel vm = new()
            {
                ThresholdList = await _db.TaxThresholds.ToListAsync()
            };

            return View(vm);
        }
        public async Task<IActionResult> ExtractFromSARS()
        {
            var thresholdList = await _dataExtractor.GetTaxThresholdListAsync();

            TaxThresholdViewModel vm = new()
            {
                ThresholdList = thresholdList
            };

            return View("Create", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TaxThresholdViewModel vm)
        {

            if (!ModelState.IsValid) return View(vm); 

            if(vm.Threshold.Year != 0 && vm.Threshold.LowAge != 0 
                && vm.Threshold.MidAge != 0 && vm.Threshold.OldAge != 0)
            {
                vm.ThresholdList.Add(vm.Threshold);
            }
            
            foreach(var threshold in vm.ThresholdList)
            {
                _db.Add(threshold);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.TaxThresholds.FindAsync(id);

            if (r != null)
            {
                _db.Remove(r);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Create));
        }
    }
}
