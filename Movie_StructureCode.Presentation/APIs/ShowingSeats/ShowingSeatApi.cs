using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.CreateShowingSeats;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockSeats;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.LockIndividualSeats;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.UnlockSeats;
using Movie_StructureCode.Application.Features.UseCases.Commands.ShowingSeat.MarkBrokenSeats;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSeatMap;
using Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

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
                .WithSummary("L?y c?u trúc gh? toàn phòng (organized by row) v?i th?ng kê Available/Locked/Booked")
                .Produces<SeatMapDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            //  COMMAND 
            group.MapPost("/", CreateShowingSeatsAsync)
                .WithName("CreateShowingSeats")
                .WithSummary("T?o hàng lo?t ShowingSeats cho su?t chi?u (t? ??ng t?o toàn b? gh? phòng, có th? lock m?t s? hàng)")
                .Produces<CreateShowingSeatsResult>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapPut("/lock-rows", LockSeatsAsync)
                .WithName("LockSeats")
                .WithSummary("Lock m?t ho?c nhi?u hàng gh? (khóa 10 phút, ch? Available ho?c lock h?t h?n)")
                .Produces<LockSeatsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapPut("/lock-individual", LockIndividualSeatsAsync)
                .WithName("LockIndividualSeats")
                .WithSummary("Lock riêng l? t?ng gh? (x? lý c?p b?ch: h?ng, VIP, b?o trì, v.v.)")
                .Produces<LockIndividualSeatsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapPut("/unlock", UnlockSeatsAsync)
                .WithName("UnlockSeats")
                .WithSummary("M? khóa gh? ?ang b? lock - tr? v? Available")
                .Produces<UnlockSeatsResult>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapPut("/mark-broken", MarkBrokenSeatsAsync)
                .WithName("MarkBrokenSeats")
                .WithSummary("?ánh d?u gh? h?ng v?nh vi?n (lock vô th?i h?n, IsActive = false)")
                .Produces<MarkBrokenSeatsResult>(StatusCodes.Status200OK)
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

        private static async Task<IResult> LockSeatsAsync(
            ISender sender,
            [FromBody] LockSeatsRequest request)
        {
            var command = new LockSeats.Command(
                request.ShowingId,
                request.RoomId,
                request.RowIndices);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> LockIndividualSeatsAsync(
            ISender sender,
            [FromBody] LockIndividualSeatsRequest request)
        {
            var command = new LockIndividualSeats.Command(
                request.ShowingId,
                request.RoomId,
                request.SeatIds,
                request.Reason);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UnlockSeatsAsync(
            ISender sender,
            [FromBody] UnlockSeatsRequest request)
        {
            var command = new UnlockSeats.Command(
                request.ShowingId,
                request.RoomId,
                request.SeatIds);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> MarkBrokenSeatsAsync(
            ISender sender,
            [FromBody] MarkBrokenSeatsRequest request)
        {
            var command = new MarkBrokenSeats.Command(
                request.ShowingId,
                request.RoomId,
                request.SeatIds,
                request.Reason);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }
    }

    //  Request models 

    public sealed record CreateShowingSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        decimal Price,
        IEnumerable<int>? LockedRowIndices = null);

    public sealed record LockSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        IEnumerable<int> RowIndices);

    public sealed record LockIndividualSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        IEnumerable<Guid> SeatIds,
        string? Reason = null);

    public sealed record UnlockSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        IEnumerable<Guid> SeatIds);

    public sealed record MarkBrokenSeatsRequest(
        Guid ShowingId,
        Guid RoomId,
        IEnumerable<Guid> SeatIds,
        string? Reason = null);
}
