using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Get.v1;

public sealed record ProcurementScheduleResponse(
    Guid Id,
    DateOnly? AdsPosting,
    DateOnly? PreBidConference,
    DateOnly? BidOpening,
    DateOnly? BidEvaluation,
    DateOnly? PostQualification,
    DateOnly? NoticeOfAward,
    DateOnly? ContractSigning,
    DateOnly? NoticeToProceeed,
    DateOnly? DeliveryCompletion);

public sealed record ProjectBudgetResponse(
    Guid Id,
    decimal TotalAmount,
    decimal MooeAmount,
    decimal CoAmount);

public sealed record GetProcurementProjectResponse(
    Guid Id,
    string? PapCode,
    string ProjectTitle,
    string PmoEndUser,
    bool IsEpa,
    ModeOfProcurement Mode,
    string FundSource,
    string? Remarks,
    Guid? SourcePlanItemId,
    ProcurementScheduleResponse Schedule,
    ProjectBudgetResponse Budget);
