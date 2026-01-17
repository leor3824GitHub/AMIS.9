using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents the procurement schedule milestones for a project.
/// Owned by ProcurementProject aggregate.
/// </summary>
public sealed class ProcurementSchedule : BaseEntity
{
    public Guid ProjectId { get; private set; }

    public DateOnly? AdsPosting { get; private set; }
    public DateOnly? PreBidConference { get; private set; }
    public DateOnly? BidOpening { get; private set; }
    public DateOnly? BidEvaluation { get; private set; }
    public DateOnly? PostQualification { get; private set; }
    public DateOnly? NoticeOfAward { get; private set; }
    public DateOnly? ContractSigning { get; private set; }
    public DateOnly? NoticeToProceeed { get; private set; }
    public DateOnly? DeliveryCompletion { get; private set; }

    private ProcurementSchedule() { }

    internal ProcurementSchedule(Guid projectId)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
    }

    internal void Update(
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
        // Validate chronological order where applicable
        ValidateScheduleSequence(adsPosting, bidOpening, noticeOfAward, contractSigning, deliveryCompletion);

        AdsPosting = adsPosting;
        PreBidConference = preBidConference;
        BidOpening = bidOpening;
        BidEvaluation = bidEvaluation;
        PostQualification = postQualification;
        NoticeOfAward = noticeOfAward;
        ContractSigning = contractSigning;
        NoticeToProceeed = noticeToProceeed;
        DeliveryCompletion = deliveryCompletion;
    }

    private static void ValidateScheduleSequence(
        DateOnly? adsPosting,
        DateOnly? bidOpening,
        DateOnly? noticeOfAward,
        DateOnly? contractSigning,
        DateOnly? deliveryCompletion)
    {
        if (adsPosting.HasValue && bidOpening.HasValue && bidOpening < adsPosting)
            throw new ArgumentException("Bid opening cannot be before ads posting.");

        if (bidOpening.HasValue && noticeOfAward.HasValue && noticeOfAward < bidOpening)
            throw new ArgumentException("Notice of award cannot be before bid opening.");

        if (noticeOfAward.HasValue && contractSigning.HasValue && contractSigning < noticeOfAward)
            throw new ArgumentException("Contract signing cannot be before notice of award.");

        if (contractSigning.HasValue && deliveryCompletion.HasValue && deliveryCompletion < contractSigning)
            throw new ArgumentException("Delivery completion cannot be before contract signing.");
    }
}

