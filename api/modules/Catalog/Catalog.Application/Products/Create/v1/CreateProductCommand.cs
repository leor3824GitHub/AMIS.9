using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Create.v1;
public sealed record CreateProductCommand(
    [property: DefaultValue("Sample Product")] string? Name,
    [property: DefaultValue(10)] decimal SKU,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue(null)] Guid? CategoryId = null,
    [property: DefaultValue("L1B1")] string Location = "L1B1",
    [property: DefaultValue("pc")] string Unit = "pc") : IRequest<CreateProductResponse>;
