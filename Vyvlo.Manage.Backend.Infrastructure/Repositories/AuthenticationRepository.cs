using Vyvlo.Manage.Backend.Domain.Database;
using Vyvlo.Manage.Backend.Domain.Repositories;
using Vyvlo.Manage.Backend.Infrastructure.Cassandra;

namespace Vyvlo.Manage.Backend.Infrastructure.Repositories;

internal class AuthenticationRepository(CassandraDB db) : IAuthenticationRepository
{
    async Task<User?> IAuthenticationRepository.GetUserByEmailAsync(string email)
    {
        
        var session = db.GetSession();
        
        var statement = (await session.PrepareAsync(@"
                   SELECT user_id,  email, first_name, last_name
                   FROM users
                   WHERE email= ? LIMIT 1")).Bind(email);

        var result = await session.ExecuteAsync(statement);
        var row = result.FirstOrDefault();


        if (row == null)
        {
            return null;
        }

        var user = new User
        {
            UserId = row.GetValue<Guid>("user_id"),
            Email = row.GetValue<string>("email"),
            FirstName = row.GetValue<string>("first_name"),
            LastName = row.GetValue<string>("last_name")
        };
        statement = (await session.PrepareAsync(@"
                   SELECT password_salt, password_hash
                   FROM user_credentials
                   WHERE email= ? LIMIT 1")).Bind(email);

        result = await session.ExecuteAsync(statement);
        row = result.FirstOrDefault();

        if (row == null)
        {
            return null;
        }

        user.Credentials = new UserCredentials
        {
            PasswordSalt = row.GetValue<string>("password_salt"),
            PasswordHash = row.GetValue<string>("password_hash")
        };

        return user;

    }

    async Task<User?> IAuthenticationRepository.GetUserFromRefreshToken(string token)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync(@"
                   SELECT user_id
                   FROM refresh_tokens
                   WHERE refresh_token= ? LIMIT 1")).Bind(token);

        var result = await session.ExecuteAsync(statement);
        var row = result.FirstOrDefault();
        if (row is null)
        {
            return null;
        }

        var userId = row.GetValue<Guid>("user_id");
        statement = (await session.PrepareAsync(@"
                   SELECT user_id,  email, first_name, last_name
                   FROM users
                   WHERE user_id= ? LIMIT 1")).Bind(userId);

        result = await session.ExecuteAsync(statement);
        row = result.FirstOrDefault();

        return row is null ? null : new User
        {
            UserId = row.GetValue<Guid>("user_id"),
            Email = row.GetValue<string>("email"),
            FirstName = row.GetValue<string>("first_name"),
            LastName = row.GetValue<string>("last_name")
        };
    }

    async Task<Dictionary<Guid, string>> IAuthenticationRepository.GetUserOwnedStoresAsync(Guid UserId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync(@"
                   select store_id,name from user_owned_stores where user_id = ?")).Bind(UserId);

        var result = await session.ExecuteAsync(statement);

        return result is null ? [] :
               result.ToDictionary(row => row.GetValue<Guid>("store_id"),
                                   row => row.GetValue<string>("name"));
    }

    async Task IAuthenticationRepository.LogUserLoginAsync(LoginHistory loginHistory)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync(@$"
                        INSERT INTO login_history(
                                	user_id, 
                                	login_date,
                                    login_time,
                                	email, 
                                	ip, 
                                	status
                                )
                                VALUES(?,?,?,?,?,?);")).Bind(
                                    loginHistory.UserId,
                                    loginHistory.LoginDate,
                                    loginHistory.LoginTime,
                                    loginHistory.Email,
                                    loginHistory.Ip,
                                    loginHistory.Status);

        await session.ExecuteAsync(statement);
    }

    async Task IAuthenticationRepository.RegisterAsync(User user)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync(@$"
                    INSERT INTO users(
	                    user_id, 
	                    created_date, 
	                    email, 
	                    first_name, 
	                    last_name, 
	                    phone_numbers
                    )
                    VALUES(?,?,?,?,?,?);")).Bind(
                            user.UserId,
                            user.CreatedDate,
                            user.Email,
                            user.FirstName,
                            user.LastName,
                            user.PhoneNumbers
                       );

        await session.ExecuteAsync(statement);

        var credentialsstatement = (await session.PrepareAsync(@"
                    INSERT INTO user_credentials(
                        email, 
                        password_salt, 
                        password_hash
                    )
                    VALUES(?,?,?);")).Bind(
                        user.Email,
                        user.Credentials.PasswordSalt,
                        user.Credentials.PasswordHash);

        await session.ExecuteAsync(credentialsstatement);
    }

    async Task IAuthenticationRepository.StoreRefreshToken(string token, Guid userId)
    {
        var session = db.GetSession();
        var statement = (await session.PrepareAsync(@"INSERT INTO refresh_tokens (refresh_token, user_id) VALUES (?, ?) USING TTL ?")).Bind(
                                            token,
                                            userId,
                                            259_200);//3 days

        await session.ExecuteAsync(statement);
    }

    async Task<bool> IAuthenticationRepository.UserExistsAsync(string email)
    {
        var sessions = db.GetSession();
        var statement = (await sessions.PrepareAsync("SELECT COUNT(*) FROM users where email = ? LIMIT 1")).Bind(email);
        var result = await sessions.ExecuteAsync(statement);
        return (result.FirstOrDefault()?.GetValue<long>(0) ?? 0) > 0;

    }
}
