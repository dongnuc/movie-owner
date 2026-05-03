using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.CreateShowingSeats;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.SyncUserSeats;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatCount;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatMap;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSessionRemainingSeconds;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;
using System.Security.Claims;
using static Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.SyncUserSeats.SyncUserSeatsRequest;

namespace Movie_StructureCode.Presentation.APIs.ShowingSeats
{
    public class ShowingSeatApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/showing-seats";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("ShowingSeats")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            //  QUERY 
            group.MapGet("/seat-map", GetSeatMapAsync)
                .WithName("GetSeatMap")
                .WithSummary("Lấy cấu trúc ghế toàn phòng (organized by row) với thống kê Available/Locked/Booked")
                .Produces<SeatMapDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/seat-count/{showingId:guid}", GetSeatCountAsync)
                .WithName("GetSeatCount")
                .WithSummary("Lấy số lượng ghế theo trạng thái (Available/Locked/Booked) của suất chiếu")
                .Produces<GetSeatCount.SeatCountDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/session-remaining-seconds/{showingId:guid}", GetSessionRemainingSecondsAsync)
                .WithName("GetSessionRemainingSeconds")
                .WithSummary("Lấy số giây còn lại của phiên session holding (dùng cho countdown timer) - userId từ access token")
                .Produces<GetSessionRemainingSeconds.SessionRemainingSecondsDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .RequireAuthorization();

            //  COMMAND - USER SEAT SYNC (with access token extraction)
            group.MapPost("/sync", SyncUserSeatsAsync)
                .WithName("SyncUserSeats")
                .WithSummary("Đồng bộ ghế cho user (so sánh trước/sau, lock ghế mới, unlock ghế bỏ - userId từ access token)")
                .Produces<SyncUserSeatsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .RequireAuthorization();

            //  COMMAND - ADMIN LOCK / UNLOCK
            group.MapPost("/", CreateShowingSeatsAsync)
                .WithName("CreateShowingSeats")
                .WithSummary("Tạo hàng loạt ShowingSeats cho suất chiếu (tự động tạo toàn bộ ghế phòng, có thể lock một số hàng)")
                .Produces<CreateShowingSeatsResult>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        //  HANDLERS 

        private static async Task<IResult> GetSeatMapAsync(
            ISender sender,
            [FromQuery] Guid showingId,
            [FromQuery] Guid roomId)
        {
            var query = new GetSeatMap.Query(showingId, roomId);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetSeatCountAsync(
            ISender sender,
            [FromRoute] Guid showingId)
        {
            var query = new GetSeatCount.Query(showingId);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        /// <summary>
        /// Get session remaining seconds - extracts userId from access token via HttpContext
        /// Returns remaining seconds for countdown timer functionality
        /// </summary>
        private static async Task<IResult> GetSessionRemainingSecondsAsync(
            ISender sender,
            [FromRoute] Guid showingId,
            HttpContext httpContext)
        {
            try
            {
                // STEP 1: Extract userId from access token
                var user = httpContext.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    return Results.Unauthorized();

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? user.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Results.Unauthorized();

                // STEP 2: Validate request
                if (showingId == Guid.Empty)
                {
                    var error = Result.Failure(new Error("Showing.Invalid", "ShowingId is required."));
                    return HandlerFailure(error);
                }

                // STEP 3: Create query
                var query = new GetSessionRemainingSeconds.Query(showingId, userId);

                // STEP 4: Execute handler
                var result = await sender.Send(query);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : HandlerFailure(result);
            }
            catch (Exception ex)
            {
                var error = Result.Failure(new Error("Session.Error", $"Error getting session remaining seconds: {ex.Message}"));
                return HandlerFailure(error);
            }
        }

        /// <summary>
        /// Sync user seats - extracts userId from access token via HttpContext
        /// Handles differential logic: lock new seats, unlock deselected seats
        /// </summary>
        private static async Task<IResult> SyncUserSeatsAsync(
            ISender sender,
            [FromBody] SyncUserSeatsRequest request,
            HttpContext httpContext)
        {
            try
            {
                // STEP 1: Extract userId from access token
                // Try multiple sources for userId
                var user = httpContext.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    return Results.Unauthorized();

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? user.FindFirst("sub")?.Value;

                // STEP 2: Validate request
                if (request.ShowingId == Guid.Empty)
                {
                    var error = Result.Failure(new Error("Showing.Invalid", "ShowingId is required."));
                    return HandlerFailure(error);
                }

                // STEP 3: Create command with userId from token
                var command = new Command(
                    request.ShowingId,
                    userId,
                    request.SelectedSeatIds ?? new List<Guid>());

                // STEP 4: Execute handler
                var result = await sender.Send(command);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : HandlerFailure(result);
            }
            catch (Exception ex)
            {
                var error = Result.Failure(new Error("Sync.Error", $"Error syncing seats: {ex.Message}"));
                return HandlerFailure(error);
            }
        }

        private static async Task<IResult> CreateShowingSeatsAsync(
            ISender sender,
            [FromBody] CreateShowingSeatsRequest request)
        {
            var command = new CreateShowingSeats.Command(
                request.ShowingId,
                request.RoomId,
                request.Price,
                request.LockedRowIndices);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/showing-seats", result.Value)
                : HandlerFailure(result);
        }
    }

    //  Request models 

    public sealed record CreateShowingSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        decimal Price,
        IEnumerable<int>? LockedRowIndices = null);

    public sealed record SyncUserSeatsRequest(
        Guid ShowingId,
        IEnumerable<Guid> SelectedSeatIds);
}
