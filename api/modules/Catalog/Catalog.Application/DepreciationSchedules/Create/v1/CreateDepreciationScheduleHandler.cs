using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Create.v1;

public sealed class CreateDepreciationScheduleHandler(
    ILogger<CreateDepreciationScheduleHandler> logger,
    [FromKeyedServices("catalog:depreciationschedules")] IRepository<DepreciationSchedule> repository)
    : IRequestHandler<CreateDepreciationScheduleCommand, CreateDepreciationScheduleResponse>
{
    public async Task<CreateDepreciationScheduleResponse> Handle(CreateDepreciationScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = DepreciationSchedule.Create(
            request.AssetId,
            request.Month,
            request.Year,
            request.MonthlyDepreciationAmount,
            accumulatedDepreciationAmount: 0m); // New schedule starts with 0 accumulated

        await repository.AddAsync(schedule, cancellationToken);

        logger.LogInformation("Depreciation schedule created {DepreciationScheduleId} for asset {AssetId}", schedule.Id, request.AssetId);
        return new CreateDepreciationScheduleResponse(schedule.Id);
    }
}
