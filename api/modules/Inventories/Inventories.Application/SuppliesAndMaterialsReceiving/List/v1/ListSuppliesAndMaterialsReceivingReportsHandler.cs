using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.List.v1;

public sealed class ListSuppliesAndMaterialsReceivingReportsHandler : IRequestHandler<ListSuppliesAndMaterialsReceivingReportsCommand, ListSuppliesAndMaterialsReceivingReportsResponse>
{
    private readonly IReadRepository<SuppliesAndMaterialsReceivingReport> _repository;

    public ListSuppliesAndMaterialsReceivingReportsHandler(
        [FromKeyedServices("inventories:smrr")] IReadRepository<SuppliesAndMaterialsReceivingReport> repository)
    {
        _repository = repository;
    }

    public async Task<ListSuppliesAndMaterialsReceivingReportsResponse> Handle(
        ListSuppliesAndMaterialsReceivingReportsCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new AllSmrrSpec();
        var reports = await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        var dtos = reports.Select(r => new SuppliesAndMaterialsReceivingReportDto(
            r.Id,
            r.SmrrNumber,
            r.Source.Name,
            r.TransactionType.Value,
            r.Source.ReceivingDate,
            r.LineItems.Count,
            r.LineItems.Sum(li => li.Quantity * li.UnitCost),
            0, // Status - placeholder
            r.Created
        )).ToList();

        return new ListSuppliesAndMaterialsReceivingReportsResponse(dtos);
    }
}

internal class AllSmrrSpec : Specification<SuppliesAndMaterialsReceivingReport>
{
    public AllSmrrSpec()
    {
        Query.OrderByDescending(x => x.Created);
    }
}
