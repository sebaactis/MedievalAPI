using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Infraestructure.Repositories
{
    public class CharacterRepository(AppDbContext context) : ICharacterRepository
    {

        public async Task<Character?> GetByIdAsync(Guid id)
        {
            return await context.Characters
                .Include(c => c.Weapons)
                    .ThenInclude(w => w.Rarity)
                .Include(c => c.Weapons)
                    .ThenInclude(w => w.WeaponType)
                .Include(c => c.CharacterItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.ItemType)
                .Include(c => c.CharacterItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.Rarity)
                .Include(c => c.CharacterClass)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await context.Characters
                .Include(c => c.Weapons)
                    .ThenInclude(w => w.Rarity)
                .Include(c => c.Weapons)
                    .ThenInclude(w => w.WeaponType)
                .Include(c => c.CharacterItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.ItemType)
                .Include(c => c.CharacterItems)
                    .ThenInclude(ci => ci.Item)
                        .ThenInclude(i => i.Rarity)
                .Include(c => c.CharacterClass)
                .ToListAsync();
        }

        public async Task<Character> AddAsync(Character character)
        {
            await context.Characters.AddAsync(character);
            await context.SaveChangesAsync();
            return character;
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            var characterFind = await context.Characters.FindAsync(character.Id);

            if (characterFind != null)
            {
                context.Characters.Update(character);
                await context.SaveChangesAsync();
                return characterFind;
            }

            return null;
        }

        public async Task<Character> DeleteAsync(Guid id)
        {
            var character = await context.Characters.FindAsync(id);
            if (character != null)
            {
                context.Characters.Remove(character);
                await context.SaveChangesAsync();
                return character;
            }

            return null;
        }
    }
}
