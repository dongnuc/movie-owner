using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Application.Abstractions.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Movie_StructureCode.Domain.Entities;
using Movie_StructureCode.Application.Abstractions.Services.Cache;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Auth.RefreshAccessToken
{
    public sealed class RefreshAccessTokenHandler : ICommandHander<RefreshAccessToken.Command, RefreshAccessTokenResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenProvider _tokenProvider;
        private readonly ITokenCacheService _tokenCacheService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshAccessTokenHandler(
            UserManager<AppUser> userManager,
            ITokenProvider tokenProvider,
            ITokenCacheService tokenCacheService,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _tokenCacheService = tokenCacheService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<RefreshAccessTokenResponse>> Handle(
            RefreshAccessToken.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get user
                var user = await _userManager.FindByIdAsync(command.UserId.ToString());
                if (user == null)
                    return Result.Failure<RefreshAccessTokenResponse>(
                        new Error("Auth.UserNotFound", "User not found."));

                // Validate refresh token
                var refreshToken = await _refreshTokenService.GetRefreshTokenByTokenAsync(command.RefreshToken);

                if (refreshToken == null ||
                    refreshToken.UserId != command.UserId ||
                    refreshToken.IsRevoked ||
                    refreshToken.ExpriseDate <= DateTime.UtcNow)
                {
                    return Result.Failure<RefreshAccessTokenResponse>(
                        new Error("Auth.InvalidRefreshToken", "Invalid or expired refresh token."));
                }

                // jti older
                var oldJti = refreshToken.JwtId;
                //redis
                await _tokenCacheService.RemoveAccessTokenAsync(oldJti);
                await _tokenCacheService.RemoveUserSessionAsync(command.UserId.ToString(), oldJti);

                var newJti = Guid.NewGuid().ToString();

                // Generate new access token
                var accessToken = await _tokenProvider.GenerateAccessTokenAsync(user,newJti);
                var accessTokenExpiresAt = DateTime.UtcNow.AddHours(1);

                var response = new RefreshAccessTokenResponse(
                    accessToken,
                    accessTokenExpiresAt
                );

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                return Result.Failure<RefreshAccessTokenResponse>(
                    new Error("Auth.RefreshFailed", $"Token refresh failed: {ex.Message}"));
            }
        }
    }
}
