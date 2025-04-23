using MedievalGame.Domain.Entities;

namespace MedievalGame.Domain.Interfaces
{
    public interface IItemRepository
    {
        Task<Item?> GetByIdAsync(Guid id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item> AddAsync(Item weapon);
        Task<Item> UpdateAsync(Item weapon);
        Task<Item> DeleteAsync(Guid id);
    }
}
