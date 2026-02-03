namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;

public sealed record PpeCategoryCodeResponse(
    Guid? Id,
    string Code,
    string AccountCode,
    string Name,
    string? Description,
    int SortOrder,
    bool IsActive,
    string? COAReference);
