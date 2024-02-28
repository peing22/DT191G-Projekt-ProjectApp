using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjectApp.Data;
using ProjectApp.Models;

namespace ProjectApp.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly string rootPath;

        public ProjectController(ApplicationDbContext context, IWebHostEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
            rootPath = hostEnv.WebRootPath;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            // Hämtar alla projekt inklusive relaterade tekniker från databasen
            var projects = await _context.Projects
                .Include(p => p.Techniques.OrderBy(t => t.Name))
                .ToListAsync();

            return View(projects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar ett specifikt projekt inklusive relaterade tekniker från databasen
            var projectModel = await _context.Projects
                .Include(p => p.Techniques.OrderBy(t => t.Name))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            // Hämtar alla tekniker från databasen och lagrar i ViewBag
            ViewBag.Techniques = _context.Techniques
                .OrderBy(t => t.Name)
                .ToList();

            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Url,ImageFile,Techniques")] ProjectModel projectModel, int[] selectedTechniques)
        {
            if (ModelState.IsValid)
            {
                // Om bildfil är medskickad
                if (projectModel.ImageFile != null)
                {
                    // Anropar UploadImage-metod som returnerar bildens filnamn
                    projectModel.ImageName = await UploadImage(projectModel.ImageFile);
                }
                else
                {   // Om bildfil saknas lagras ett bildnamn (-)
                    projectModel.ImageName = "-";
                }

                // Kopplar de valda teknikerna till projektet
                projectModel.Techniques = _context.Techniques.Where(t => selectedTechniques.Contains(t.Id)).ToList();

                // Lägger till projektet i databasen och sparar ändringarna
                _context.Add(projectModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar ett specifikt projekt inklusive relaterade tekniker från databasen
            var projectModel = await _context.Projects
                .Include(p => p.Techniques.OrderBy(t => t.Name))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectModel == null)
            {
                return NotFound();
            }
            // Hämtar alla tekniker från databasen och lagrar i ViewBag
            ViewBag.Techniques = _context.Techniques
                .OrderBy(t => t.Name)
                .ToList();

            return View(projectModel);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Url,ImageFile,Techniques")] ProjectModel projectModel, int[] selectedTechniques)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Hämtar det befintliga projektet inklusive relaterade tekniker från databasen
                    var existingProject = await _context.Projects
                        .Include(p => p.Techniques.OrderBy(t => t.Name))
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (existingProject == null)
                    {
                        return NotFound();
                    }

                    // Uppdaterar titel, beskrivning och URL
                    existingProject.Title = projectModel.Title;
                    existingProject.Description = projectModel.Description;
                    existingProject.Url = projectModel.Url;

                    // Tar bort befintliga relaterade tekniker
                    existingProject.Techniques.Clear();

                    // Kopplar de valda teknikerna till projektet
                    existingProject.Techniques = _context.Techniques.Where(t => selectedTechniques.Contains(t.Id)).ToList();

                    // Om en ny bildfil har skickats med
                    if (projectModel.ImageFile != null)
                    {
                        // Raderar befintlig bild om ImageName inte är "-"
                        if (existingProject.ImageName != "-")
                        {
                            DeleteImage(existingProject.ImageName!);
                        }

                        // Anropar UploadImage-metod som returnerar bildens filnamn
                        existingProject.ImageName = await UploadImage(projectModel.ImageFile);
                    }

                    // Uppdaterar projektet i databasen och sparar ändringarna
                    _context.Update(existingProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Hämtar det specifika projektet inklusive relaterade tekniker från databasen
            var projectModel = await _context.Projects
                .Include(p => p.Techniques.OrderBy(t => t.Name))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectModel = await _context.Projects.FindAsync(id);
            if (projectModel != null)
            {
                // Raderar befintlig bild om ImageName inte är "-"
                if (projectModel.ImageName != "-")
                {
                    DeleteImage(projectModel.ImageName!);
                }
                _context.Projects.Remove(projectModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        // Metod för att ladda upp bildfil till filsystemet
        private async Task<string> UploadImage(IFormFile imageFile)
        {
            // Skapar ett unikt filnamn för bilden
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;

            // Skapar sökväg till bildfil
            string filePath = Path.Combine(rootPath + "/images", uniqueFileName);

            // Sparar bildfil i katalog
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Returnerar filnamnet för bilden
            return uniqueFileName;
        }

        // Metod för att radera bildfil från filsystemet
        private void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(rootPath + "/images", imageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}