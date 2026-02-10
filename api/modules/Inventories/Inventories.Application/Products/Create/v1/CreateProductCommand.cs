using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Products.Create.v1;
public sealed record CreateProductCommand(
    [property: DefaultValue("Sample Product")] string? Name,
    string? Description = null,
    string UnitOfMeasure = "piece",
    int EstimatedUsefulLife = 12,
    Guid? CategoryId = null) : IRequest<CreateProductResponse>;

