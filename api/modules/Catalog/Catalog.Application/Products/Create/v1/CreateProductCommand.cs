using System.ComponentModel;
using AMIS.Framework.Core.Storage.File.Features;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Create.v1;
public sealed record CreateProductCommand(
    [property: DefaultValue("Sample Product")] string Name,
    Guid CategoryId,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue(10)] decimal SKU =10,
    [property: DefaultValue("L1B1")] string Location = "L1B1",
    [property: DefaultValue("pc")] string Unit = "pc",
    FileUploadCommand? Image = null) : IRequest<CreateProductResponse>;
