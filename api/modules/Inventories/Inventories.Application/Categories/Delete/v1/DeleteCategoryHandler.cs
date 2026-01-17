using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Categories.Delete.v1;
public sealed class DeleteCategoryHandler(
    ILogger<DeleteCategoryHandler> logger,
    [FromKeyedServices("inventories:categories")] IRepository<Category> repository)
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

