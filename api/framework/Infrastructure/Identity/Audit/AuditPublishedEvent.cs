using System.Collections.ObjectModel;
using AMIS.Framework.Core.Audit;
using MediatR;

namespace AMIS.Framework.Infrastructure.Identity.Audit;
public class AuditPublishedEvent : INotification
{
    public AuditPublishedEvent(Collection<AuditTrail>? trails)
    {
        Trails = trails;
    }
    public Collection<AuditTrail>? Trails { get; }
}
