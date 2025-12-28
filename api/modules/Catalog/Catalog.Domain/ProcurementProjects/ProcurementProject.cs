using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a Procurement Project aggregate root.
/// Contains owned entities: ProcurementSchedule and ProjectBudget.
/// </summary>
public sealed class ProcurementProject : AuditableEntity, IAggregateRoot
{
    public string? PapCode { get; private set; }
    public string ProjectTitle { get; private set; } = string.Empty;
    public string PmoEndUser { get; private set; } = string.Empty;
    public bool IsEpa { get; private set; }
    public ModeOfProcurement Mode { get; private set; }
    public string FundSource { get; private set; } = string.Empty;
    public string? Remarks { get; private set; }

    /// <summary>
    /// Reference to a ProcurementPlanItem if this project was derived from PPMP
    /// </summary>
    public Guid? SourcePlanItemId { get; private set; }

    public ProcurementSchedule Schedule { get; private set; } = null!;
    public ProjectBudget Budget { get; private set; } = null!;

    private ProcurementProject() { }

    private ProcurementProject(
        Guid id,
        string? papCode,
        string projectTitle,
        string pmoEndUser,
        bool isEpa,
        ModeOfProcurement mode,
        string fundSource,
        string? remarks,
        Guid? sourcePlanItemId)
    {
        Id = id;
        SetProjectFields(papCode, projectTitle, pmoEndUser, isEpa, mode, fundSource, remarks, sourcePlanItemId);
        Schedule = new ProcurementSchedule(Id);
        Budget = new ProjectBudget(Id);
    }

    public static ProcurementProject Create(
        string? papCode,
        string projectTitle,
        string pmoEndUser,
        bool isEpa,
        ModeOfProcurement mode,
        string fundSource,
        string? remarks,
        Guid? sourcePlanItemId = null)
        => new(Guid.NewGuid(), papCode, projectTitle, pmoEndUser, isEpa, mode, fundSource, remarks, sourcePlanItemId);

    public ProcurementProject Update(
        string? papCode,
        string projectTitle,
        string pmoEndUser,
        bool isEpa,
        ModeOfProcurement mode,
        string fundSource,
        string? remarks)
    {
        SetProjectFields(papCode, projectTitle, pmoEndUser, isEpa, mode, fundSource, remarks, SourcePlanItemId);
        return this;
    }

    public void UpdateSchedule(
        DateOnly? adsPosting,
        DateOnly? preBidConference,
        DateOnly? bidOpening,
        DateOnly? bidEvaluation,
        DateOnly? postQualification,
        DateOnly? noticeOfAward,
        DateOnly? contractSigning,
        DateOnly? noticeToProceeed,
        DateOnly? deliveryCompletion)
    {
        Schedule.Update(adsPosting, preBidConference, bidOpening, bidEvaluation, postQualification,
            noticeOfAward, contractSigning, noticeToProceeed, deliveryCompletion);
    }

    public void UpdateBudget(decimal totalAmount, decimal mooeAmount, decimal coAmount)
    {
        Budget.Update(totalAmount, mooeAmount, coAmount);
    }

    private void SetProjectFields(
        string? papCode,
        string projectTitle,
        string pmoEndUser,
        bool isEpa,
        ModeOfProcurement mode,
        string fundSource,
        string? remarks,
        Guid? sourcePlanItemId)
    {
        if (string.IsNullOrWhiteSpace(projectTitle))
            throw new ArgumentException("Project title is required.", nameof(projectTitle));
        if (string.IsNullOrWhiteSpace(pmoEndUser))
            throw new ArgumentException("PMO/End-user is required.", nameof(pmoEndUser));
        if (string.IsNullOrWhiteSpace(fundSource))
            throw new ArgumentException("Fund source is required.", nameof(fundSource));

        PapCode = string.IsNullOrWhiteSpace(papCode) ? null : papCode;
        ProjectTitle = projectTitle;
        PmoEndUser = pmoEndUser;
        IsEpa = isEpa;
        Mode = mode;
        FundSource = fundSource;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks;
        SourcePlanItemId = sourcePlanItemId;
    }
}
