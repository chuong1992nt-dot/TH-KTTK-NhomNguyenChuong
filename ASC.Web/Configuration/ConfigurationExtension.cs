using ASC.DataAccess.Interfaces;
using ASC.DataAccess.Repository;
using ASC.Web.Data;
using ASC.Web.Models;
using ASC.Web.Services;

namespace ASC.Web.Configuration
{
    public static class ConfigurationExtension
    {
        // Nhóm 1: Cấu hình các thiết lập từ appsettings.json
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            return services;
        }

        // Nhóm 2: Đăng ký các Interface và Class tương ứng (Dependency Injection)
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentitySeed, IdentitySeed>();
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSession();

            return services;
        }
    }
}