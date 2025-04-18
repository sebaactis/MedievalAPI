using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;

namespace MedievalGame.Infraestructure.Repositories
{
    public class WeaponRepository(AppDbContext context) : IWeaponRepository
    {
        public async Task<Weapon> AddAsync(Weapon weapon)
        {
            await context.AddAsync(weapon);
            await context.SaveChangesAsync();
            return weapon;
        }

        public Task<Weapon> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Weapon>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Weapon> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Weapon> UpdateAsync(Weapon weapon)
        {
            throw new NotImplementedException();
        }
    }
}
