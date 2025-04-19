using MedievalGame.Domain.Entities;

namespace MedievalGame.Domain.Interfaces
{
    public interface IWeaponRepository
    {
        Task<Weapon?> GetByIdAsync(Guid id);
        Task<IEnumerable<Weapon>> GetAllAsync();
        Task<Weapon> AddAsync(Weapon weapon);
        Task<Weapon> UpdateAsync(Weapon weapon);
        Task<Weapon> DeleteAsync(Guid id);
    }
}
