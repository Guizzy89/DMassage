using DMassage.Data;
using DMassage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            var bookings = await _context.Bookings.OrderByDescending(b => b.SlotDate).ToListAsync();
            return View(bookings);
        }

        // Добавление нового сеанса (GET-запрос)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Сохранение нового сеанса (POST-запрос)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // Редактирование сеанса (GET-запрос)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // Обновление данных сеанса (POST-запрос)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(booking);
        }

        // Подтверждение удаления сеанса (GET-запрос)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // Удаление сеанса (POST-запрос)
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Запись клиента на сеанс (GET-запрос)
        [HttpGet]
        public async Task<IActionResult> Confirm(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || !booking.IsAvailable)
            {
                return NotFound();
            }
            return View(booking);
        }

        // Завершение процесса записи клиента (POST-запрос)
        [HttpPost]
        public async Task<IActionResult> Confirm(int id, string clientName, string phoneNumber, string comment)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null || !booking.IsAvailable)
            {
                return NotFound();
            }

            booking.ClientName = clientName;
            booking.PhoneNumber = phoneNumber;
            booking.Comment = comment;
            booking.IsAvailable = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}