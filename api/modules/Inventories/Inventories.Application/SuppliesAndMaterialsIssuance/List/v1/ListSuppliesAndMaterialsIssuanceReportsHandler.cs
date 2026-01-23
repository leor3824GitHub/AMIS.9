using Ardalis.Specification;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.List.v1;

public sealed class ListSuppliesAndMaterialsIssuanceReportsHandler : IRequestHandler<ListSuppliesAndMaterialsIssuanceReportsCommand, ListSuppliesAndMaterialsIssuanceReportsResponse>
{
    private readonly IReadRepository<SuppliesAndMaterialsIssuanceReport> _repository;

    public ListSuppliesAndMaterialsIssuanceReportsHandler(
        [FromKeyedServices("inventories:smir")] IReadRepository<SuppliesAndMaterialsIssuanceReport> repository)
    {
        _repository = repository;
    }

    public async Task<ListSuppliesAndMaterialsIssuanceReportsResponse> Handle(
        ListSuppliesAndMaterialsIssuanceReportsCommand request,
        CancellationToken cancellationToken)
    {
        var spec = new AllSmirSpec();
        var reports = await _repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        var dtos = reports.Select(r => new SuppliesAndMaterialsIssuanceReportDto(
            r.Id,
            r.SmirNumber,
            r.Recipient.Name,
            r.IssuanceReason.Value,
            r.TransactionDate,
            r.LineItems.Count,
            r.LineItems.Sum(li => li.Quantity * li.UnitCost),
            0, // Status - placeholder
            r.Created
        )).ToList();

        return new ListSuppliesAndMaterialsIssuanceReportsResponse(dtos);
    }
}

internal class AllSmirSpec : Specification<SuppliesAndMaterialsIssuanceReport>
{
    public AllSmirSpec()
    {
        Query.OrderByDescending(x => x.Created);
    }
}
