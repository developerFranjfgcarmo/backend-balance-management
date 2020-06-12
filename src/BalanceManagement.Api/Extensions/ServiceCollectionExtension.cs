using BalanceManagement.Data.Context;
using BalanceManagement.Service.IService;
using BalanceManagement.Service.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BalanceManagement.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BalanceManagementDbContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("BalanceManagement")));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBalanceManagementDbContext, BalanceManagementDbContext>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
