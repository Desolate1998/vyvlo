using Common.DateTimeProvider;

namespace Vyvlo.Manage.Backend.Domain.Database;

public class LoginHistory
{
    public Guid UserId { get; set; }
    public int LoginDate { get; set; }
    public string Email { get; set; }
    public string Ip { get; set; }
    public string Status { get; set; }
    public long LoginTime { get; set; }

    public static LoginHistory Create(Guid userId, string email, string ip, string status)
    {
        return new LoginHistory
        {
            UserId = userId,
            Email = email,
            Ip = ip,
            Status = status,
            LoginDate = int.Parse(DateTimeProvider.ApplicationDate.ToString("yyyyMMdd")),
            LoginTime = DateTimeProvider.ApplicationDate.Ticks
        };
    }
}
