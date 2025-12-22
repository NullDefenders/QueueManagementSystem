using Infrastructure.EntityFrameWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PreRegistrationService.Repositories;
using PreRegistrationService.Services;
using PreRegistrationService.Settings;
using System.Data.Common;

namespace PreRegistrationService
{
    public static class Registrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //var applicationSettings = configuration.Get<ApplicationSettings>();
            services.InstallServices()
                    .InstallRepositories();
            return services;
        }

        private static IServiceCollection InstallServices(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddSingleton<MongoDbService>();
            servicesCollection.AddSingleton<UniqueStringGenerator>();
            return servicesCollection;
        }

        private static IServiceCollection InstallRepositories(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddScoped<RecordRepository>(provider =>
            {
                var antherDependency = provider.GetRequiredService<MongoDbService>();
                return new RecordRepository(antherDependency);
            });
            return servicesCollection;
        }
    }
}
