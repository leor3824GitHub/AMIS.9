using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Update.v1;
public sealed record UpdateSupplierCommand(
    Guid Id,
    string? Name,
    bool IsVAT = true,
    string? Address = null,
    string? TIN = null,
    string? ContactNo = null,
    string? Emailadd = null) : IRequest<UpdateSupplierResponse>;
