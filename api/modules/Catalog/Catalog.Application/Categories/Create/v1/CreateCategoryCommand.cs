using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Categories.Create.v1;
public sealed record CreateCategoryCommand(
    [property: DefaultValue("Sample Category")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null) : IRequest<CreateCategoryResponse>;

