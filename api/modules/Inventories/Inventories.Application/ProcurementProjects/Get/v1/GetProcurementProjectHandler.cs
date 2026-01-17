using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Get.v1;

public sealed class GetProcurementProjectHandler(
    [FromKeyedServices("inventories:procurementProjects")] IReadRepository<ProcurementProject> repository)
    : IRequestHandler<GetProcurementProjectRequest, GetProcurementProjectResponse>
{
    public async Task<GetProcurementProjectResponse> Handle(GetProcurementProjectRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(new GetProcurementProjectSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (project is null)
        {
            throw new NotFoundException($"Procurement Project with Id '{request.Id}' was not found.");
        }

        return new GetProcurementProjectResponse(
            project.Id,
            project.PapCode,
            project.ProjectTitle,
            project.PmoEndUser,
            project.IsEpa,
            project.Mode,
            project.FundSource,
            project.Remarks,
            project.SourcePlanItemId,
            new ProcurementScheduleResponse(
                project.Schedule.Id,
                project.Schedule.AdsPosting,
                project.Schedule.PreBidConference,
                project.Schedule.BidOpening,
                project.Schedule.BidEvaluation,
                project.Schedule.PostQualification,
                project.Schedule.NoticeOfAward,
                project.Schedule.ContractSigning,
                project.Schedule.NoticeToProceeed,
                project.Schedule.DeliveryCompletion),
            new ProjectBudgetResponse(
                project.Budget.Id,
                project.Budget.TotalAmount,
                project.Budget.MooeAmount,
                project.Budget.CoAmount));
    }
}

