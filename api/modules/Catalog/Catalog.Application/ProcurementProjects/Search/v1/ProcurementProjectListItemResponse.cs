using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Search.v1;

public sealed record ProcurementProjectListItemResponse(
    Guid Id,
    string? PapCode,
    string ProjectTitle,
    string PmoEndUser,
    bool IsEpa,
    ModeOfProcurement Mode,
    string FundSource,
    decimal TotalBudget);
