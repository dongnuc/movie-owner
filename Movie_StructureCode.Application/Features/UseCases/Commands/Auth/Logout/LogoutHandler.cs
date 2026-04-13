using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Application.Abstractions.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Auth.Logout
{
    public sealed class LogoutHandler : ICommandHander<Logout.Command, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutHandler(
            UserManager<AppUser> userManager,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<bool>> Handle(
            Logout.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                // Get user
                var user = await _userManager.FindByIdAsync(command.UserId.ToString());
                if (user == null)
                    return Result.Failure<bool>(
                        new Error("Auth.UserNotFound", "User not found."));

                // Revoke all refresh tokens
                await _refreshTokenService.RevokeAllRefreshTokensAsync(command.UserId);

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                return Result.Failure<bool>(
                    new Error("Auth.LogoutFailed", $"Logout failed: {ex.Message}"));
            }
        }
    }
}
