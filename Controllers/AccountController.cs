using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMassage.Controllers
{
    public class AccountController : Controller
    {
        // Вход в систему
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Проверка правильности имени пользователя и пароля
            if (CheckCredentials(model.Username, model.Password))
            {
                // Создаем идентификационные данные пользователя
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Авторизуем пользователя
                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Неправильное имя пользователя или пароль.";
                return View(model);
            }
        }

        // Выход из системы
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Простая проверка данных пользователя (можно заменить на реальную базу данных)
        private bool CheckCredentials(string username, string password)
        {
            // Просто пример проверки
            return username == "admin" && password == "password";
        }
    }

    // Модель данных для входа
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}