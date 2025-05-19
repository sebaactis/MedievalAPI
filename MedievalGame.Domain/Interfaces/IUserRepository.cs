using MedievalGame.Domain.Entities;

namespace MedievalGame.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(Guid id);
        Task<User> GetByUsernameAsync(string username);
    }
}
