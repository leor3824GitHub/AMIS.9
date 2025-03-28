﻿using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class SupplierNotFoundException : NotFoundException
{
    public SupplierNotFoundException(Guid id)
        : base($"employee with id {id} not found")
    {
    }
}
