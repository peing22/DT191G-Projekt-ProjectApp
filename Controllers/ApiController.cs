using ProjectApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Route("api/project")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/project
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            // Kontrollerar om null
            if (_context.Projects == null)
            {
                return NotFound();
            }

            // Hämtar alla projekt från databasen och formaterar resultatet
            var projects = await _context.Projects
                .Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    description = p.Description,
                    url = p.Url,
                    imageName = p.ImageName,
                    techniques = p.Techniques.OrderBy(t => t.Name).Select(t => t.Name).ToList()
                })
                .ToListAsync();

            // Returnerar OK-repons och listan med projekt
            return Ok(projects);
        }
    }
}
