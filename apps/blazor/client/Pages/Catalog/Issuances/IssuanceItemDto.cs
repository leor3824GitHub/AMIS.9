using System;

namespace AMIS.Blazor.Client.Pages.Catalog.Issuances;

public class IssuanceItemDto
{
    public Guid Id { get; set; }
    public Guid? IssuanceId { get; set; }
    public Guid? ProductId { get; set; }
    public int Qty { get; set; }
    public double UnitPrice { get; set; }
    public string Status { get; set; } = "Pending";
}
