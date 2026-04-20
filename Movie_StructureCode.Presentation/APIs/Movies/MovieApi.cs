using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Movie.CreateMovie;
using Movie_StructureCode.Application.Features.UseCases.Commands.Movie.UpdateMovie;
using Movie_StructureCode.Application.Features.UseCases.Commands.Movie.DeleteMovie;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieById;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovies;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMovieWithShowings;
using Movie_StructureCode.Application.Features.UseCases.Queries.Movie.GetMoviesForAdmin;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Movies
{
    public class MovieApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/movies";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Movies")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── QUERY (USER) ──────────────────────────────────────────────────
            group.MapGet("/", GetMoviesAsync)
                .WithName("GetMovies")
                .WithSummary("Lấy danh sách phim (tìm kiếm + lọc category + phân trang)")
                .Produces<PagedResult<MovieUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // ── QUERY (ADMIN) ──────────────────────────────────────────────────
            // Admin routes phải đứng trước user routes để tránh conflict
            group.MapGet("/admin/list", GetMoviesForAdminAsync)
                .WithName("GetMoviesForAdmin")
                .WithSummary("Admin: Lấy danh sách phim (tìm kiếm + lọc category + lọc IsActive + phân trang)")
                .Produces<PagedResult<MovieAdminDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/admin/{id:guid}", GetMovieByIdAdminAsync)
                .WithName("GetMovieByIdAdmin")
                .WithSummary("Admin: Lấy chi tiết phim theo ID (đầy đủ thông tin)")
                .Produces<MovieAdminDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ── USER routes ──────────────────────────────────────────────────
            group.MapGet("/{id:guid}", GetMovieByIdAsync)
                .WithName("GetMovieById")
                .WithSummary("Lây chi tiết phim theo ID")
                .Produces<MovieUserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/{id:guid}/showings", GetMovieWithShowingsAsync)
                .WithName("GetMovieWithShowings")
                .WithSummary("Lấy phim kèm danh sách suất chiếu")
                .Produces<MovieWithShowingsUserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ── COMMAND ──────────────────────────────────────────────────────
            group.MapPost("/", CreateMovieAsync)
                .WithName("CreateMovie")
                .WithSummary("Tạo phim mới (check trùng title + category active)")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateMovieAsync)
                .WithName("UpdateMovie")
                .WithSummary("Cập nhật thông tin phim (check trùng title)")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteMovieAsync)
                .WithName("DeleteMovie")
                .WithSummary("Xoá phim: Hard delete nếu chưa có showing, Soft delete nếu có showing cũ (check không có active showing)")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        // ── HANDLERS ─────────────────────────────────────────────────────────

        private static async Task<IResult> GetMoviesAsync(
            ISender sender,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? search = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetMovies.Query(categoryId, search, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetMovieByIdAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetMovieById.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetMovieByIdAdminAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetMovieById.AdminQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetMovieWithShowingsAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetMovieWithShowings.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetMoviesForAdminAsync(
            ISender sender,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetMoviesForAdmin.Query(categoryId, search, isActive, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateMovieAsync(
            ISender sender,
            [FromBody] CreateMovieRequest request)
        {
            var command = new CreateMovie.Command(
                request.Title,
                request.Description,
                request.Image,
                request.BasePrice,
                request.Duration,
                request.CategoryId);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/movies/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateMovieAsync(
            ISender sender,
            [FromRoute] Guid id,
            [FromBody] UpdateMovieRequest request)
        {
            var command = new UpdateMovie.Command(
                id,
                request.Title,
                request.Description,
                request.Image,
                request.BasePrice,
                request.Duration,
                request.CategoryId,
                request.IsActive);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

        private static async Task<IResult> DeleteMovieAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var command = new DeleteMovie.Command(id);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }
    }

    // ── Request models ────────────────────────────────────────────────────────

    public sealed record CreateMovieRequest(
        string Title,
        string? Description,
        string? Image,
        decimal BasePrice,
        int Duration,
        Guid CategoryId);

    public sealed record UpdateMovieRequest(
        string Title,
        string? Description,
        string? Image,
        decimal BasePrice,
        int Duration,
        Guid CategoryId,
        bool IsActive);
}
