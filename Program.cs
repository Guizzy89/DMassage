using DMassage.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Подключение к локальной базе данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=DMassage.db"));

var app = builder.Build();

app.MapDefaultControllerRoute();

app.Run();
