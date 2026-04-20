using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.CreateSeatType;
using Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.UpdateSeatType;
using Movie_StructureCode.Application.Features.UseCases.Commands.SeatType.DeleteSeatType;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypes;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetSeatTypeById;
using Movie_StructureCode.Application.Features.UseCases.Queries.SeatType.GetAllSeatTypesForAdmin;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.SeatTypes
{
    public class SeatTypeApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/seattypes";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("SeatTypes")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ?? QUERY (USER) 
            group.MapGet("/", GetSeatTypesAsync)
                .WithName("GetSeatTypes")
                .WithSummary("Lấy danh sách loại ghế (search + phân trang) - chỉ active")
                .Produces<PagedResult<SeatTypeUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/{id:guid}", GetSeatTypeByIdAsync)
                .WithName("GetSeatTypeById")
                .WithSummary("Lấy chi tiết loại ghế theo ID")
                .Produces<SeatTypeUserDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ?? QUERY (ADMIN) 
            group.MapGet("/admin/list", GetAllSeatTypesForAdminAsync)
                .WithName("GetAllSeatTypesForAdmin")
                .WithSummary("Admin: Lấy danh sách tất cả loại ghế (active + inactive, search + filter + phân trang)")
                .Produces<PagedResult<SeatTypeAdminDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapGet("/admin/{id:guid}", GetSeatTypeByIdAdminAsync)
                .WithName("GetSeatTypeByIdAdmin")
                .WithSummary("Admin: Lấy chi tiết loại ghế theo ID (đầy đủ thông tin)")
                .Produces<SeatTypeAdminDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);

            // ?? COMMAND
            group.MapPost("/", CreateSeatTypeAsync)
                .WithName("CreateSeatType")
                .WithSummary("Tạo loại ghế mới")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateSeatTypeAsync)
                .WithName("UpdateSeatType")
                .WithSummary("Cập nhật loại ghế")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteSeatTypeAsync)
                .WithName("DeleteSeatType")
                .WithSummary("Xoá loại ghế")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        // ?? HANDLERS 

        private static async Task<IResult> GetSeatTypesAsync(
            ISender sender,
            [FromQuery] string? search = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetSeatTypes.Query(search, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetSeatTypeByIdAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetSeatTypeById.Query(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetAllSeatTypesForAdminAsync(
            ISender sender,
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllSeatTypesForAdmin.Query(search, isActive, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetSeatTypeByIdAdminAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var query = new GetSeatTypeById.AdminQuery(id);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateSeatTypeAsync(
            ISender sender,
            [FromBody] CreateSeatTypeRequest request)
        {
            var command = new CreateSeatType.Command(request.Name, request.Surcharge);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/seattypes/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateSeatTypeAsync(
            ISender sender,
            [FromRoute] Guid id,
            [FromBody] UpdateSeatTypeRequest request)
        {
            var command = new UpdateSeatType.Command(id, request.Name, request.Surcharge, request.IsActive);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

        private static async Task<IResult> DeleteSeatTypeAsync(
            ISender sender,
            [FromRoute] Guid id)
        {
            var command = new DeleteSeatType.Command(id);
            var result = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }
    }

    // ?? Request models ????????????????????????????????????????????????????????

    public sealed record CreateSeatTypeRequest(
        string Name,
        decimal Surcharge);

    public sealed record UpdateSeatTypeRequest(
        string Name,
        decimal Surcharge,
        bool IsActive);
}
