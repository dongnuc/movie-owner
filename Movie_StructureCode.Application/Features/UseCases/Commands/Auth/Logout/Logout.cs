using Movie_StructureCode.Contract.Abstractions.Message;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Auth.Logout
{
    public static class Logout
    {
        /// <summary>
        /// Logout command - ??ng xu?t và revoke refresh tokens
        /// </summary>
        public sealed record Command(
            Guid UserId
        ) : ICommand<bool>;
    }
}
