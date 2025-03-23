using MediatR;

namespace AMIS.WebApi.Catalog.Application.Categories.Delete.v1;
public sealed record DeleteCategoryCommand(
    Guid Id) : IRequest;
