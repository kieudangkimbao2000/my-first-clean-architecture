using Domain.Entity;

namespace Application.Model.Mapper
{
    public static class UserMapper
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Role = user.Role
            };
        }

        public static User ToUser(this UserDTO userDTO)
        {
            return new User
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
                Email = userDTO.Email,
                Role = userDTO.Role
            };
        }
    }
}