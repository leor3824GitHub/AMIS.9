using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Services;

public interface IAssetPropertyCodeGenerator
{
    string Generate(AcceptanceItem item);
}
