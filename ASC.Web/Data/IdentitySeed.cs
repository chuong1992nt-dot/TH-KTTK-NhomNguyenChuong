using ASC.Web.Configuration;
using ASC.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ASC.Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IOptions<ApplicationSettings> _options;

        // Tự động tiêm mọi thứ qua Constructor
        public IdentitySeed(UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager,
                            ApplicationDbContext context,
                            IOptions<ApplicationSettings> options)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _options = options;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();

            // 1. Lấy danh sách Roles từ appsettings.json và cắt chuỗi
            var roles = _options.Value.Roles.Split(',');

            // 2. Tạo Roles
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Trim()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Trim()));
                }
            }

            // 3. Lấy thông tin Admin từ appsettings.json
            var adminEmail = _options.Value.AdminEmail;
            var adminPassword = _options.Value.AdminPassword;

            // 4. Tạo Admin
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    IsDeleted = false
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}