using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    decimal SKU,
    string Location,
    string Unit,
    string? Description = null,
    Guid? CategoryId = null) : IRequest<UpdateProductResponse>;
