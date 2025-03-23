using AMIS.Framework.Core.Identity.Tokens.Features.Generate;
using AMIS.Framework.Core.Identity.Tokens.Features.Refresh;
using AMIS.Framework.Core.Identity.Tokens.Models;

namespace AMIS.Framework.Core.Identity.Tokens;
public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(TokenGenerationCommand request, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request, string ipAddress, CancellationToken cancellationToken);

}
