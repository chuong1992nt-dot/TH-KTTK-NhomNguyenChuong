using ASC.Business.Interfaces;
using ASC.Web.Configuration;
using ASC.Web.Data;
using ASC.Web.Models;
using ASC.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("navigation.json", optional: true, reloadOnChange: true);

// 1. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ASC.Web")
    )
);
builder.Services.AddScoped<DbContext, ApplicationDbContext>();

// 2. Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // false = không cần xác nhận email
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Google OAuth
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        var googleSection = builder.Configuration.GetSection("Google:Identity");
        options.ClientId = googleSection["ClientId"]!;
        options.ClientSecret = googleSection["ClientSecret"]!;
    });

// 4. MVC + Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// 5. AutoMapper - scan tất cả assembly trong project
builder.Services.AddAutoMapper(
    typeof(ASC.Web.Mappings.MappingProfile).Assembly,
    typeof(ASC.Web.Areas.ServiceRequests.Models.ServiceRequestMappingProfile).Assembly
);

// 6. Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, AuthMessageSender>();

// 7. NavigationMenu
builder.Services.Configure<NavigationMenu>(builder.Configuration.GetSection("NavigationMenu"));

// 8. Custom Dependencies (đọc từ ConfigurationExtension.cs)
builder.Services.AddConfig(builder.Configuration).AddMyDependencyGroup();

var app = builder.Build();

//  HTTP Pipeline 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Routes
app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// ── Seed Data ──
using (var scope = app.Services.CreateScope())
{
    try
    {
        // Seed roles + admin user
        var identitySeed = scope.ServiceProvider.GetRequiredService<IIdentitySeed>();
        await identitySeed.Seed();

        // Seed MasterData cache
        var masterDataCache = scope.ServiceProvider.GetRequiredService<IMasterDataCacheOperations>();
        await masterDataCache.CreateMasterDataCacheAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi khởi tạo dữ liệu ban đầu!");
    }
}

app.Run();
