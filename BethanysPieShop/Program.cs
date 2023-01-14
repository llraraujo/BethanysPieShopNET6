using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dependecy Injection of Services
builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IPieRepository, MockPieRepository>();


//enables MVC
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BethanysPieShopDbContext>(
    options => {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:BethanysPieShopContextConnection"]);
    });

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

app.Run();
