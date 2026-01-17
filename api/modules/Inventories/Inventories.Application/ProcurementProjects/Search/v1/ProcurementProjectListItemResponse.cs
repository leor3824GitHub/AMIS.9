using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Search.v1;

public sealed record ProcurementProjectListItemResponse(
    Guid Id,
    string? PapCode,
    string ProjectTitle,
    string PmoEndUser,
    bool IsEpa,
    ModeOfProcurement Mode,
    string FundSource,
    decimal TotalBudget);

