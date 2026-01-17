using MediatR;

namespace AMIS.WebApi.Inventories.Application.Categories.Update.v1;
public sealed record UpdateCategoryCommand(
    Guid Id,
    string? Name,
    string? Description = null) : IRequest<UpdateCategoryResponse>;

