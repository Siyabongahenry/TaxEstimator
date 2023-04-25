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
    public class SARSTaxTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ISARSDataExtractor _dataExtractor;

        public SARSTaxTableController(ApplicationDbContext db,ISARSDataExtractor dataExtractor)
        {
            _db = db;
            _dataExtractor = dataExtractor;
        }
        public async Task<IActionResult> Index(int year = 2024)
        {
            var taxBrackets = await _db.TaxBrackets
                .Where(bracket=>bracket.Year == year)
                .AsNoTracking()
                .OrderBy(b=>b.From)
                .ToListAsync();

            TaxBracketsViewModel model = new TaxBracketsViewModel()
            {
                TaxBracketList = taxBrackets,
                TaxYear = year
            };

            return View(model);
        }

        public async Task<IActionResult> Create(int? year)
        {
         
            if(year != null)
            {
                var taxBrackets = await _db.TaxBrackets.Where(bracket => bracket.Year == year).ToListAsync();
                return View(new TaxBracketsViewModel()
                    {
                        TaxBracketList = taxBrackets,
                        TaxYear = (int)year,
                        TaxBracket = new TaxBracket() { Year = (int)year }
                    }
                );
            }
            return View(new TaxBracketsViewModel());
        }

        public async Task<IActionResult> ExtractFromSARS(int? year)
        {
            if (year == null) return View("Create");

        
            TaxBracketsViewModel vm = new TaxBracketsViewModel()
            {
                TaxBracketList =await _dataExtractor.GetTaxBracketListAsync((int)year),
                TaxBracket = new TaxBracket() { Year= (int)year },
                TaxYear = (int)year
            };
            return View("Create",vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TaxBracketsViewModel lstVM)
        {
            if(!ModelState.IsValid) return View(lstVM);

            bool bracketExist = lstVM
                .TaxBracketList
                .Exists(bracket => lstVM.TaxBracket.From == bracket.From
                    && lstVM.TaxBracket.To == bracket.To);
            
            //adding a new row in the column
            if ( lstVM.TaxBracket.To > 0 && lstVM.TaxBracket.Year > 0 && !bracketExist)
            {
                lstVM.TaxBracketList.Add(lstVM.TaxBracket);
              
            }
            
            foreach(TaxBracket bracket in lstVM.TaxBracketList)
            {
                //skip bracket with zero inputs
                var b =await _db.TaxBrackets.FirstOrDefaultAsync(b=>b.Id == bracket.Id);

                //skip if bracket exist
                if (b != null && b.From == bracket.From 
                    && b.To == bracket.To && b.Margin == bracket.Margin 
                    && b.Base == bracket.Base) continue;

                //remove existing bracket to add an updated one
                if(b != null) _db.Remove(b);
                

                _db.Add(bracket);
                
            }

            await _db.SaveChangesAsync();

            return View(lstVM);
        }

        public async Task<IActionResult> DeleteRow(int id,int year)
        {
            var bracket = await _db.TaxBrackets.FirstOrDefaultAsync(b => b.Id == id);

            if(bracket != null)
            {
                _db.Remove(bracket);

                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Create",new { year});
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
