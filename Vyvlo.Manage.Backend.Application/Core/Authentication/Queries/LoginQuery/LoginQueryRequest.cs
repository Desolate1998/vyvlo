namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;

public record LoginQueryRequest(
               string Email,
               string Password
           );