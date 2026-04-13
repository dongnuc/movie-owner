using Movie_StructureCode.Contract.Abstractions.Message;
using Movie_StructureCode.Contract.Abstractions.Shared;
using Movie_StructureCode.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Movie_StructureCode.Application.Features.UseCases.Commands.Auth.Register
{
    public sealed class RegisterHandler : ICommandHander<Register.Command, Guid>
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> Handle(
            Register.Command command,
            CancellationToken cancellationToken)
        {
            // Validate password match
            if (command.Password != command.ConfirmPassword)
                return Result.Failure<Guid>(
                    new Error("Auth.PasswordMismatch", "Passwords do not match."));

            // Check if username already exists
            var userExists = await _userManager.FindByNameAsync(command.Username);
            if (userExists != null)
                return Result.Failure<Guid>(
                    new Error("Auth.UsernameTaken", $"Username '{command.Username}' is already taken."));

            // Check if email already exists
            var emailExists = await _userManager.FindByEmailAsync(command.Email);
            if (emailExists != null)
                return Result.Failure<Guid>(
                    new Error("Auth.EmailTaken", $"Email '{command.Email}' is already registered."));

            // Create new user
            var appUser = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = command.Username,
                Email = command.Email,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(appUser, command.Password);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure<Guid>(
                    new Error("Auth.RegistrationFailed", $"Registration failed: {errors}"));
            }

            return Result.Success(appUser.Id);
        }
    }
}
