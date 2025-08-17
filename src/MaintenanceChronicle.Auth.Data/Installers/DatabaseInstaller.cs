using MaintenanceChronicle.Auth.Infrastructure.ServiceInstaller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaintenanceChronicle.Auth.Data.Installers;

public class DatabaseInstaller : IServiceInstaller
{
    public int Order => 0;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DbConnection") , optionsBuilder =>
            {
                optionsBuilder.UseNodaTime();
            });

            options.UseOpenIddict();
        });
    }
}