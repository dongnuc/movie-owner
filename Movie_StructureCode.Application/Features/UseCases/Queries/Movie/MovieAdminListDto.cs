namespace Movie_StructureCode.Application.Features.UseCases.Queries.Movie
{
    public sealed record MovieAdminListDto(
        Guid     Id,
        string   Title,
        decimal  BasePrice,
        int      Duration,
        string?  CategoryName,
        bool     IsActive
    );

    public sealed record MovieListByTheaterId(
        Guid     Id,
        string   Title,
        string?  Image,
        decimal  BasePrice,
        int      Duration,
        string?  CategoryName
    );
}
