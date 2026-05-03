using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Queries.ShowingSeat.GetSessionRemainingSeconds
{
    public static class GetSessionRemainingSeconds
    {
        public sealed record Query(
            Guid ShowingId,
            string UserId
        ) : IQuery<SessionRemainingSecondsDto>
        {
        }

        /// <summary>
        /// DTO for session remaining seconds
        /// </summary>
        public sealed record SessionRemainingSecondsDto(
            int? RemainingSeconds
        );
    }
}
