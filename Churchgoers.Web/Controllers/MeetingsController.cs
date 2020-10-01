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
        private readonly IBlobHelper _blobHelper;
        private readonly ICombosHelper _combosHelper;

        public MeetingsController(IUserHelper userHelper, DataContext context, IBlobHelper blobHelper, ICombosHelper combosHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _blobHelper = blobHelper;
            _combosHelper = combosHelper;
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            ViewData["ChurchName1"] = user.Church.Name;

            return View(await _context.Meetings
               .Include(c => c.Church)
               .Include(a => a.Assistances)
               .ToListAsync());
        }

        [Authorize(Roles = "Teacher")]
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

        [Authorize(Roles = "User")]
        public async Task<IActionResult> IndexUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            ViewData["Email"] = user.Email;

            return View(await _context.Assistances
                 .Include(c => c.Meeting)
                 .Include(s => s.User)
                 .ToListAsync());
        }
    }
}