using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Update.v1;
public sealed class UpdateSupplierHandler(
    ILogger<UpdateSupplierHandler> logger,
    [FromKeyedServices("catalog:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<UpdateSupplierCommand, UpdateSupplierResponse>
{
    public async Task<UpdateSupplierResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var supplier = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = supplier ?? throw new SupplierNotFoundException(request.Id);
        var updatedSupplier = supplier.Update(request.Name!, request.Address, request.Tin, request.TaxClassification, request.ContactNo, request.Emailadd);
        await repository.UpdateAsync(updatedSupplier, cancellationToken);
        logger.LogInformation("Supplier with id : {SupplierId} updated.", supplier.Id);
        return new UpdateSupplierResponse(supplier.Id);
    }
}
