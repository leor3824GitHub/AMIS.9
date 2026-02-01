using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.CreateTenant;
public sealed record CreateTenantCommand(string Id,
    string Name,
    string? ConnectionString,
    string AdminEmail,
    string? Issuer,
    string? NfaOfficeCode) : IRequest<CreateTenantResponse>;
