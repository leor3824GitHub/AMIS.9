using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Middleware;

public class EmployeeRegistrationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<EmployeeRegistrationMiddleware> _logger;

    // Paths that should be excluded from registration check
    private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/api/v1/catalog/employees/check-registration",
        "/api/v1/catalog/employees/self-register",
        "/api/tokens",
        "/api/identity",
        "/api/users",
        "/api/personal",
        "/swagger",
        "/health",
        "/_framework",
        "/_content"
    };

    public EmployeeRegistrationMiddleware(
        RequestDelegate next,
        ILogger<EmployeeRegistrationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip middleware if not authenticated
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            await _next(context);
            return;
        }

        // Skip middleware for excluded paths
        var path = context.Request.Path.Value ?? string.Empty;
        if (ExcludedPaths.Any(excluded => path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Get current user service
        var currentUser = context.RequestServices.GetService<ICurrentUser>();
        if (currentUser is null || !currentUser.IsAuthenticated())
        {
            await _next(context);
            return;
        }

        try
        {
            var userId = currentUser.GetUserId();

            // Get employee repository
            var repository = context.RequestServices
                .GetKeyedService<IReadRepository<Employee>>("catalog:employees");

            if (repository is not null)
            {
                var spec = new EmployeeByUserIdSpec(userId);
                var employee = await repository.FirstOrDefaultAsync(spec, context.RequestAborted);

                if (employee is null)
                {
                    _logger.LogWarning(
                        "User {UserId} attempted to access {Path} without employee registration",
                        userId,
                        path);

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "EmployeeRegistrationRequired",
                        message = "You must complete your employee registration before accessing this resource.",
                        // Corrected path includes module base 'catalog'
                        registrationUrl = "/api/v1/catalog/employees/self-register"
                    });
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking employee registration for user");
            // Continue processing - don't block on middleware errors
        }

        await _next(context);
    }
}
