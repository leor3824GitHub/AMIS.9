﻿using AMIS.Framework.Core.Tenant.Abstractions;
using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.ActivateTenant;
public sealed class ActivateTenantHandler(ITenantService service) : IRequestHandler<ActivateTenantCommand, ActivateTenantResponse>
{
    public async Task<ActivateTenantResponse> Handle(ActivateTenantCommand request, CancellationToken cancellationToken)
    {
        var status = await service.ActivateAsync(request.TenantId, cancellationToken);
        return new ActivateTenantResponse(status);
    }
}
