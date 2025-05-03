
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;

namespace MedievalGame.Infraestructure.Repositories
{
    public class WeaponAuditRepository(AppDbContext context) : IWeaponAuditRepository
    {
        public async Task SaveAsync(WeaponAuditLog log, CancellationToken cancellationToken)
        {
            await context.WeaponAuditLogs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
