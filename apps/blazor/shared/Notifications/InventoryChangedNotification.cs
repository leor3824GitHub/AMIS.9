using System;

namespace AMIS.Blazor.Shared.Notifications;

// Broadcast when inventory may have changed so listeners can refresh.
public class InventoryChangedNotification : INotificationMessage
{
    // Optional: scope refresh to a specific product if known
    public Guid? ProductId { get; set; }
}
