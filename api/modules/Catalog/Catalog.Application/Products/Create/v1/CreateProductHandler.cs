using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Storage;
using AMIS.Framework.Core.Storage.File;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Products.Create.v1;
public sealed class CreateProductHandler(
    ILogger<CreateProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository,
    IStorageService storageService)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var productImagePath = await storageService.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken);
        var product = Product.Create(request.Name!, request.Description, request.SKU, request.Location, request.Unit, productImagePath.ToString(), request.CategoryId);
        await repository.AddAsync(product, cancellationToken);
        logger.LogInformation("product created {ProductId}", product.Id);
        return new CreateProductResponse(product.Id);
    }
}
