using MediatR;

namespace AMIS.WebApi.Inventories.Application.Suppliers.Update.v1;
public sealed record UpdateSupplierCommand(
    Guid Id,
    string? Name,
    string TaxClassification,
    string? Address = null,
    string? Tin = null,
    string? ContactNo = null,
    string? Emailadd = null) : IRequest<UpdateSupplierResponse>;

