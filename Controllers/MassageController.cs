// Controllers/MassageController.cs
using DMassage.Data;
using DMassage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMassage.Controllers
{
    public class MassageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MassageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Massage
        public async Task<IActionResult> Index()
        {
            var massages = await _context.Massages.ToListAsync();
            return View(massages); // Показываем список услуг
        }

        // GET: Massage/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var massage = await _context.Massages.FindAsync(id);
            if (massage == null)
                return NotFound();

            return View(massage);
        }

        // POST: Massage/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] Massage massage)
        {
            if (!ModelState.IsValid)
                return View(massage);

            _context.Add(massage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Massage/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var massage = await _context.Massages.FindAsync(id);
            if (massage == null)
                return NotFound();

            return View(massage);
        }

        // POST: Massage/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Massage massage)
        {
            if (!ModelState.IsValid || id != massage.Id)
                return BadRequest();

            try
            {
                _context.Update(massage);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка сохранения: {ex.Message}");
                return View(massage);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Massage/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var massage = await _context.Massages.FindAsync(id);
            if (massage == null)
                return NotFound();

            return View(massage);
        }

        // POST: Massage/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var massage = await _context.Massages.FindAsync(id);
            if (massage == null)
                return NotFound();

            _context.Remove(massage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}