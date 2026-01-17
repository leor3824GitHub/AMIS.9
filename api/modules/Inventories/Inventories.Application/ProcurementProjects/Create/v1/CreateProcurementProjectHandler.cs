using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Create.v1;

public sealed class CreateProcurementProjectHandler(
    ILogger<CreateProcurementProjectHandler> logger,
    [FromKeyedServices("inventories:procurementProjects")] IRepository<ProcurementProject> repository)
    : IRequestHandler<CreateProcurementProjectCommand, CreateProcurementProjectResponse>
{
    public async Task<CreateProcurementProjectResponse> Handle(CreateProcurementProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = ProcurementProject.Create(
            request.PapCode,
            request.ProjectTitle,
            request.PmoEndUser,
            request.IsEpa,
            request.Mode,
            request.FundSource,
            request.Remarks,
            request.SourcePlanItemId);

        // Update schedule if provided
        project.UpdateSchedule(
            request.AdsPosting,
            request.PreBidConference,
            request.BidOpening,
            request.BidEvaluation,
            request.PostQualification,
            request.NoticeOfAward,
            request.ContractSigning,
            request.NoticeToProceeed,
            request.DeliveryCompletion);

        // Update budget if provided
        if (request.TotalAmount > 0 || request.MooeAmount > 0 || request.CoAmount > 0)
        {
            project.UpdateBudget(request.TotalAmount, request.MooeAmount, request.CoAmount);
        }

        await repository.AddAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Procurement project created {ProcurementProjectId}", project.Id);
        return new CreateProcurementProjectResponse(project.Id);
    }
}

