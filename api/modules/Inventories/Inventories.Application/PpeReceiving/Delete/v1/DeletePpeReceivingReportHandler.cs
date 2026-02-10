using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Delete.v1;

public sealed class DeletePpeReceivingReportHandler(
    [FromKeyedServices("inventories:pperr")] IRepository<Domain.PPERR> repository,
    ILogger<DeletePpeReceivingReportHandler> logger) : IRequestHandler<DeletePpeReceivingReportCommand, DeletePpeReceivingReportResponse>
{
    public async Task<DeletePpeReceivingReportResponse> Handle(DeletePpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        var report = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (report is null)
        {
            logger.LogWarning("PPE Receiving Report {Id} not found for deletion", request.Id);
            throw new InvalidOperationException($"PPE Receiving Report {request.Id} not found.");
        }

        // Only allow deletion of Draft reports
        if (report.Status != Domain.ValueObjects.PpeReportStatus.Draft)
        {
            logger.LogWarning("Cannot delete PPERR {Id} with status {Status}. Only Draft reports can be deleted.", request.Id, report.Status);
            throw new InvalidOperationException($"Only Draft reports can be deleted. Current status: {report.Status}");
        }

        // Delete the report
        await repository.DeleteAsync(report, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("PPERR {Id} deleted successfully", request.Id);

        return new DeletePpeReceivingReportResponse(
            Id: report.Id,
            Message: "PPE Receiving Report deleted successfully");
    }
}
