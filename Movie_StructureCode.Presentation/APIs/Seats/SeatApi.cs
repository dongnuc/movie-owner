using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat.CreateSeatsForRoom;
using Movie_StructureCode.Application.Features.UseCases.Commands.Seat.UpdateSeats;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Seats
{
    public class SeatApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/seats";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Seats")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ?? COMMAND 
            group.MapPut("/batch-update", UpdateSeatsBatchAsync)
                .WithName("BatchUpdateSeats")
                .WithSummary("Admin: Batch update seats (update trên memory, save 1 lần, return kết quả thành công thất bại)")
                .Produces<BatchUpdateSeatsResultDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPost("/create-for-room", CreateSeatsForRoomAsync)
                .WithName("CreateSeatsForRoom")
                .WithSummary("Tạo toàn bộ seats cho phòng trong 1 transaction (A1, A2, ..., B1, B2, ...)")
                .Produces<CreateSeatsForRoomResult>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        // ?? HANDLERS 

        private static async Task<IResult> CreateSeatsForRoomAsync(
            ISender sender,
            [FromBody] CreateSeatsForRoomRequest request)
        {
            var command = new CreateSeatsForRoom.Command(
                request.RoomId,
                request.TotalRow,
                request.TotalCol);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/seats", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateSeatsBatchAsync(
            ISender sender,
            [FromBody] UpdateSeatsBatchRequest request)
        {
            var command = new UpdateSeats.Command(request.Seats);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }
    }

    // ?? Request models 

    /// <summary>
    /// Request model để tạo seats cho room
    /// </summary>
    public sealed record CreateSeatsForRoomRequest(
        Guid RoomId,
        int TotalRow,
        int TotalCol);

    /// <summary>
    /// Request model cho batch update seats
    /// </summary>
    public sealed record UpdateSeatsBatchRequest(
        IEnumerable<UpdateSeatDto> Seats);
}
