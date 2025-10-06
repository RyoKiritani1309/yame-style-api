using YameApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews(); // Changed to support MVC views

// Register application services
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<ICartService, CartService>();

// Add CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files (CSS, JS, images)
app.UseCors("AllowFrontend");
app.UseRouting();
app.UseAuthorization();

// Map MVC routes (for views)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers
app.MapControllers();

app.Run();
