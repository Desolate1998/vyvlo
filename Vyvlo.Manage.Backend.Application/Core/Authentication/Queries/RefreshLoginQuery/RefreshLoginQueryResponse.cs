namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.RefreshLoginQuery;

public record RefreshLoginQueryResponse(string Name, string Surname, string Token, Dictionary<Guid, string> UserStores, string RefreshToken);
