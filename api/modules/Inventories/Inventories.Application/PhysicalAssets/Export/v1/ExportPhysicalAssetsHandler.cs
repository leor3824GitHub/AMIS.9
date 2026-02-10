using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Export.v1;

public sealed class ExportPhysicalAssetsHandler(
    [FromKeyedServices("inventories:physicalassets")] IReadRepository<PhysicalAsset> repository)
    : IRequestHandler<ExportPhysicalAssetsCommand, ExportPhysicalAssetsResponse>
{
    public async Task<ExportPhysicalAssetsResponse> Handle(ExportPhysicalAssetsCommand request, CancellationToken cancellationToken)
    {
        var spec = new ExportPhysicalAssetsSpec(request);
        var assets = await repository.ListAsync(spec, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            assets = assets
                .Where(a => !string.IsNullOrWhiteSpace(a.CurrentAssignment?.Location) &&
                            a.CurrentAssignment!.Location!.Contains(request.Location, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var csv = GenerateCsv(assets);
        var fileName = $"PhysicalAssets_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

        return new ExportPhysicalAssetsResponse(fileName, csv, "text/csv");
    }

    private static byte[] GenerateCsv(IEnumerable<PhysicalAsset> assets)
    {
        var sb = new StringBuilder();
        sb.AppendLine("PropertyCode,Classification,AcquisitionCost,AcquisitionDate,Location,Condition,Quantity,BookValue,IsDisposed,CurrentCustodianId");

        foreach (var asset in assets)
        {
            sb.AppendLine($"{asset.PropertyCode},{asset.CurrentClassification},{asset.AcquisitionCost},{asset.AcquisitionDate:yyyy-MM-dd},{EscapeCsv(asset.CurrentAssignment?.Location)},{asset.Condition},{asset.Quantity},{asset.BookValue},{asset.IsDisposed},{asset.CurrentCustodianId}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string EscapeCsv(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
}

public sealed class ExportPhysicalAssetsSpec : Specification<PhysicalAsset>
{
    public ExportPhysicalAssetsSpec(ExportPhysicalAssetsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.Classification))
        {
            Query.Where(p => p.CurrentClassification.ToString() == request.Classification);
        }

        if (!string.IsNullOrWhiteSpace(request.Condition))
        {
            Query.Where(p => p.Condition == request.Condition);
        }

        if (request.IsDisposed.HasValue)
        {
            Query.Where(p => p.IsDisposed == request.IsDisposed.Value);
        }

        Query.OrderByDescending(p => p.Created);
    }
}

