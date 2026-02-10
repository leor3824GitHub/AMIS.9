using MediatR;

namespace AMIS.WebApi.Inventories.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    string? Description = null,
    string? UnitOfMeasure = null,
    int? EstimatedUsefulLife = null,
    Guid? CategoryId = null) : IRequest<UpdateProductResponse>;

