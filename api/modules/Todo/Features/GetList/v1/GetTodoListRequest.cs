using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Todo.Features.GetList.v1;
public record GetTodoListRequest(PaginationFilter Filter) : IRequest<PagedList<TodoDto>>;
