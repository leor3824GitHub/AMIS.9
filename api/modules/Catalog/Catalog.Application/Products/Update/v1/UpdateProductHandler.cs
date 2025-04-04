using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Storage;
using AMIS.Framework.Core.Storage.File;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Products.Update.v1;
public sealed class UpdateProductHandler(
    ILogger<UpdateProductHandler> logger,
    [FromKeyedServices("catalog:products")] IRepository<Product> repository,
    IStorageService storageService)
    : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
{
    public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = product ?? throw new ProductNotFoundException(request.Id);

        // Remove old image if flag is set
        if (request.DeleteCurrentImage)
        {
            var currentProductImagePath = product.ImagePath;
            if (!string.IsNullOrEmpty(currentProductImagePath))
            {
                string root = Directory.GetCurrentDirectory();
                storageService.Remove(new Uri(Path.Combine(root, currentProductImagePath)));
            }

            product = product.ClearImagePath();
        }

        var productImagePath = request.Image is not null
            ? (await storageService.UploadAsync<Product>(request.Image, FileType.Image, cancellationToken)).ToString()
            : product.ImagePath;

        var updatedProduct = product.Update(request.Name, request.Description, request.SKU, request.Location, request.Unit, productImagePath, request.CategoryId);
        await repository.UpdateAsync(updatedProduct, cancellationToken);
        logger.LogInformation("product with id : {ProductId} updated.", product.Id);
        return new UpdateProductResponse(product.Id);
    }
}
