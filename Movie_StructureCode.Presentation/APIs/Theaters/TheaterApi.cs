using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Theater.CreateTheater;
using Movie_StructureCode.Application.Features.UseCases.Commands.Theater.UpdateTheater;
using Movie_StructureCode.Application.Features.UseCases.Queries.Theater;
using Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaterById;
using Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheaters;
using Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersByMovie;
using Movie_StructureCode.Application.Features.UseCases.Queries.Theater.GetTheatersForAdmin;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Theaters
{
    public class TheaterApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/theaters";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Theaters")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── QUERY (USER) ──────────────────────────────────────────────────
            group.MapGet("/", GetTheatersAsync)
                .WithName("GetTheaters")
                .WithSummary("Lấy danh sách rạp (phân trang)")
                .Produces<PagedResult<TheaterUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:guid}", GetTheaterByIdAsync)
                .WithName("GetTheaterById")
                .WithSummary("Lấy chi tiết rạp theo ID")
                .Produces<TheaterUserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/movie/{movieId:guid}", GetTheatersByMovieAsync)
                .WithName("GetTheatersByMovie")
                .WithSummary("Lấy danh sách rạp chiếu phim")
                .Produces<IEnumerable<TheaterUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ── QUERY (ADMIN) ──────────────────────────────────────────────────
            group.MapGet("/admin/list", GetTheatersForAdminAsync)
                .WithName("GetTheatersForAdmin")
                .WithSummary("Admin: Lấy danh sách rạp (tìm kiếm + lọc IsActive + phân trang)")
                .Produces<PagedResult<TheaterAdminDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // ── COMMAND ──────────────────────────────────────────────────────
            group.MapPost("/", CreateTheaterAsync)
                .WithName("CreateTheater")
                .WithSummary("Tạo rạp mới")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateTheaterAsync)
                .WithName("UpdateTheater")
                .WithSummary("Cập nhật thông tin rạp")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

        }

        // ── HANDLERS ─────────────────────────────────────────────────────────

        private static async Task<IResult> GetTheatersAsync(
            ISender sender,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetTheaters.Query(pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetTheaterByIdAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetTheaterById.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetTheatersByMovieAsync(
            ISender sender,
            [FromRoute] Guid movieId,
            [FromQuery] DateTime date)
        {
            var query = new GetTheatersByMovie.Query(movieId, date);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetTheatersForAdminAsync(
            ISender sender,
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetTheatersForAdmin.Query(search, isActive, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateTheaterAsync(
            ISender sender,
            [FromBody] CreateTheaterRequest request)
        {
            var command = new CreateTheater.Command(request.Name, request.Location);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/theaters/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateTheaterAsync(
            ISender sender,
            [FromRoute] Guid id,
            [FromBody] UpdateTheaterRequest request)
        {
            var command = new UpdateTheater.Command(
                id,
                request.Name,
                request.Location,
                request.IsActive);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

    }

    // ── Request models ────────────────────────────────────────────────────────

    public sealed record CreateTheaterRequest(
        string Name,
        string? Location);

    public sealed record UpdateTheaterRequest(
        string Name,
        string? Location,
        bool IsActive);
}
