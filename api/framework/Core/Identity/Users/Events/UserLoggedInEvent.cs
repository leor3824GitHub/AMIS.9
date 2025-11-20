using MediatR;

namespace AMIS.Framework.Core.Identity.Users.Events;

/// <summary>
/// Event published when a user successfully logs in.
/// </summary>
public sealed record UserLoggedInEvent(
    Guid UserId,
    string? UserName,
    string? Email) : INotification;
