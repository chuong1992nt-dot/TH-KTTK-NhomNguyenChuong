using ASC.Business;
using ASC.Business.Interfaces;
using ASC.DataAccess.Interfaces;
using ASC.DataAccess.Repository;
using ASC.Web.Data;
using ASC.Web.Models;
using ASC.Web.Services;

namespace ASC.Web.Configuration
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            // Cache: dùng MemoryCache thay Redis (không cần cài Redis)
            // Nếu muốn dùng Redis, bật lại dòng này và cài Redis:
            // services.AddStackExchangeRedisCache(options => {
            //     options.Configuration = config["RedisCache:ConnectionString"];
            //     options.InstanceName = config["RedisCache:InstanceName"];
            // });
            services.AddDistributedMemoryCache(); // ← thay Redis bằng Memory

            return services;
        }

        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // DataAccess
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentitySeed, IdentitySeed>();

            // Email / SMS
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            // Business 
            services.AddScoped<IMasterDataOperations, MasterDataOperations>();
            services.AddScoped<IMasterDataCacheOperations, MasterDataCacheOperations>();

            // Business 
            services.AddScoped<IServiceRequestOperations, ServiceRequestOperations>();

            return services;
        }
    }
}
