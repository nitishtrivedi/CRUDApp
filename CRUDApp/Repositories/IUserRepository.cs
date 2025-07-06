using CRUDApp.Models;

namespace CRUDApp.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(bool? isAdmin = null);
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<bool> UpdateAsync(User user, int id);
        Task<bool> DeleteAsync(int id);
    }
}
