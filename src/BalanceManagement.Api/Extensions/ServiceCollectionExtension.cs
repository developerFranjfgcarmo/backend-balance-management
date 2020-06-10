using BalanceManagement.Data.Context;
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
            //todo:domain
            return services;
        }
    }
}
