﻿using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Identity.Users.Features.RegisterUser;
using AMIS.Framework.Infrastructure.Auth.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Framework.Infrastructure.Identity.Users.Endpoints;
public static class RegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/register", (RegisterUserCommand request,
            IUserService service,
            HttpContext context,
            CancellationToken cancellationToken) =>
        {
            var origin = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
            return service.RegisterAsync(request, origin, cancellationToken);
        })
        .WithName(nameof(RegisterUserEndpoint))
        .WithSummary("register user")
        .RequirePermission("Permissions.Users.Create")
        .WithDescription("register user");
    }
}
