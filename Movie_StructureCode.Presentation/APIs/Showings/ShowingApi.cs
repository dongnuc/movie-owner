using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Showing.CreateShowing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Showing.UpdateShowing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Showing.DeleteShowing;
using Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByMovie;
using Movie_StructureCode.Application.Features.UseCases.Queries.Showing.GetShowingsByDateRange;
using Movie_StructureCode.Application.Features.UseCases.Queries.Showing;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Showings
{
    public class ShowingApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/showings";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Showings")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── QUERY ────────────────────────────────────────────────────────

            group.MapGet("/movie/{movieId:guid}", GetShowingsByMovieAsync)
                .WithName("GetShowingsByMovie")
                .WithSummary("Lấy danh sách suất chiếu theo phim")
                .Produces<IEnumerable<ShowingDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/date-range", GetShowingsByDateRangeAsync)
                .WithName("GetShowingsByDateRange")
                .WithSummary("Lấy danh sách rạp và phòng chiếu phim trong một ngày")
                .Produces<IEnumerable<ShowingsSummaryDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ── COMMAND ──────────────────────────────────────────────────────
            group.MapPost("/", CreateShowingAsync)
                .WithName("CreateShowing")
                .WithSummary("Tạo suất chiếu mới (Admin only)")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateShowingAsync)
                .WithName("UpdateShowing")
                .WithSummary("Cập nhật suất chiếu (Admin only)")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteShowingAsync)
                .WithName("DeleteShowing")
                .WithSummary("Xoá suất chiếu (Admin only)")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        // ── HANDLERS ─────────────────────────────────────────────────────────

        private static async Task<IResult> GetShowingsByMovieAsync(
            ISender sender,
            [FromRoute] Guid movieId,
            [FromQuery] Guid? theaterId = null,
            [FromQuery] DateTime? date = null)
        {
            var query = new GetShowingsByMovie.Query(movieId, theaterId, date);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetShowingsByDateRangeAsync(
            ISender sender,
            [FromQuery] Guid movieId,
            [FromQuery] DateTime date)
        {
            var query = new GetShowingsByDateRange.Query(movieId, date);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateShowingAsync(
            ISender sender,
            [FromBody] CreateShowingRequest request)
        {
            var command = new CreateShowing.Command(
                request.MovieId,
                request.RoomId,
                request.TimeStart,
                request.Price,
                request.MaxTicketsPerUser);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/showings/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateShowingAsync(
            ISender sender,
            [FromRoute] Guid id,
            [FromBody] UpdateShowingRequest request)
        {
            var command = new UpdateShowing.Command(
                id,
                request.TimeStart,
                request.Price,
                request.MaxTicketsPerUser,
                request.IsActive);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

        private static async Task<IResult> DeleteShowingAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var command = new DeleteShowing.Command(id);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }
    }

    // ── Request models ────────────────────────────────────────────────────────

    public sealed record CreateShowingRequest(
        Guid MovieId,
        Guid RoomId,
        DateTime TimeStart,
        decimal Price,
        int MaxTicketsPerUser);

    public sealed record UpdateShowingRequest(
        DateTime TimeStart,
        decimal Price,
        int MaxTicketsPerUser,
        bool IsActive);
}
