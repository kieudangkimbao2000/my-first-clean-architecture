
using Application.Interface;
using Application.Model;
using Application.Model.Mapper;
using Application.Model.Response;
using Domain.Constant;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository repository;
        private readonly IPasswordService passwordService;
        private readonly IJWTService jwtService;
        private readonly ILogger<AuthService> logger;

        public AuthService(
            IUserRepository repository, 
            IPasswordService passwordService,
            IJWTService jwtService,
            ILogger<AuthService> logger)
        {
            this.repository = repository;
            this.passwordService = passwordService;
            this.jwtService = jwtService;
            this.logger = logger;
        }

        public ResponseDTO<LoginResponse> Login(LoginRequestDTO loginRequest)
        {
            var user = repository.GetUserByUsername(loginRequest.Username);

            if (user == null)
            {
                return new ResponseDTO<LoginResponse> { Success = false, Data = null, Message = MessageContant.LOGIN_FAILED };
            }

            if (!passwordService.VerifyPassword(loginRequest.Password, user.Password))
            {
                return new ResponseDTO<LoginResponse> { Success = false, Data = null, Message = MessageContant.LOGIN_FAILED };
            }

            var userDTO = user.ToUserDTO();

            var token = jwtService.GenerateToken(userDTO);

            return new ResponseDTO<LoginResponse> { 
                Success = true, 
                Data = new LoginResponse { Username = userDTO.Username, Role = userDTO.Role.ToString(), Token = token }, 
                Message = MessageContant.LOGIN_SUCCESS
            };
        }

        public ResponseDTO<RegisterResponse> Register(UserDTO user)
        {
            var hashedPassword = passwordService.HashPassword(user.Password);
            var entity = user.ToUser();
            entity.Password = hashedPassword;
            try
            {
                repository.AddUser(entity);

                return new ResponseDTO<RegisterResponse> { 
                    Success = true, 
                    Data = new RegisterResponse { Username = entity.Username, Role = entity.Role.ToString() }, 
                    Message = MessageContant.REGISTER_SUCCESS
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"Register failed: {ex.Message}");
                return new ResponseDTO<RegisterResponse> { 
                    Success = false, 
                    Data = null, 
                    Message = MessageContant.REGISTER_FAILED
                };
            }
        }

        public void UpdateUser(UserDTO user)
        {
            // Implement update user logic here
        }

        public void DeleteUser(string username)
        {
            // Implement delete user logic here
        }   
    }
}