using Churchgoers.Common.Requests;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public MeetingsController(DataContext context, IUserHelper userHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
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
                .ThenInclude(a => a.User)
                .ThenInclude(u => u.Profession)
                .Where(m => m.Church.Id == user.Church.Id)
                .OrderByDescending(m => m.Date)
                .ToListAsync();

            return Ok(_converterHelper.ToMeetingResponse(meetings));
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
                        User = users
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
            return Ok(_converterHelper.ToMeetingResponse(meeting));
        
        }

        [HttpPut]
        public async Task<IActionResult> PutMeeting([FromBody] MeetingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            Meeting meeting = await _context.Meetings
                 .Include(m => m.Assistances)
                 .ThenInclude(a => a.User)
                 .ThenInclude(u => u.Church)
                 .Include(m => m.Assistances)
                 .ThenInclude(a => a.User)
                 .ThenInclude(u => u.Profession)
                 .FirstOrDefaultAsync(m => m.Date.Year == request.Date.Year &&
                                          m.Date.Month == request.Date.Month &&
                                          m.Date.Day == request.Date.Day &&
                                          m.Church.Id == user.Church.Id);

            if (meeting == null)
            {
                //TODO:Pendiente para traducir
                return NotFound("Error006");
            }

            bool i = request.Assistances
            .Where(assis => assis.Id == 1)
            .Select(assis => assis.IsPresent)
            .FirstOrDefault();

            foreach (Assistance assistance in meeting.Assistances)
            {
                assistance.IsPresent = request.Assistances
                .Where(a => a.Id == assistance.Id)
                .Select(a => a.IsPresent)
                .FirstOrDefault();
            }

            _context.Meetings.Update(meeting);

            await _context.SaveChangesAsync();
            return Ok(_converterHelper.ToMeetingResponse(meeting));
        }
    }
}
