using AMIS.Blazor.Shared.Notifications;

namespace AMIS.Blazor.Infrastructure.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}