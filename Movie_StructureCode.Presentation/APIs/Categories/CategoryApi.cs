using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Movie_StructureCode.Application.Features.UseCases.Commands.Category.CreateCategory;
using Movie_StructureCode.Application.Features.UseCases.Commands.Category.DeleteCategory;
using Movie_StructureCode.Application.Features.UseCases.Commands.Category.UpdateCategory;
using Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategory;
using Movie_StructureCode.Application.Features.UseCases.Queries.Category.GetCategoriesForAdmin;
using Movie_StructureCode.Application.Features.UseCases.Queries.Category;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Presentation.Abstractions;

namespace Movie_StructureCode.Presentation.APIs.Categories
{
    public class CategoryApi : ApiEndpoint, ICarterModule
    {
        private const string BaseUrl = "/api/v{version:apiVersion}/categories";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.NewVersionedApi("Categories")
                .MapGroup(BaseUrl)
                .HasApiVersion(1);

            // ── QUERY (USER) ──────────────────────────────────────────────────
            group.MapGet("/", GetCategoriesAsync)
                .WithName("GetCategories")
                .WithSummary("Lấy danh sách category có phân trang")
                .Produces<PagedResult<CategoryUserDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // ── QUERY (ADMIN) ──────────────────────────────────────────────────
            group.MapGet("/admin/list", GetCategoriesForAdminAsync)
                .WithName("GetCategoriesForAdmin")
                .WithSummary("Admin: Lấy danh sách category (tìm kiếm + lọc IsActive + phân trang)")
                .Produces<PagedResult<CategoryAdminDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            // ── COMMAND ──────────────────────────────────────────────────────
            group.MapPost("/", CreateCategoryAsync)
                .WithName("CreateCategory")
                .WithSummary("Tạo mới category")
                .Produces<Guid>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);

            group.MapPut("/{id:guid}", UpdateCategoryAsync)
                .WithName("UpdateCategory")
                .WithSummary("Cập nhật category")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);

            group.MapDelete("/{id:guid}", DeleteCategoryAsync)
                .WithName("DeleteCategory")
                .WithSummary("Xoá category")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        // ── HANDLERS ─────────────────────────────────────────────────────────

        private static async Task<IResult> GetCategoriesAsync(
            ISender sender,
            [FromQuery] Guid?   id          = null,
            [FromQuery] int     pageNumber  = 1,
            [FromQuery] int     pageSize    = 10)
        {
            var query  = new GetCategories.Query(id, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> GetCategoriesForAdminAsync(
            ISender sender,
            [FromQuery] string? search = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetCategoriesForAdmin.Query(search, isActive, pageNumber, pageSize);
            var result = await sender.Send(query);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> CreateCategoryAsync(
            ISender                    sender,
            [FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategory.Command(request.Name);
            var result  = await sender.Send(command);

            return result.IsSuccess
                ? Results.Created($"/api/v1/categories/{result.Value}", result.Value)
                : HandlerFailure(result);
        }

        private static async Task<IResult> UpdateCategoryAsync(
            ISender sender,
            [FromRoute] Guid                    id,
            [FromBody]  UpdateCategoryRequest   request)
        {
            var command = new UpdateCategory.Command(id, request.Name, request.IsActive);
            var result  = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }

        private static async Task<IResult> DeleteCategoryAsync(
            ISender             sender,
            [FromRoute] Guid    id)
        {
            var command = new DeleteCategory.Command(id);
            var result  = await sender.Send(command);

            return result.IsSuccess
                ? Results.NoContent()
                : HandlerFailure(result);
        }
    }

    // ── Request models ────────────────────────────────────────────────────────

    public sealed record CreateCategoryRequest(string Name);

    public sealed record UpdateCategoryRequest(string Name, bool IsActive);
}
