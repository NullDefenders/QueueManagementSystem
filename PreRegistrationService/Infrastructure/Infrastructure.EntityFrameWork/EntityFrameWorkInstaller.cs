using System.Threading.Tasks;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EntityFrameWork
{
    public static class EntityFrameWorkInstaller
    {
        public static IServiceCollection ConfigureContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(optionBuilder => optionBuilder.UseLazyLoadingProxies());

            return services;
        }
    }
}
