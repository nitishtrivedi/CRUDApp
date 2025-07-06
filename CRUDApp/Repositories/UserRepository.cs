using CRUDApp.Data;
using CRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDApp.Repositories
{
    public class UserRepository(AppDBContext dBContext) : IUserRepository
    {
        private readonly AppDBContext _dbContext = dBContext;

        public async Task<IEnumerable<User>> GetAllAsync(bool? isAdmin = null)
        {
            IEnumerable<User> users = await _dbContext.Users.AsNoTracking().ToListAsync();
            
            if (isAdmin.HasValue)
            {
                if (isAdmin == true)
                {
                    users = users.Where(x => x.IsAdmin == true);
                }
                else
                {
                    users = users.Where(x => x.IsAdmin == false);
                }
            }
                
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            return user;
        }
        public async Task<User> AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user, int id)
        {
            if (id != user.Id) return false;
            var existingUser = await _dbContext.Users.FindAsync(id);
            if (existingUser == null) return false;

            //Manually update fields
            existingUser.Username = user.Username;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Email = user.Email;
            existingUser.IsAdmin = user.IsAdmin;

            _dbContext.Entry(existingUser).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
