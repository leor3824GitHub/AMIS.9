namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

/// <summary>
/// Request body for approving an annual procurement plan
/// </summary>
public sealed record ApproveAnnualProcurementPlanBody(Guid ApprovedByUserId);

/// <summary>
/// Request body for rejecting an annual procurement plan
/// </summary>
public sealed record RejectAnnualProcurementPlanBody(Guid RejectedByUserId, string Reason);
