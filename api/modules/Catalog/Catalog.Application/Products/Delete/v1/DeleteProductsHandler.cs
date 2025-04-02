using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace AMIS.WebApi.Catalog.Application.Products.Delete.v1
{
    public sealed class DeleteProductsHandler(
        ILogger<DeleteProductHandler> logger,
        [FromKeyedServices("catalog:products")] IRepository<Product> repository)
        : IRequestHandler<DeleteProductsCommand>
    {       
        public async Task Handle(DeleteProductsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var products = new List<Product>();

            foreach (var productId in request.ProductIds)
            {
                var product = await repository.GetByIdAsync(productId, cancellationToken);
                if (product != null)
                {
                    products.Add(product);
                }
            }

            if (products.Count == 0)
            {
                logger.LogInformation("No products found for the provided {ProductCount} IDs", products.Count);
                //throw new ProductNotFoundException("No products found for the provided IDs.");
            }

            await repository.DeleteRangeAsync(products, cancellationToken);
            logger.LogInformation("{ProductCount} products deleted", products.Count);
        }
    }
}
