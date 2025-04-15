using MedievalGame.Domain.Entities;

namespace MedievalGame.Domain.Interfaces
{
    public interface ICharacterRepository
    {
        Task<Character?> GetByIdAsync(Guid id);
        Task<List<Character>> GetAllAsync();
        Task<Guid> AddAsync(Character character);
        Task<Character> UpdateAsync(Character character);
        Task<Character> DeleteAsync(Guid id);
    }
}
