using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using MedievalGame.Infraestructure.Data;


namespace MedievalGame.Infraestructure.Repositories
{
    public class CharacterAuditRepository(AppDbContext context) : ICharacterAuditRepository
    {

        public async Task SaveAsync(CharacterAuditLog log, CancellationToken cancellationToken)
        {
            await context.CharacterAuditLogs.AddAsync(log, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
