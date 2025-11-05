using System.ComponentModel;
using AMIS.Framework.Core.Storage.File.Features;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Create.v1;
public sealed record CreateProductCommand(
    [property: DefaultValue("Sample Product")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue(10)] decimal SKU =10,
    [property: DefaultValue(UnitOfMeasure.Piece)] UnitOfMeasure Unit = UnitOfMeasure.Piece,
    FileUploadCommand? Image = null,
    Guid? CategoryId = null) : IRequest<CreateProductResponse>;
