using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Todo.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Todo.Features.GetList.v1;

public sealed class GetTodoListHandler(
    [FromKeyedServices("todo")] IReadRepository<TodoItem> repository)
    : IRequestHandler<GetTodoListRequest, PagedList<TodoDto>>
{
    public async Task<PagedList<TodoDto>> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new EntitiesByPaginationFilterSpec<TodoItem, TodoDto>(request.Filter);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<TodoDto>(items, request.Filter.PageNumber, request.Filter.PageSize, totalCount);
    }
}
