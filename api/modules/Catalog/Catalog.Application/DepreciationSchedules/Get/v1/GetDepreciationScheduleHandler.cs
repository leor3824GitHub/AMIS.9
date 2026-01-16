using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Get.v1;

public sealed class GetDepreciationScheduleHandler(
    ILogger<GetDepreciationScheduleHandler> logger,
    [FromKeyedServices("catalog:depreciationschedules")] IReadRepository<DepreciationSchedule> repository)
    : IRequestHandler<GetDepreciationScheduleCommand, GetDepreciationScheduleResponse>
{
    public async Task<GetDepreciationScheduleResponse> Handle(GetDepreciationScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Depreciation schedule {request.Id} not found");

        logger.LogInformation("Retrieved depreciation schedule {DepreciationScheduleId}", schedule.Id);
        return new GetDepreciationScheduleResponse(
            schedule.Id,
            schedule.PhysicalAssetId,
            schedule.Year,
            schedule.Month,
            schedule.MonthlyDepreciationAmount,
            schedule.AccumulatedDepreciationAmount,
            schedule.Status.ToString(),
            schedule.JournalEntryVoucherId);
    }
}
