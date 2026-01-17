using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

public interface IAssetPropertyCodeGenerator
{
    string Generate(AcceptanceItem item);
}

