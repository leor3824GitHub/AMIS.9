using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Delete.v1;
public sealed record DeleteSupplierCommand(
    Guid Id) : IRequest;
