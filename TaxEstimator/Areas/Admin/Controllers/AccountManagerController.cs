using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxEstimator.Data;
using TaxEstimator.Models;

namespace TaxEstimator.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy ="write")]
    public class AccountManagerController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly ApplicationDbContext _db;
        public AccountManagerController(UserManager<ApplicationUser> userManager, ApplicationDbContext db) {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _db.ApplicationUser.ToListAsync();

            return View(users);
        }
    }
}
