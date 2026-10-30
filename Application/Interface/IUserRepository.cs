using Domain.Entity;

namespace Application.Interface
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string username);
        Task<User> SaveChangesAsync(User user);
    }
}