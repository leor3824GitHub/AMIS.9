using Carter;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.WebApi.Todo.Domain;
using AMIS.WebApi.Todo.Features.Create.v1;
using AMIS.WebApi.Todo.Features.Delete.v1;
using AMIS.WebApi.Todo.Features.Get.v1;
using AMIS.WebApi.Todo.Features.GetList.v1;
using AMIS.WebApi.Todo.Features.Update.v1;
using AMIS.WebApi.Todo.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Todo;
public static class TodoModule
{

    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var todoGroup = app.MapGroup("todos").WithTags("todos");
            todoGroup.MapTodoItemCreationEndpoint();
            todoGroup.MapGetTodoEndpoint();
            todoGroup.MapGetTodoListEndpoint();
            todoGroup.MapTodoItemUpdationEndpoint();
            todoGroup.MapTodoItemDeletionEndpoint();
        }
    }
    public static WebApplicationBuilder RegisterTodoServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<TodoDbContext>();
        builder.Services.AddScoped<IDbInitializer, TodoDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        builder.Services.AddKeyedScoped<IReadRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        return builder;
    }
    public static WebApplication UseTodoModule(this WebApplication app)
    {
        return app;
    }
}
