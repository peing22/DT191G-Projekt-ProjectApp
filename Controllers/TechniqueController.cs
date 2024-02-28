using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjectApp.Data;
using ProjectApp.Models;

namespace ProjectApp.Controllers
{
    [Authorize]
    public class TechniqueController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechniqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Technique
        public async Task<IActionResult> Index()
        {
            var techniques = await _context.Techniques
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(techniques);
        }

        // GET: Technique/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniqueModel = await _context.Techniques
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techniqueModel == null)
            {
                return NotFound();
            }

            return View(techniqueModel);
        }

        // GET: Technique/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technique/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TechniqueModel techniqueModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(techniqueModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(techniqueModel);
        }

        // GET: Technique/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniqueModel = await _context.Techniques.FindAsync(id);
            if (techniqueModel == null)
            {
                return NotFound();
            }
            return View(techniqueModel);
        }

        // POST: Technique/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TechniqueModel techniqueModel)
        {
            if (id != techniqueModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(techniqueModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechniqueModelExists(techniqueModel.Id))
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
            return View(techniqueModel);
        }

        // GET: Technique/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techniqueModel = await _context.Techniques
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techniqueModel == null)
            {
                return NotFound();
            }

            return View(techniqueModel);
        }

        // POST: Technique/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var techniqueModel = await _context.Techniques.FindAsync(id);
            if (techniqueModel != null)
            {
                _context.Techniques.Remove(techniqueModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechniqueModelExists(int id)
        {
            return _context.Techniques.Any(e => e.Id == id);
        }
    }
}
