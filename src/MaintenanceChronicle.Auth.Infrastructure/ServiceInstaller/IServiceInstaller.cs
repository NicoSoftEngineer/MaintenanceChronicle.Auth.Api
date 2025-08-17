using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaintenanceChronicle.Auth.Infrastructure.ServiceInstaller;

public interface IServiceInstaller
{
    int Order { get; }
    void Install(IServiceCollection services, IConfiguration configuration);
}