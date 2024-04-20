using ErrorOr;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;

public class RegisterCommandHandler(IAuthenticationRepository repository) : IRequestHandler<RegisterCommand, ErrorOr<bool>>
{
    /// <summary>
    /// Handles the registration of a new user.
    ///   <list type="number">
    ///     <item>Check if the email is already in use</item> 
    ///     <item>Create a new user</item> 
    ///   </list>
    /// </summary>
    async Task<ErrorOr<bool>> IRequestHandler<RegisterCommand, ErrorOr<bool>>.Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
      
        if(await repository.UserExistsAsync(request.Data.Email))
        {
            return Error.Conflict("email", "User already exists");
        }

        var user = User.Create(request.Data.FirstName,
                               request.Data.LastName,
                               request.Data.PhoneNumbers,
                               request.Data.Email.ToLower(),
                               request.Data.Password);

        await repository.RegisterAsync(user);

        return true;
    }
}
