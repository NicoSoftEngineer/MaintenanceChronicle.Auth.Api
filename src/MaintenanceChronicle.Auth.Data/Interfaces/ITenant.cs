using MaintenanceChronicle.Auth.Data.Entities;

namespace MaintenanceChronicle.Auth.Data.Interfaces;

public interface ITenant
{
    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}

