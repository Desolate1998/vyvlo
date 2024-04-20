namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Commands.RegisterCommand;

public record RegisterCommandRequest(
    string FirstName,
    string LastName,
    HashSet<string> PhoneNumbers,
    string Email,
    string Password
 );
