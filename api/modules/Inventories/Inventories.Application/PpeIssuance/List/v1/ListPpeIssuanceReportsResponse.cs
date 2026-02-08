namespace AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;

public sealed class ListPpeIssuanceReportsResponse
{
    public IReadOnlyList<PpeIssuanceReportDto> Reports { get; set; } = [];
}

public sealed class PpeIssuanceReportDto
{
    public Guid Id { get; set; }
    public string IRNumber { get; set; } = string.Empty;
    public string IssuedTo { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int LineItemsCount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
