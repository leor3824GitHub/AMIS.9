using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Categories.Update.v1;
public sealed class UpdateCategoryHandler(
    ILogger<UpdateCategoryHandler> logger,
    [FromKeyedServices("catalog:categories")] IRepository<Category> repository)
    : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
{
    public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var category = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = category ?? throw new CategoryNotFoundException(request.Id);
        var updatedCategory = category.Update(request.Name, request.Description);
        await repository.UpdateAsync(updatedCategory, cancellationToken);
        logger.LogInformation("Category with id : {CategoryId} updated.", category.Id);
        return new UpdateCategoryResponse(category.Id);
    }
}
