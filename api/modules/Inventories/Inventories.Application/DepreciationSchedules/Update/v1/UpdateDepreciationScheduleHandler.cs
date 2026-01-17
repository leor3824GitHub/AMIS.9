using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Update.v1;

public sealed class UpdateDepreciationScheduleHandler(
    [FromKeyedServices("inventories:depreciationschedules")] IRepository<DepreciationSchedule> repository)
    : IRequestHandler<UpdateDepreciationScheduleCommand, UpdateDepreciationScheduleResponse>
{
    public async Task<UpdateDepreciationScheduleResponse> Handle(
        UpdateDepreciationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var depreciationSchedule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Depreciation schedule {request.Id} not found");

        // Only update remarks
        if (!string.IsNullOrWhiteSpace(request.Remarks))
        {
            depreciationSchedule.GetType().GetProperty(nameof(DepreciationSchedule.Remarks))?
                .SetValue(depreciationSchedule, request.Remarks);
        }

        await repository.UpdateAsync(depreciationSchedule, cancellationToken);

        return new UpdateDepreciationScheduleResponse(
            depreciationSchedule.Id,
            depreciationSchedule.Remarks);
    }
}

