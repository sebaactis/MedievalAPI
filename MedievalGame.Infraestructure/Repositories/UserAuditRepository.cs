using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;

namespace MedievalGame.Infraestructure.Repositories
{
    public class UserAuditRepository(AppDbContext context) : IUserAuditRepository
    {
        public async Task SaveAsync(UserAuditLog log, CancellationToken cancellationToken)
        {
            await context.UserAuditLogs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
