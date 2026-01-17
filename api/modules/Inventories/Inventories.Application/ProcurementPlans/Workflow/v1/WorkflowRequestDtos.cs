namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Workflow.v1;

/// <summary>
/// Request body for approving a procurement plan
/// </summary>
public sealed record ApproveProcurementPlanBody(Guid ApprovedByUserId);

/// <summary>
/// Request body for rejecting a procurement plan
/// </summary>
public sealed record RejectProcurementPlanBody(Guid RejectedByUserId, string Reason);

