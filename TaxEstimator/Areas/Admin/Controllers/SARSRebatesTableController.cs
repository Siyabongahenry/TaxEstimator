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
    public class SARSRebatesTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISARSDataExtractor _dataExtractor;
        public SARSRebatesTableController(ApplicationDbContext db,ISARSDataExtractor dataExtractor) 
        { 
            _db = db;
            _dataExtractor = dataExtractor;
        }
        public async Task<IActionResult> Index()
        {
            var rebateLst = await _db.TaxRebates.ToListAsync();

            return View(rebateLst);
        }

        public async Task<IActionResult> Create()
        {
            TaxRebatesViewModel vm = new()
            {
                RebateList = await _db.TaxRebates.ToListAsync()
            };

            return View(vm);
        }
        public async Task<IActionResult> ExtractFromSARS()
        {
            var rebateList = await _dataExtractor.GetTaxRebateListAsync();

            TaxRebatesViewModel vm = new()
            {
                RebateList = await _dataExtractor.GetTaxRebateListAsync()
            };

            return View("Create",vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TaxRebatesViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.Rebate.Year != 0 && vm.Rebate.Primary != 0
                && vm.Rebate.Secondary != 0 && vm.Rebate.Tertiary != 0)
            {
                vm.RebateList.Add(vm.Rebate);
            }

            foreach (Rebate r in vm.RebateList)
            {
                if (r.Id  != null)
                {
                    _db.Remove(r);
                }

                _db.Add(r);
            }
           
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.TaxRebates.FindAsync(id);

            if (r != null)
            {
                _db.Remove(r);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Create));
        }
    }
}
