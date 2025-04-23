using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedievalGame.Infraestructure.Repositories
{
    public class ItemRepository(AppDbContext context) : IItemRepository
    {
        public async Task<Item> AddAsync(Item item)
        {
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var itemWithIncludes = await context.Items
                .Include(i => i.ItemType)
                .Include(i => i.Rarity)
                .FirstOrDefaultAsync(i => i.Id == item.Id);

            return itemWithIncludes!;
        }

        public async Task<Item> DeleteAsync(Guid id)
        {
            var item = await context.Items.FindAsync(id);
            if (item != null)
            {
                context.Items.Remove(item);
                await context.SaveChangesAsync();
                return item;
            }

            return null;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await context.Items
                    .Include(i => i.Rarity)
                    .Include(i => i.ItemType)
                    .ToListAsync();          
        }

        public async Task<Item?> GetByIdAsync(Guid id)
        {
            return await context.Items
                    .Include(i => i.Rarity)
                    .Include(i => i.ItemType)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            var itemFind = await context.Items.FindAsync(item.Id);

            if (itemFind != null)
            {
                context.Items.Update(item);
                await context.SaveChangesAsync();
                return itemFind;
            }

            return null;
        }
    }
}
