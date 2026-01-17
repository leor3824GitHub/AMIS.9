namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.DTOs;

public record RecipientInfoDto
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string? ContactNumber { get; init; }
}
