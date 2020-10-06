using Churchgoers.Web.Data;
using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Churchgoers.Web.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public MeetingsController(IUserHelper userHelper, DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if(User.IsInRole("Admin"))
            {
                return View(await _context.Meetings
               .Include(c => c.Church)
               .Include(a => a.Assistances)
               .ToListAsync());
            }
            else
            {
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                ViewData["ChurchName1"] = user.Church.Name;

                return View(await _context.Meetings
                   .Include(c => c.Church)
                   .Include(a => a.Assistances)
                   .ToListAsync());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Meeting @Meeting = await _context.Meetings
                 .Include(c => c.Church)
                 .ThenInclude(s => s.Users)
                 .Include(a => a.Assistances)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@Meeting == null)
            {
                return NotFound();
            }

            return View(@Meeting);
        }
    }
}