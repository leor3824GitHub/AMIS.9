using AMIS.Framework.Core.Identity.Users.Events;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Employees.EventHandlers;

/// <summary>
/// Automatically creates an employee record when a user logs in successfully
/// if one doesn't already exist.
/// </summary>
public class AutoCreateEmployeeOnLoginHandler(
    ILogger<AutoCreateEmployeeOnLoginHandler> logger,
    [FromKeyedServices("catalog:employees")] IRepository<Employee> repository)
    : INotificationHandler<UserLoggedInEvent>
{
    public async Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking employee record for user {UserId}", notification.UserId);

            // Check if employee record already exists
            var spec = new EmployeeByUserIdSpec(notification.UserId);
            var existingEmployee = await repository.FirstOrDefaultAsync(spec, cancellationToken);

            if (existingEmployee is not null)
            {
                logger.LogDebug("Employee record already exists for user {UserId}", notification.UserId);
                return;
            }

            // Auto-create employee record with default values
            var employee = Employee.Create(
                name: notification.UserName ?? "Unknown",
                designation: "Staff", // Default designation
                responsibilityCode: "GENERAL", // Default responsibility code
                userId: notification.UserId);

            await repository.AddAsync(employee, cancellationToken);

            logger.LogInformation(
                "Auto-created employee record {EmployeeId} for user {UserId}",
                employee.Id,
                notification.UserId);
        }
        catch (Exception ex)
        {
            // Don't fail login if employee creation fails, just log it
            logger.LogError(ex,
                "Failed to auto-create employee record for user {UserId}. User can manually register employee information.",
                notification.UserId);
        }
    }
}
