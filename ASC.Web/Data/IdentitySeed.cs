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

            // 1. Tạo Roles
            var roles = _options.Value.Roles.Split(',');
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Trim()))
                    await _roleManager.CreateAsync(new IdentityRole(role.Trim()));
            }

            // 2. Tạo Admin nếu chưa có
            var adminEmail = _options.Value.AdminEmail;
            var adminPassword = _options.Value.AdminPassword;

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    IsDeleted = false,
                    LockoutEnabled = true
                };

                var result = await _userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

            foreach (var user in _userManager.Users.ToList())
            {
                if (!user.LockoutEnabled)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                }
            }
        }
    }
}
