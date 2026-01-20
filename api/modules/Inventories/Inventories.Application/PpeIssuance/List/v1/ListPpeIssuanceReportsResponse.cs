namespace AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;

public sealed class ListPpeIssuanceReportsResponse
{
    public IReadOnlyList<PpeIssuanceReportDto> Reports { get; set; } = [];
}

public sealed class PpeIssuanceReportDto
{
    public Guid Id { get; set; }
    public string ReportNumber { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string IssuanceType { get; set; } = string.Empty;
    public DateTime IssuanceDate { get; set; }
    public int LineItemsCount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
