using System;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;

public sealed class InventoryTransactionNotFoundException : Exception
{
    public InventoryTransactionNotFoundException(Guid id)
        : base($"InventoryTransaction with Id '{id}' was not found.") { }
}
