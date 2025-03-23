using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Categories.Create.v1;
public sealed class CreateCategoryHandler(
    ILogger<CreateCategoryHandler> logger,
    [FromKeyedServices("catalog:categories")] IRepository<Category> repository)
    : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var category = Category.Create(request.Name!, request.Description);
        await repository.AddAsync(category, cancellationToken);
        logger.LogInformation("category created {CategoryId}", category.Id);
        return new CreateCategoryResponse(category.Id);
    }
}
