using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Suppliers.Delete.v1;
public sealed class DeleteSupplierHandler(
    ILogger<DeleteSupplierHandler> logger,
    [FromKeyedServices("inventories:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<DeleteSupplierCommand>
{
    public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = supplier ?? throw new SupplierNotFoundException(request.Id);
        await repository.DeleteAsync(supplier, cancellationToken);
        logger.LogInformation("Supplier with id : {SupplierId} deleted", supplier.Id);
    }
}

