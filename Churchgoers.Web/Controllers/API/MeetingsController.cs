using Churchgoers.Web.Data;
using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Churchgoers.Web.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;

        public MeetingsController(IUserHelper userHelper, DataContext context)
        {
            _userHelper = userHelper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            List<Meeting> meetings = await _context.Meetings
                .Include(m => m.Church)
                .Include(m => m.Assistances)
                .ThenInclude(u => u.User)
                .ThenInclude(p => p.Profession)
                .Where(m => m.Church.Id == user.Church.Id)
                .ToListAsync();

            return Ok(meetings);
        }












        [HttpPost]
        [Route("{date}")]
        public async Task<IActionResult> PostMeeting(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return NotFound("You must enter a date.");
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            Church church = await _context.Churches
                .Include(u => u.Users)
                .ThenInclude(p => p.Profession)
                .FirstOrDefaultAsync(u => u.Id == user.Church.Id);

            if (church == null)
            {
                return NotFound("Error004");
            }

            Meeting meeting = await _context.Meetings
                .Include(m => m.Church)
                .Include(m => m.Assistances)
                .ThenInclude(u => u.User)
                .ThenInclude(p => p.Profession)
                .FirstOrDefaultAsync(m => m.Date.Year == date.Year &&
                                     m.Date.Month == date.Month &&
                                     m.Date.Day == date.Day &&
                                     m.Church.Id == user.Church.Id);

            bool isNew = false;
            if (meeting == null)
            {
                isNew = true;
                meeting = new Meeting
                {
                    Assistances = new List<Assistance>(),
                    Church = church,
                    Date = date
                };
            }

            foreach (User users in church.Users)
            {
                Assistance assistance = meeting.Assistances.FirstOrDefault(a => a.User.Id == users.Id);
                if (assistance == null)
                {
                    meeting.Assistances.Add(new Assistance
                    {
                        User = user
                    });
                }
            }

            if (isNew)
            {
                _context.Meetings.Add(meeting);
            }
            else
            {
                _context.Meetings.Update(meeting);
            }

            await _context.SaveChangesAsync();
            return Ok(meeting);
        }
    }
}