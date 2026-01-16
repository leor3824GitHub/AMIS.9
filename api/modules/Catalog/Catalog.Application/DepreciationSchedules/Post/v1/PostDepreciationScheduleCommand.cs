using MediatR;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Post.v1;

public sealed record PostDepreciationScheduleCommand(
    Guid Id,
    Guid JournalEntryVoucherId) : IRequest<PostDepreciationScheduleResponse>;

public sealed record PostDepreciationScheduleResponse(Guid Id);
