using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Post.v1;

public sealed class PostDepreciationScheduleHandler(
    ILogger<PostDepreciationScheduleHandler> logger,
    [FromKeyedServices("inventories:depreciationschedules")] IRepository<DepreciationSchedule> repository)
    : IRequestHandler<PostDepreciationScheduleCommand, PostDepreciationScheduleResponse>
{
    public async Task<PostDepreciationScheduleResponse> Handle(PostDepreciationScheduleCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var schedule = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Depreciation schedule {request.Id} not found");

        schedule.Post(request.JournalEntryVoucherId);
        await repository.UpdateAsync(schedule, cancellationToken);

        logger.LogInformation("Depreciation schedule {DepreciationScheduleId} posted to voucher {VoucherId}", schedule.Id, request.JournalEntryVoucherId);
        return new PostDepreciationScheduleResponse(schedule.Id);
    }
}

