using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed class CreatePpeIssuanceReportHandler(
    ILogger<CreatePpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PPEIR> issuanceRepository)
    : IRequestHandler<CreatePpeIssuanceReportCommand, CreatePpeIssuanceReportResponse>
{
    public async Task<CreatePpeIssuanceReportResponse> Handle(
        CreatePpeIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Creating new PPE Issuance Report with IR number: {IRNumber} for: {IssuedTo}",
                request.IRNumber,
                request.IssuedTo);

            var report = new PPEIR(
                request.IRNumber,
                request.IssuedTo,
                request.Address,
                PpeIssueType.FromString(request.Type),
                request.Date,
                request.Notes);

            var lineItems = request.LineItems.Select(x => new PPEIRLineItem(
                x.PropertyCode,
                request.IRNumber)).ToList();

            report.AddItems(lineItems);

            await issuanceRepository.AddAsync(report, cancellationToken);
            await issuanceRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "Successfully created PPE Issuance Report as Draft with ID: {Id} and IR number: {IRNumber}",
                report.Id,
                report.IRNumber);

            return new CreatePpeIssuanceReportResponse(
                report.Id,
                report.IRNumber);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error creating PPE Issuance Report with IR number: {IRNumber}",
                request.IRNumber);
            throw;
        }
    }
}

