namespace Common.JwtTokenGenerator;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string firstName, string lastName, Guid userId);
    string GenerateRefreshToken();
}
