namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed class ListPpeReceivingReportsResponse
{
    public IReadOnlyList<PpeReceivingReportDto> Reports { get; set; } = [];
}

public sealed class PpeReceivingReportDto
{
    public Guid Id { get; set; }
    public string ReportNumber { get; set; } = string.Empty;
    public string SourceName { get; set; } = string.Empty;
    public string ReceiptType { get; set; } = string.Empty;
    public DateTime SourceReceiptDate { get; set; }
    public int LineItemsCount { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
