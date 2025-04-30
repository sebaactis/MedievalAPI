using MedievalGame.Domain.Entities;

namespace MedievalGame.Domain.Interfaces
{
    public interface ICharacterAuditRepository
    {
        Task SaveAsync(CharacterAuditLog log, CancellationToken cancellationToken);
    }
}
