using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vyvlo.Manage.Backend.Domain.Database;

namespace Vyvlo.Manage.Backend.Domain.Repositories;

public interface IAuthenticationRepository
{
    Task RegisterAsync(User user);
    Task<bool> UserExistsAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);
    Task LogUserLoginAsync(LoginHistory loginHistory);
    Task<Dictionary<Guid,string>> GetUserOwnedStoresAsync(Guid userId);
    Task StoreRefreshToken(string token, Guid userId);
    Task<User?> GetUserFromRefreshToken(string token);

}
