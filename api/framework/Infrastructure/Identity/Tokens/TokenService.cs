using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Finbuckle.MultiTenant.Abstractions;
using AMIS.Framework.Core.Auth.Jwt;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Identity.Tokens;
using AMIS.Framework.Core.Identity.Tokens.Features.Generate;
using AMIS.Framework.Core.Identity.Tokens.Features.Refresh;
using AMIS.Framework.Core.Identity.Tokens.Models;
using AMIS.Framework.Core.Identity.Users.Events;
using AMIS.Framework.Infrastructure.Auth.Jwt;
using AMIS.Framework.Infrastructure.Identity.Audit;
using AMIS.Framework.Infrastructure.Identity.Users;
using AMIS.Framework.Infrastructure.Tenant;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Authorization;

namespace AMIS.Framework.Infrastructure.Identity.Tokens;

public sealed class TokenService : ITokenService
{
    private readonly UserManager<FshUser> _userManager;
    private readonly IMultiTenantContextAccessor<FshTenantInfo>? _multiTenantContextAccessor;
    private readonly JwtOptions _jwtOptions;
    private readonly IPublisher _publisher;

    public TokenService(IOptions<JwtOptions> jwtOptions, UserManager<FshUser> userManager, IMultiTenantContextAccessor<FshTenantInfo>? multiTenantContextAccessor, IPublisher publisher)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _multiTenantContextAccessor = multiTenantContextAccessor;
        _publisher = publisher;
    }

    public async Task<TokenResponse> GenerateTokenAsync(TokenGenerationCommand request, string ipAddress, CancellationToken cancellationToken)
    {
        var currentTenant = _multiTenantContextAccessor!.MultiTenantContext.TenantInfo;
        if (currentTenant == null) throw new UnauthorizedException();
        if (string.IsNullOrWhiteSpace(currentTenant.Id)
           || await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
           || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException();
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException("user is deactivated");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedException("email not confirmed");
        }

        if (currentTenant.Id != TenantConstants.Root.Id)
        {
            if (!currentTenant.IsActive)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} is deactivated");
            }

            if (DateTime.UtcNow > currentTenant.ValidUpto)
            {
                throw new UnauthorizedException($"tenant {currentTenant.Id} validity has expired");
            }
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }


    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenCommand request, string ipAddress, CancellationToken cancellationToken)
    {
        try
        {
            var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
            var userId = _userManager.GetUserId(userPrincipal);
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("Invalid token - no user ID found");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                // Try to get tenant from token to provide better diagnostics
                var tenantFromToken = userPrincipal.FindFirst(FshClaims.Tenant)?.Value ?? "unknown";
                throw new UnauthorizedException($"User not found (ID: {userId}, Tenant: {tenantFromToken})");
            }

            // Validate refresh token: must exist, match the sent token, and not be expired
            if (string.IsNullOrWhiteSpace(user.RefreshToken))
            {
                throw new UnauthorizedException("No refresh token stored for user");
            }

            if (user.RefreshToken != request.RefreshToken)
            {
                throw new UnauthorizedException("Refresh token mismatch");
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Refresh token expired");
            }

            return await GenerateTokensAndUpdateUser(user, ipAddress);
        }
        catch (SecurityTokenException ex)
        {
            throw new UnauthorizedException($"Invalid token format: {ex.Message}");
        }
    }
    private async Task<TokenResponse> GenerateTokensAndUpdateUser(FshUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);
        string refreshToken = GenerateRefreshToken();
        var expiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationInDays);

        // Update user with new refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = expiryTime;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            throw new UnauthorizedException($"Failed to update user: {errors}");
        }

        // Verify the user can be found after update
        var verifyUser = await _userManager.FindByIdAsync(user.Id);
        if (verifyUser is null)
        {
            throw new UnauthorizedException($"User cannot be found after token generation (ID: {user.Id})");
        }

        if (string.IsNullOrWhiteSpace(verifyUser.RefreshToken))
        {
            throw new UnauthorizedException($"Refresh token was not persisted (ID: {user.Id})");
        }

        await _publisher.Publish(new AuditPublishedEvent(new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Operation = "Token Generated",
                Entity = "Identity",
                UserId = new Guid(user.Id),
                DateTime = DateTime.UtcNow,
            }
        }));

        // Publish UserLoggedInEvent for cross-module communication (e.g., auto-create employee record)
        await _publisher.Publish(new UserLoggedInEvent(
            new Guid(user.Id),
            user.UserName,
            user.Email));

        return new TokenResponse(token, refreshToken, expiryTime);
    }

    private string GenerateJwt(FshUser user, string ipAddress) =>
    GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtOptions.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes),
           signingCredentials: signingCredentials,
           issuer: JwtAuthConstants.Issuer,
           audience: JwtAuthConstants.Audience
           );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private List<Claim> GetClaims(FshUser user, string ipAddress) =>
        new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new(FshClaims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(FshClaims.IpAddress, ipAddress),
            new(FshClaims.Tenant, _multiTenantContextAccessor!.MultiTenantContext.TenantInfo!.Id),
            new(FshClaims.ImageUrl, user.ImageUrl == null ? string.Empty : user.ImageUrl.ToString())
        };
    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
#pragma warning disable CA5404 // Do not disable token validation checks
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = JwtAuthConstants.Audience,
            ValidIssuer = JwtAuthConstants.Issuer,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
#pragma warning restore CA5404 // Do not disable token validation checks
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException("invalid token");
        }

        return principal;
    }
}
