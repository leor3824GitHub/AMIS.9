using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Application.PropertyCodes;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

public interface IAssetPropertyCodeGenerator
{
    Task<string> GenerateAsync(AcceptanceItem item, CancellationToken cancellationToken = default);
    Task<string> GenerateAsync(CoaPropertyCodeRequest request, CancellationToken cancellationToken = default);
}

