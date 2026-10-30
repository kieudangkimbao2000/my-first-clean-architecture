using Application.Model;
using Application.Model.Response;

namespace Application.Interface
{
    public interface IAuthService
    {
        
        ResponseDTO<LoginResponse> Login(LoginRequestDTO loginRequest);
        ResponseDTO<RegisterResponse> Register(UserDTO user);
        void UpdateUser(UserDTO user);
        void DeleteUser(string username);
    }
}