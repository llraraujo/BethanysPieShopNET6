using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dependecy Injection of Services
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();


//enables MVC
builder.Services.AddControllersWithViews();

//Add DbContext to the App
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
DbInitializer.Seed(app);
app.Run();
