namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed class ListPpeReceivingReportsResponse
{
    public IReadOnlyList<PpeReceivingReportDto> Reports { get; set; } = [];
}

public sealed class PpeReceivingReportDto
{
    public Guid Id { get; set; }
    public string RRNumber { get; set; } = string.Empty;
    public string ReceivedFrom { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int ItemsCount { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
