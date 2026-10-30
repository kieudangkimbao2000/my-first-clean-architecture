using Infrastructure.Db;
using Domain.Entity;
using Application.Interface;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        User IUserRepository.GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        void IUserRepository.AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        void IUserRepository.UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        void IUserRepository.DeleteUser(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        async Task<User> IUserRepository.SaveChangesAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}