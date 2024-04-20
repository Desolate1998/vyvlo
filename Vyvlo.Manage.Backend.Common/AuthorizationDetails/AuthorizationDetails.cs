using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Vyvlo.Manage.Backend.Common.AuthorizationDetails;

public class AuthorizationDetails
{
    public string Ip { get; set; }
    public Guid UserId { get; set; }
    
    public static AuthorizationDetails Create(IHttpContextAccessor context)
    {
        AuthorizationDetails authorizationDetails = new();
        authorizationDetails.Ip = context.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? throw new NullReferenceException("Invalid Authorization Details");

        var parsed = Guid.TryParse(context.HttpContext?.User.FindFirstValue("sid"), out Guid userId);
        if (parsed)
        {
            authorizationDetails.UserId = userId;
        }
        return authorizationDetails;
    }
}
