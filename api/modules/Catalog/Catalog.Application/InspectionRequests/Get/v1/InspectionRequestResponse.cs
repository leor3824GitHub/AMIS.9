using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;

public sealed record InspectionRequestResponse(
    Guid? Id,
    Guid? PurchaseId,
    Guid RequestedById,
    Guid? AssignedInspectorId,
    InspectionRequestStatus Status,
    DateTime DateCreated,
    PurchaseResponse Purchases
);

