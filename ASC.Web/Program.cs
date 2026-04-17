using ASC.DataAccess;
using ASC.DataAccess.Interfaces;
using ASC.DataAccess.Repository;
using ASC.Web.Configuration;
using ASC.Web.Data;
using ASC.Web.Services;
using ASC.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("navigation.json", optional: true, reloadOnChange: true);

// 1. Cấu hình DbContext 
builder.Services.AddDbContext<ASC.Web.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ASC.Web")
    )
);

builder.Services.AddScoped<DbContext, ASC.Web.Data.ApplicationDbContext>();

// 2. Cấu hình Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => {
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ASC.Web.Data.ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Google:Identity");
        options.ClientId = googleAuthNSection["ClientId"];
        options.ClientSecret = googleAuthNSection["ClientSecret"];
    });

// 3. Khai báo MVC và Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// 4. Tiêm các Dependencies khác (Từ file ConfigurationExtension)
builder.Services.AddConfig(builder.Configuration).AddMyDependencyGroup();

// 5. CẤU HÌNH GỬI EMAIL (BẠN CHÈN ĐOẠN NÀY VÀO ĐÂY NHÉ)
// - Đọc thông tin cấu hình từ appsettings.json
builder.Services.Configure<ASC.Web.Configuration.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// - Đăng ký interface của riêng Project (để dùng trong các Controller của bạn sau này)
builder.Services.AddTransient<ASC.Web.Services.IEmailSender, ASC.Web.Services.AuthMessageSender>();
builder.Services.AddTransient<ASC.Web.Services.ISmsSender, ASC.Web.Services.AuthMessageSender>();

// - Đăng ký interface chuẩn của Identity (bắt buộc để nút Đăng ký / Quên mật khẩu hoạt động)
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, ASC.Web.Services.AuthMessageSender>();

builder.Services.Configure<ASC.Web.Configuration.NavigationMenu>(builder.Configuration.GetSection("NavigationMenu"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
 
app.UseRouting();
app.UseSession();

// Bắt buộc phải có Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

// Định tuyến cho Area (ServiceRequests)
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Map đường dẫn cho các trang Identity (Login, Register...)

// Thực thi Seed Data
using (var scope = app.Services.CreateScope())
{
    try
    {
        var identitySeed = scope.ServiceProvider.GetRequiredService<IIdentitySeed>();
        identitySeed.Seed().Wait();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi cmnr khi Seeding Database!");
    }
}

app.Run();