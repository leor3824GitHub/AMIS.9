using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Reverse.v1;

/// <summary>
/// Handler for reversing a depreciation schedule entry
/// </summary>
public sealed class ReverseDepreciationScheduleHandler(
    ILogger<ReverseDepreciationScheduleHandler> logger,
    [FromKeyedServices("catalog:depreciationschedules")] IRepository<DepreciationSchedule> repository)
    : IRequestHandler<ReverseDepreciationScheduleCommand, ReverseDepreciationScheduleResponse>
{
    public async Task<ReverseDepreciationScheduleResponse> Handle(ReverseDepreciationScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Depreciation schedule {request.Id} not found");

        schedule.Reverse(request.Reason ?? "");
        await repository.UpdateAsync(schedule, cancellationToken);

        logger.LogInformation("Depreciation schedule {DepreciationScheduleId} reversed. Reason: {Reason}", schedule.Id, request.Reason);
        return new ReverseDepreciationScheduleResponse
        {
            Id = schedule.Id,
            Status = schedule.Status.ToString(),
            Reason = request.Reason,
            Message = "Depreciation schedule reversed"
        };
    }
}
