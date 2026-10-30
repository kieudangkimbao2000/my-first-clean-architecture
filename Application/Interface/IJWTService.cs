using Application.Model;

namespace Application.Interface
{
    public interface IJWTService
    {
        string GenerateToken(UserDTO user);
    }
}