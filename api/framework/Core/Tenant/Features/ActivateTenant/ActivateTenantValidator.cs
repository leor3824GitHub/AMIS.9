﻿using FluentValidation;

namespace AMIS.Framework.Core.Tenant.Features.ActivateTenant;
public sealed class ActivateTenantValidator : AbstractValidator<ActivateTenantCommand>
{
    public ActivateTenantValidator() =>
       RuleFor(t => t.TenantId)
           .NotEmpty();
}
