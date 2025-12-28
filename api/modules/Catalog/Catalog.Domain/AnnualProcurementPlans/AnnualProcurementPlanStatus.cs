namespace AMIS.WebApi.Catalog.Domain;

public enum AnnualProcurementPlanStatus
{
    None = 0,
    Draft = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5
}
