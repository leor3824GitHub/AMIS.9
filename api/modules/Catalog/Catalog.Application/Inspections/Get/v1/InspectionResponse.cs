namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public sealed class InspectionResponse
{
    public Guid Id { get; set; }
    public DateTime InspectionDate { get; set; }
    public string InspectedByName { get; set; }
    public string Remarks { get; set; }
    public Guid PurchaseOrderId { get; set; }
}
