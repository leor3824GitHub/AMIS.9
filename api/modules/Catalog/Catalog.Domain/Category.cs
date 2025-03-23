using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    private Category() { }

    private Category(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
        QueueDomainEvent(new CategoryCreated { Category = this });
    }

    public static Category Create(string name, string? description)
    {
        return new Category(Guid.NewGuid(), name, description);
    }

    public Category Update(string? name, string? description)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CategoryUpdated { Category = this });
        }

        return this;
    }
}


