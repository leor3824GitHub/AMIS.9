using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Delete.v1;

public sealed record DeleteDepreciationScheduleCommand(Guid Id) : IRequest<DeleteDepreciationScheduleResponse>;

public sealed record DeleteDepreciationScheduleResponse(Guid Id);

public sealed class DeleteDepreciationScheduleHandler(
    ILogger<DeleteDepreciationScheduleHandler> logger,
    [FromKeyedServices("catalog:depreciationschedules")] IRepository<DepreciationSchedule> repository)
    : IRequestHandler<DeleteDepreciationScheduleCommand, DeleteDepreciationScheduleResponse>
{
    public async Task<DeleteDepreciationScheduleResponse> Handle(DeleteDepreciationScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Depreciation schedule {request.Id} not found");

        await repository.DeleteAsync(schedule, cancellationToken);

        logger.LogInformation("Depreciation schedule {DepreciationScheduleId} deleted", schedule.Id);
        return new DeleteDepreciationScheduleResponse(schedule.Id);
    }
}
