using MaintenanceChronicle.Auth.Infrastructure.ServiceInstaller;
using OpenIddict.Abstractions;
using System.Security.Cryptography.X509Certificates;
using MaintenanceChronicle.Auth.Data;

namespace MaintenanceChronicle.Auth.Api.Installers;

public class OpenIddictInstaller : IServiceInstaller
{
    public int Order => 2;
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenIddict()
    // Register the OpenIddict core components
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<AppDbContext>()
               .ReplaceDefaultEntities<Guid>(); // Use Guid as primary key type

    })

    // Register the OpenIddict server components
    .AddServer(options =>
    {

        // ===== Flows Configuration =====
        options.AllowAuthorizationCodeFlow()
               .RequireProofKeyForCodeExchange() // Enforce PKCE
               .AllowRefreshTokenFlow(); // Only if needed for legacy apps

        // ===== Security Configuration =====
        if ()
        {
            // Development signing keys (auto-generated)
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();
        }
        else
        {
            // Production certificates
            // Option 1: Load from file
            var signingCert = new X509Certificate2(
                Path.Combine(builder.Environment.ContentRootPath, "Certificates", "signing.pfx"),
                builder.Configuration["Certificates:SigningPassword"]);

            var encryptionCert = new X509Certificate2(
                Path.Combine(builder.Environment.ContentRootPath, "Certificates", "encryption.pfx"),
                builder.Configuration["Certificates:EncryptionPassword"]);

            options.AddSigningCertificate(signingCert)
                   .AddEncryptionCertificate(encryptionCert);

            // Option 2: Load from certificate store (Windows/Linux)
            // options.AddSigningCertificate(thumbprint: builder.Configuration["Certificates:SigningThumbprint"]);
            // options.AddEncryptionCertificate(thumbprint: builder.Configuration["Certificates:EncryptionThumbprint"]);
        }

        // ===== Token Configuration =====
        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15))
               .SetIdentityTokenLifetime(TimeSpan.FromMinutes(15))
               .SetRefreshTokenLifetime(TimeSpan.FromDays(7))
               .SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(5))
               .SetDeviceCodeLifetime(TimeSpan.FromMinutes(5))
               .SetUserCodeLifetime(TimeSpan.FromMinutes(5));

        // Enable refresh token rotation for better security
        options.SetRefreshTokenReuseLeeway(TimeSpan.Zero);

        // Register scopes
        options.RegisterScopes(
            OpenIddictConstants.Scopes.OpenId,
            OpenIddictConstants.Scopes.Email,
            OpenIddictConstants.Scopes.Profile,
            OpenIddictConstants.Scopes.Phone,
            OpenIddictConstants.Scopes.Address,
            OpenIddictConstants.Scopes.OfflineAccess,
            OpenIddictConstants.Scopes.Roles
        );

        // Register claim types that will be included in tokens
        options.RegisterClaims(
            OpenIddictConstants.Claims.Subject,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.GivenName,
            OpenIddictConstants.Claims.FamilyName,
            OpenIddictConstants.Claims.MiddleName,
            OpenIddictConstants.Claims.Nickname,
            OpenIddictConstants.Claims.PreferredUsername,
            OpenIddictConstants.Claims.Profile,
            OpenIddictConstants.Claims.Picture,
            OpenIddictConstants.Claims.Website,
            OpenIddictConstants.Claims.Email,
            OpenIddictConstants.Claims.EmailVerified,
            OpenIddictConstants.Claims.Gender,
            OpenIddictConstants.Claims.Birthdate,
            OpenIddictConstants.Claims.Zoneinfo,
            OpenIddictConstants.Claims.Locale,
            OpenIddictConstants.Claims.PhoneNumber,
            OpenIddictConstants.Claims.PhoneNumberVerified,
            OpenIddictConstants.Claims.Address,
            OpenIddictConstants.Claims.UpdatedAt,
            OpenIddictConstants.Claims.Role
        );

        // ASP.NET Core integration
        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough()
               .EnableLogoutEndpointPassthrough()
               .EnableVerificationEndpointPassthrough()
               .EnableStatusCodePagesIntegration(); // Return proper status codes
    })

    // Register the OpenIddict validation components (for API protection)
    .AddValidation(options =>
    {
        options.UseLocalServer(); // Use the same server as the issuer
        options.UseAspNetCore();  // ASP.NET Core integration
    });
    }
}