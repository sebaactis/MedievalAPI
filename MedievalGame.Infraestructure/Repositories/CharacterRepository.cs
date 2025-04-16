using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalGame.Infraestructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;

        public CharacterRepository(AppDbContext context)
            => _context = context;

        public async Task<Character?> GetByIdAsync(Guid id)
        {
            return await _context.Characters
                .Include(c => c.Weapons)
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Character>> GetAllAsync()
        {
            return await _context.Characters
                .Include(c => c.Weapons)
                .Include(c => c.Items)
                .ToListAsync();
        }

        public async Task<Guid> AddAsync(Character character)
        {
            await _context.Characters.AddAsync(character);
            await _context.SaveChangesAsync();
            return character.Id;
        }

        public async Task<Character> UpdateAsync(Character character)
        {
            var characterFind = await _context.Characters.FindAsync(character.Id);

            if(characterFind != null)
            {
                _context.Characters.Update(character);
                await _context.SaveChangesAsync();
                return characterFind;
            }

            return null;
        }

        public async Task<Character> DeleteAsync(Guid id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                return character;
            }

            return null;
        }
    }
}
