using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Categories.Delete.v1;
public sealed class DeleteCategoryHandler(
    ILogger<DeleteCategoryHandler> logger,
    [FromKeyedServices("catalog:categories")] IRepository<Category> repository)
    : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = category ?? throw new CategoryNotFoundException(request.Id);
        await repository.DeleteAsync(category, cancellationToken);
        logger.LogInformation("Category with id : {CategoryId} deleted", category.Id);
    }
}
