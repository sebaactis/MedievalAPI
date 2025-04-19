using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Weapon> DeleteAsync(Guid id)
        {
            var weapon = await context.Weapons.FindAsync(id);
            if (weapon != null)
            {
                context.Weapons.Remove(weapon);
                await context.SaveChangesAsync();
                return weapon;
            }

            return null;
        }

        public async Task<IEnumerable<Weapon>> GetAllAsync()
        {
            return await context.Weapons
                    .Include(w => w.Rarity)
                    .Include(w => w.WeaponType)
                    .ToListAsync();
        }

        public async Task<Weapon?> GetByIdAsync(Guid id)
        {
            return await context.Weapons
                    .Include(w => w.Rarity)
                    .Include(w => w.WeaponType)
                    .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Weapon> UpdateAsync(Weapon weapon)
        {
            var weaponFind = await context.Weapons.FindAsync(weapon.Id);

            if (weaponFind != null)
            {
                context.Weapons.Update(weapon);
                await context.SaveChangesAsync();
                return weaponFind;
            }

            return null;
        }
    }
}
