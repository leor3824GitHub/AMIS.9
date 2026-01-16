using MediatR;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Post.v1;

public sealed record PostJournalEntryVoucherCommand(Guid Id) : IRequest<PostJournalEntryVoucherResponse>;

public sealed record PostJournalEntryVoucherResponse(Guid Id);
