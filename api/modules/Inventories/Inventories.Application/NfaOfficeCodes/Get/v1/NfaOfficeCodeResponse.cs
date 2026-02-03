namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;

public sealed record NfaOfficeCodeResponse(
    Guid? Id,
    string Code,
    string OfficeName,
    string? Description,
    string? ParentOfficeCode,
    int SortOrder,
    bool IsActive);
