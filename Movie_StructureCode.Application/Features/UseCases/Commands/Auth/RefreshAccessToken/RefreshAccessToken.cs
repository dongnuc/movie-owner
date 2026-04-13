using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Auth.RefreshAccessToken
{
    public static class RefreshAccessToken
    {
        /// <summary>
        /// Refresh access token command - c?p access token m?i t? refresh token
        /// </summary>
        public sealed record Command(
            Guid UserId,
            string RefreshToken
        ) : ICommand<RefreshAccessTokenResponse>;
    }

    /// <summary>
    /// Response khi refresh access token thành công
    /// </summary>
    public sealed record RefreshAccessTokenResponse(
        string AccessToken,
        DateTime AccessTokenExpiresAt
    );
}
