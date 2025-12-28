using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.ProcurementProjects.Get.v1;
using AMIS.WebApi.Catalog.Domain.ProcurementProjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Update.v1;

public sealed class UpdateProcurementProjectHandler(
    ILogger<UpdateProcurementProjectHandler> logger,
    [FromKeyedServices("catalog:procurementProjects")] IRepository<ProcurementProject> repository)
    : IRequestHandler<UpdateProcurementProjectCommand, UpdateProcurementProjectResponse>
{
    public async Task<UpdateProcurementProjectResponse> Handle(UpdateProcurementProjectCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var project = await repository.FirstOrDefaultAsync(new GetProcurementProjectSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (project is null)
        {
            throw new NotFoundException($"Procurement Project with Id '{request.Id}' was not found.");
        }

        project.Update(
            request.PapCode,
            request.ProjectTitle,
            request.PmoEndUser,
            request.IsEpa,
            request.Mode,
            request.FundSource,
            request.Remarks);

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

        project.UpdateBudget(request.TotalAmount, request.MooeAmount, request.CoAmount);

        await repository.UpdateAsync(project, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Procurement project updated {ProcurementProjectId}", project.Id);
        return new UpdateProcurementProjectResponse(project.Id);
    }
}
