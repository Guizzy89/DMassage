using DMassage.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем службы аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Account/Login";          // Страничка входа
        opt.LogoutPath = "/Account/Logout";        // Страничка выхода
        opt.AccessDeniedPath = "/Account/AccessDenied"; // Страничка отказа в доступе
    });

builder.Services.AddControllersWithViews();

// Подключение к локальной базе данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=DMassage.db"));

var app = builder.Build();

// Включаем процессы аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Стандартные middleware
app.UseStaticFiles();
app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
