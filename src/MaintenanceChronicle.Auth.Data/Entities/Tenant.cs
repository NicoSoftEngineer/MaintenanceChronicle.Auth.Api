using System.ComponentModel.DataAnnotations;
using MaintenanceChronicle.Auth.Shared.Constants;

namespace MaintenanceChronicle.Auth.Data.Entities;

public class Tenant
{
    public Guid Id { get; set; }

    [MaxLength(StringLengthConstants.MaxNameLength)]
    public required string Name { get; set; } 
}