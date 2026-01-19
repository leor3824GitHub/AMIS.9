using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;

public sealed class GetInventoryTransactionLogSpecs : Specification<InventoryTransactionLog>
{
    public GetInventoryTransactionLogSpecs(Guid id)
    {
        Query.Where(x => x.Id == id);
    }
}
