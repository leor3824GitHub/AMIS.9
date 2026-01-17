using MediatR;

namespace AMIS.WebApi.Inventories.Application.Categories.Delete.v1;
public sealed record DeleteCategoryCommand(
    Guid Id) : IRequest;

