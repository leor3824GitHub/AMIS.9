using MediatR;

namespace AMIS.WebApi.Inventories.Application.Suppliers.Delete.v1;
public sealed record DeleteSupplierCommand(
    Guid Id) : IRequest;

