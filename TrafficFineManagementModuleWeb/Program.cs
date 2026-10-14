using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.LogoutPath = "/Account/Logout";
        o.AccessDeniedPath = "/Account/AccessDenied";
        o.ExpireTimeSpan = TimeSpan.FromHours(8);
        //SlidingExpiration, kullanıcı aktif olduğu sürece oturum süresini yeniler; 8 saat boyunca hiç işlem yapmazsa oturum düşer.
        o.SlidingExpiration = true;
    });

// Aksi belirtilmedikçe tüm sayfalar giriş gerektirir
///<summary>
///FallbackPolicy, her controller’a tek tek [Authorize] yazmak yerine “varsayılan olarak her şey giriş gerektirsin” demek. 
///Sadece login sayfası gibi açık olması gereken yerlere [AllowAnonymous] koyacağız. 
///Böylece yeni bir controller eklerken [Authorize] yazmayı unutup sayfayı herkese açık bırakma riski ortadan kalkar.
///</summary>
builder.Services.AddAuthorization(o =>
{
    o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITrafficFineDal, EfTrafficFineDal>();
builder.Services.AddScoped<IAppUserDal, EfAppUserDal>();
builder.Services.AddScoped<IVehicleDal, EfVehicleDal>();
builder.Services.AddScoped<IDriverDal, EfDriverDal>();
builder.Services.AddScoped<ITrafficFineTypeDal, EfTrafficFineTypeDal>();
builder.Services.AddScoped<ITrafficFineService, TrafficFineManager>();
builder.Services.AddScoped<IAppUserService, AppUserManager>();
builder.Services.AddScoped<IVehicleService, VehicleManager>();
builder.Services.AddScoped<IDriverService, DriverManager>();
builder.Services.AddScoped<ITrafficFineTypeService, TrafficFineTypeManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}
app.UseHttpsRedirection();

// Form verisi kültürden bağımsız okunur (ondalık ayırıcı her zaman nokta).
// Ekranda gösterim ise FormatExtensions ile açıkça Türkçe yapılır.
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture),
    SupportedCultures = new[] { CultureInfo.InvariantCulture },
    SupportedUICultures = new[] { CultureInfo.InvariantCulture }
});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//Fallback policy, CSS ve JS dosyalarına da giriş zorunluluğu getirebilir ve login sayfası tasarımsız görünür.
//Düzeltelim app.MapStaticAssets().AllowAnonymous();
app.MapStaticAssets().AllowAnonymous();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
