namespace Vyvlo.Manage.Backend.Api.Contracts.Authentication.Register;

public class RegisterRequest
{
    public  string FirstName { get; set; }
    public  string LastName { get; set; }
    public  string Email { get; set; }
    public string Password { get; set; }
}
