using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Create.v1;
public sealed class CreateSupplierHandler(
    ILogger<CreateSupplierHandler> logger,
    [FromKeyedServices("catalog:suppliers")] IRepository<Supplier> repository)
    : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
{
    public async Task<CreateSupplierResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var supplier = Supplier.Create(request.Name!, request.Address, request.Tin, request.TaxClassification, request.ContactNo, request.Emailadd);
        await repository.AddAsync(supplier, cancellationToken);
        logger.LogInformation("Supplier created {SupplierId}", supplier.Id);
        return new CreateSupplierResponse(supplier.Id);
    }
}
