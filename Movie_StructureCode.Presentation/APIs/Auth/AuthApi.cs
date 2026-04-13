using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Auth.Logout;
using Movie_StructureCode.Application.Features.UseCases.Commands.Auth.RefreshAccessToken;
using Movie_StructureCode.Application.Features.UseCases.Commands.Auth.Register;
using Movie_StructureCode.Application.Features.UseCases.Queries.Auth.Login;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Auth
{
    public class AuthApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/auth";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Auth")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── REGISTER ─────────────────────────────────────────────────
            group.MapPost("/register", RegisterAsync)
                .WithName("Register")
                .WithSummary("Đăng ký tài khoản mới (username, email, password, re-password)")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .AllowAnonymous();

            // ── LOGIN ────────────────────────────────────────────────────
            group.MapPost("/login", LoginAsync)
                .WithName("Login")
                .WithSummary("Đăng nhập (username hoặc email + password)")
                .Produces<LoginResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .AllowAnonymous();

            // ── REFRESH TOKEN ───────────────────────────────────────────
            group.MapPost("/refresh", RefreshAccessTokenAsync)
                .WithName("RefreshAccessToken")
                .WithSummary("Cấp access token mới từ refresh token")
                .Produces<RefreshAccessTokenResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .AllowAnonymous();

            // ── LOGOUT ───────────────────────────────────────────────────
            group.MapPost("/logout", LogoutAsync)
                .WithName("Logout")
                .WithSummary("Đăng xuất (revoke refresh tokens)")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized);
        }

        // ── HANDLERS ─────────────────────────────────────────────────────

        private static async Task<IResult> RegisterAsync(
            ISender sender,
            [FromBody] RegisterRequest request)
        {
            var command = new Register.Command(
                request.Username,
                request.Email,
                request.Password,
                request.ConfirmPassword);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/auth/profile/{result.Value}", new { userId = result.Value })
                : HandlerFailure(result);
        }

        private static async Task<IResult> LoginAsync(
            ISender sender,
            [FromBody] LoginRequest request)
        {
            var query = new Login.Query(request.UsernameOrEmail, request.Password);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> RefreshAccessTokenAsync(
            ISender sender,
            [FromBody] RefreshTokenRequest request)
        {
            var command = new RefreshAccessToken.Command(request.UserId, request.RefreshToken);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> LogoutAsync(
            ISender sender,
            [FromBody] LogoutRequest request)
        {
            var command = new Logout.Command(request.UserId);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }
    }

    // ── Request models ────────────────────────────────────────────────────────

    /// <summary>
    /// Register request - tạo tài khoản mới
    /// </summary>
    public sealed record RegisterRequest(
        string Username,
        string Email,
        string Password,
        string ConfirmPassword);

    /// <summary>
    /// Login request - đăng nhập bằng username hoặc email
    /// </summary>
    public sealed record LoginRequest(
        string UsernameOrEmail,
        string Password);

    /// <summary>
    /// Refresh token request - cấp access token mới
    /// </summary>
    public sealed record RefreshTokenRequest(
        Guid UserId,
        string RefreshToken);

    /// <summary>
    /// Logout request - đăng xuất
    /// </summary>
    public sealed record LogoutRequest(
        Guid UserId);
}
