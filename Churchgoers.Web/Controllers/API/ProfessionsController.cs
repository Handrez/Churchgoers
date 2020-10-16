using Churchgoers.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Churchgoers.Web.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProfessionsController(DataContext context)
        { 
            _context = context;
        }

        [HttpGet]
        public IActionResult GetProfessions()
        {
            return Ok(_context.Professions);
        }
    }
}
