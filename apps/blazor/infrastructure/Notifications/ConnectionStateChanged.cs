using AMIS.Blazor.Shared.Notifications;

namespace AMIS.Blazor.Infrastructure.Notifications;

public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;