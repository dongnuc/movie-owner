using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Room.CreateRoom;
using Movie_StructureCode.Application.Features.UseCases.Commands.Room.UpdateRoom;
using Movie_StructureCode.Application.Features.UseCases.Queries.Room;
using Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsByTheater;
using Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomsForAdmin;
using Movie_StructureCode.Application.Features.UseCases.Queries.Room.GetRoomWithSeats;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;
using System.Security.Claims;

namespace Movie_StructureCode.Presentation.APIs.Rooms
{
    public class RoomApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/rooms";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Rooms")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── QUERY (USER) ──────────────────────────────────────────────────
            group.MapGet("/", GetRoomsByTheaterAsync)
                .WithName("GetRoomsByTheater")
                .WithSummary("Lấy danh sách phòng theo rạp (BẮT BUỘC theaterId + tìm kiếm + phân trang)")
                .Produces<PagedResult<RoomListUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapGet("/{showingId:guid}/layout", GetRoomWithSeatsUserAsync)
                .WithName("GetRoomWithSeatsUser")
                .WithSummary("User: Lấy chi tiết phòng kèm cấu trúc ghế (chỉ ghế có StatusSeat = Active)")
                .Produces<RoomDetailUserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .RequireAuthorization();

            // ── QUERY (ADMIN) - MUST come before user routes ──────────────────────────────────────────────────
            group.MapGet("/admin/{theaterId:guid}/{roomId:guid}/with-seats", GetRoomWithSeatsAdminAsync)
                .WithName("GetRoomWithSeatsAdmin")
                .WithSummary("Admin: Lấy chi tiết phòng kèm cấu trúc ghế (validation: room phải thuộc về theater)")
                .Produces<RoomDetailAdminDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status403Forbidden);

            // ── QUERY (ADMIN) ──────────────────────────────────────────────────
            group.MapGet("/admin/list", GetRoomsForAdminAsync)
                .WithName("GetRoomsForAdmin")
                .WithSummary("Admin: Lấy danh sách phòng (BẮT BUỘC theaterId + tìm kiếm + lọc IsActive + phân trang)")
                .Produces<PagedResult<RoomListAdminDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // ── COMMAND ──────────────────────────────────────────────────────
            group.MapPost("/", CreateRoomAsync)
                .WithName("CreateRoom")
                .WithSummary("Tạo phòng mới (tự động tạo ghế theo cấu trúc)")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateRoomAsync)
                .WithName("UpdateRoom")
                .WithSummary("Cập nhật thông tin phòng")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

        }

        // ── HANDLERS ─────────────────────────────────────────────────────────

        private static async Task<IResult> GetRoomsByTheaterAsync(
            ISender sender,
            [FromQuery] Guid theaterId,        // Bắt buộc - không nullable
            [FromQuery] string? search = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetRoomsByTheater.Query(theaterId, search, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetRoomWithSeatsUserAsync(
            ISender sender,
            HttpContext httpContext,
            [FromRoute] Guid showingId)
        {
            var user = httpContext.User;
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var query = new GetRoomWithSeatsUser.Query(showingId, userId);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetRoomWithSeatsAdminAsync(
            ISender sender,
            [FromRoute] Guid theaterId,
            [FromRoute] Guid roomId)
        {
            var query = new GetRoomWithSeatsAdmin.Query(theaterId, roomId);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetRoomsForAdminAsync(
            ISender sender,
            [FromQuery] Guid theaterId,         // Bắt buộc - không nullable
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetRoomsForAdmin.Query(theaterId, search, isActive, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateRoomAsync(
            ISender sender,
            [FromBody] CreateRoomRequest request)
        {
            var command = new CreateRoom.Command(
                request.TheaterId,
                request.Name,
                request.TotalRow,
                request.TotalCol);

            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/rooms/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateRoomAsync(
            ISender sender,
            [FromRoute] Guid id,
            [FromBody] UpdateRoomRequest request)
        {
            var command = new UpdateRoom.Command(id, request.Name, request.TotalRow, request.TotalCol, request.IsActive);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

    }

    // ── Request models ────────────────────────────────────────────────────────

    public sealed record CreateRoomRequest(
        Guid TheaterId,
        string Name,
        int TotalRow,
        int TotalCol);

    public sealed record UpdateRoomRequest(
        string Name,
        int TotalRow,
        int TotalCol,
        bool IsActive);
}
