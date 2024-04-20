using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vyvlo.Manage.Backend.Domain.Database;

public class UserCredentials
{
    public string Email { get; set; }
    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
    public Guid UserId { get; set; }
    public List<Tuple<string, DateTime>> EmailHistory { get; set; }
}
