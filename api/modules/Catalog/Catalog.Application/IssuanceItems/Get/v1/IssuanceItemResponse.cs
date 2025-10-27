using AMIS.WebApi.Catalog.Application.Products.Get.v1;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
public sealed record IssuanceItemResponse(
    Guid? Id, 
    Guid IssuanceId, 
    Guid ProductId, 
    int Qty, decimal UnitPrice, 
    string Status, 
    ProductResponse? Product
    );
