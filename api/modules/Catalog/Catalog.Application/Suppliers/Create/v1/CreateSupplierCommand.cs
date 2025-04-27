using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Create.v1;
public sealed record CreateSupplierCommand(
    string? Name,    
    string? Address,
    string? Tin,
    string? ContactNo,
    string? Emailadd, 
    string TaxClassification) : IRequest<CreateSupplierResponse>;

