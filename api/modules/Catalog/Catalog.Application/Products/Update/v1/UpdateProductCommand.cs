using AMIS.Framework.Core.Storage.File.Features;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Update.v1;
public sealed record UpdateProductCommand(
    Guid Id,
    string? Name,
    decimal SKU,
    UnitOfMeasure Unit,
    string? Description = null,
    FileUploadCommand? Image = null,
    Guid? CategoryId = null,
    bool DeleteCurrentImage = false) : IRequest<UpdateProductResponse>;
