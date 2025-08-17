using MaintenanceChronicle.Auth.Data.Interfaces;
using MaintenanceChronicle.Auth.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MaintenanceChronicle.Auth.Data.Entities;

public class AppUser : IdentityUser<Guid>, ITenant
{
    [MaxLength(StringLengthConstants.MaxFirstNameLength)]
    public string? FirstName { get; set; }

    [MaxLength(StringLengthConstants.MaxLastNameLength)]
    public string? LastName { get; set; }

    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
}