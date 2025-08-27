// Controllers/BookingController.cs
using DMassage.Data;
using DMassage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMassage.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.Where(b => b.IsAvailable).ToListAsync();
            return View(bookings); // Показываем доступные слоты
        }

        // GET: Booking/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("SlotDate,Duration")] Booking booking)
        {
            if (!ModelState.IsValid)
                return View(booking);

            booking.IsAvailable = true;
            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SlotDate,Duration,IsAvailable")] Booking booking)
        {
            if (!ModelState.IsValid || id != booking.Id)
                return BadRequest();

            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка сохранения: {ex.Message}");
                return View(booking);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            _context.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Получение полной информации по выбранному слоту
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // После внесения данных клиентом обновляем статус слота
        [HttpPost]
        public async Task<IActionResult> Confirm(int id, [Bind("ClientName,PhoneNumber,Comment")] Booking booking)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
                return NotFound();

            existingBooking.ClientName = booking.ClientName;
            existingBooking.PhoneNumber = booking.PhoneNumber;
            existingBooking.Comment = booking.Comment;
            existingBooking.IsAvailable = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Возвращаемся обратно на страницу бронирования
        }
    }
}