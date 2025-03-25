using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Create.v1;
public sealed record CreateSupplierCommand(
    [property: DefaultValue("Sample Supplier")] string? Name,
    [property: DefaultValue(true)] bool IsVAT = true,
    [property: DefaultValue("Address")] string? Address = null,
    [property: DefaultValue("TIN")] string? TIN = null,
    [property: DefaultValue("ContactNo")] string? ContactNo = null,
    [property: DefaultValue("Emailadd")] string? Emailadd = null) : IRequest<CreateSupplierResponse>;

