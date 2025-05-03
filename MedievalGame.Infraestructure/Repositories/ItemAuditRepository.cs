using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;

namespace MedievalGame.Infraestructure.Repositories
{
    public class ItemAuditRepository(AppDbContext context) : IItemAuditRepository
    {
        public async Task SaveAsync(ItemAuditLog log, CancellationToken cancellationToken)
        {
            await context.ItemAuditLogs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
