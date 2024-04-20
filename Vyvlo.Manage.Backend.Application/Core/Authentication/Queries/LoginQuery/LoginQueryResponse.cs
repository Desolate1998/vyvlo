namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;

public record LoginQueryResponse(string Name, string Surname, string Token, Dictionary<Guid, string> UserStores,string RefreshToken);
