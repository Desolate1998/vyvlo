using Common.DateTimeProvider;
using Infrastructure.Core.PasswordHelper;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vyvlo.Manage.Backend.Domain.Database;

public class User
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public HashSet<string> PhoneNumbers { get; set; }
    public UserCredentials Credentials { get; set; }
    public List<LoginHistory> LoginHistories { get; set; }
    public List<Store> Stores { get; set; }

    public static User Create(string name, string surname, HashSet<string> phoneNumbers, string email, string password)
    {
        var salt = PasswordHelper.GenerateSalt();

        return new User
        {
            FirstName = name,
            LastName = surname,
            PhoneNumbers = phoneNumbers,
            Email = email,
            UserId = Guid.NewGuid(),
            Credentials = new UserCredentials
            {
                Email = email,
                PasswordSalt = Convert.ToBase64String(salt),
                PasswordHash = PasswordHelper.HashPassword(password, salt),
            },
            CreatedDate = DateTimeProvider.ApplicationDate,
        };
    }

}
