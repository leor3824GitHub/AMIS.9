using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Delete.v1;

public sealed class DeletePpeIssuanceReportHandler(
    [FromKeyedServices("inventories:ppeir")] IRepository<Domain.PPEIR> repository,
    ILogger<DeletePpeIssuanceReportHandler> logger) : IRequestHandler<DeletePpeIssuanceReportCommand, DeletePpeIssuanceReportResponse>
{
    public async Task<DeletePpeIssuanceReportResponse> Handle(DeletePpeIssuanceReportCommand request, CancellationToken cancellationToken)
    {
        var report = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (report is null)
        {
            logger.LogWarning("PPE Issuance Report {Id} not found for deletion", request.Id);
            throw new InvalidOperationException($"PPE Issuance Report {request.Id} not found.");
        }

        // Only allow deletion of Draft reports
        if (report.Status != Domain.ValueObjects.PpeReportStatus.Draft)
        {
            logger.LogWarning("Cannot delete PPEIR {Id} with status {Status}. Only Draft reports can be deleted.", request.Id, report.Status);
            throw new InvalidOperationException($"Only Draft reports can be deleted. Current status: {report.Status}");
        }

        // Delete the report
        await repository.DeleteAsync(report, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("PPEIR {Id} deleted successfully", request.Id);

        return new DeletePpeIssuanceReportResponse(
            Id: report.Id,
            Message: "PPE Issuance Report deleted successfully");
    }
}
