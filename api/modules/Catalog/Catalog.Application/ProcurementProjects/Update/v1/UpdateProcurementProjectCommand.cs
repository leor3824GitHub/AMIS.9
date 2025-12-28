using AMIS.WebApi.Catalog.Domain.ProcurementProjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Update.v1;

public sealed class UpdateProcurementProjectCommand : IRequest<UpdateProcurementProjectResponse>
{
    public Guid Id { get; set; }
    public string? PapCode { get; set; }
    public string ProjectTitle { get; set; } = string.Empty;
    public string PmoEndUser { get; set; } = string.Empty;
    public bool IsEpa { get; set; }
    public ModeOfProcurement Mode { get; set; }
    public string FundSource { get; set; } = string.Empty;
    public string? Remarks { get; set; }

    // Schedule fields
    public DateOnly? AdsPosting { get; set; }
    public DateOnly? PreBidConference { get; set; }
    public DateOnly? BidOpening { get; set; }
    public DateOnly? BidEvaluation { get; set; }
    public DateOnly? PostQualification { get; set; }
    public DateOnly? NoticeOfAward { get; set; }
    public DateOnly? ContractSigning { get; set; }
    public DateOnly? NoticeToProceeed { get; set; }
    public DateOnly? DeliveryCompletion { get; set; }

    // Budget fields
    public decimal TotalAmount { get; set; }
    public decimal MooeAmount { get; set; }
    public decimal CoAmount { get; set; }
}
