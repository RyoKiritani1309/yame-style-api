using YameApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews(); // Changed to support MVC views

// Add session support for authentication
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register application services - DATABASE BACKED
builder.Services.AddScoped<IProductService, ProductServiceDatabase>();
builder.Services.AddScoped<ICartService, CartServiceDatabase>();
builder.Services.AddScoped<IAccountService, AccountServiceDatabase>();
builder.Services.AddScoped<IOrderService, OrderServiceDatabase>();
builder.Services.AddScoped<IAdminService, AdminServiceDatabase>();

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
app.UseSession(); // Enable session before authorization
app.UseAuthorization();

// Map MVC routes (for views)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map API controllers
app.MapControllers();

app.Run();
