using Common.JwtTokenGenerator;
using ErrorOr;
using Infrastructure.Core.PasswordHelper;
using MediatR;
using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;

namespace Vyvlo.Manage.Backend.Application.Core.Authentication.Queries.LoginQuery;

public class LoginQueryHandler(IAuthenticationRepository repository, IJwtTokenGenerator tokenHelper) : IRequestHandler<LoginQuery, ErrorOr<LoginQueryResponse>>
{
    /// <summary>
    /// Handles the login query.
    /// <list type="number">
    /// <item> Check if the user exists</item>
    /// <item> Check if the password provided rehashed is the same as the one stored in the database under the user record</item>
    /// <item> Log the login attempt into the database</item>
    /// <item> Get a list of all user owned stores</item>
    /// <item> Generate a refresh token and a JWT token.</item>
    /// </list>
    /// </summary>
    public async Task<ErrorOr<LoginQueryResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(request.Data.Email.ToLower());
        if (user == null)
        {
            return Error.Unauthorized("Invalid login details");
        }

        var validPassword = PasswordHelper.ValidLoginPassword(request.Data.Password, user.Credentials.PasswordSalt, user.Credentials.PasswordHash);
        if (!validPassword)
        {
            await repository.LogUserLoginAsync(LoginHistory.Create(user.UserId, user.Email, request.Authorization.Ip, "Failed"));
            return Error.Unauthorized("Invalid login details");
        }

        await repository.LogUserLoginAsync(LoginHistory.Create(user.UserId, user.Email, request.Authorization.Ip, "Success"));

        var stores = await repository.GetUserOwnedStoresAsync(user.UserId);
        var refreshToken = tokenHelper.GenerateRefreshToken();

        await repository.StoreRefreshToken(refreshToken, user.UserId);
        return new LoginQueryResponse(user.FirstName, user.LastName, tokenHelper.GenerateToken(user.Email, user.FirstName, user.LastName, user.UserId), stores, refreshToken);
    }
}
