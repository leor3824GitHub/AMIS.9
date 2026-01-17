using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Categories.Get.v1;
public sealed class GetCategoryHandler(
    [FromKeyedServices("inventories:categories")] IReadRepository<Category> repository,
    ICacheService cache)
    : IRequestHandler<GetCategoryRequest, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"category:{request.Id}",
            async () =>
            {
                var categoryItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (categoryItem == null) throw new CategoryNotFoundException(request.Id);
                return new CategoryResponse(categoryItem.Id, categoryItem.Name, categoryItem.Description);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}

