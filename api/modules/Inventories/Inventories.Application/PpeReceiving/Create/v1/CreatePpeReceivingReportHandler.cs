using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;

public sealed class CreatePpeReceivingReportHandler(
    ILogger<CreatePpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PPERR> receivingRepository)
    : IRequestHandler<CreatePpeReceivingReportCommand, CreatePpeReceivingReportResponse>
{
    public async Task<CreatePpeReceivingReportResponse> Handle(CreatePpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = new PPERR(
                request.RRNumber,
                request.ReceivedFrom,
                request.Address,
                PpeReceiptType.FromString(request.Type),
                request.Date,
                request.Notes);

            var lineItems = request.LineItems.Select(x => new PPERRLineItem(
                x.PropertyCode,
                request.RRNumber)).ToList();

            report.AddItems(lineItems);

            await receivingRepository.AddAsync(report, cancellationToken);
            await receivingRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Receiving Report {RRNumber} created as Draft.", request.RRNumber);
            return new CreatePpeReceivingReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating PPE Receiving Report.");
            throw;
        }
    }
}


