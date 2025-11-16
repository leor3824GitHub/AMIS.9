using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;

public sealed record PurchaseRequestResponse(
    Guid? Id,
    DateTime RequestDate,
    Guid RequestedBy,
    string Purpose,
    PurchaseRequestStatus Status,
    string? ApprovalRemarks,
    Guid? ApprovedBy,
    DateTime? ApprovedOn,
    ICollection<PurchaseRequestItemResponse>? Items
);
