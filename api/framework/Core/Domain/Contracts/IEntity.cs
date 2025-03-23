using System.Collections.ObjectModel;
using AMIS.Framework.Core.Domain.Events;

namespace AMIS.Framework.Core.Domain.Contracts;

public interface IEntity
{
    Collection<DomainEvent> DomainEvents { get; }
}

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}
