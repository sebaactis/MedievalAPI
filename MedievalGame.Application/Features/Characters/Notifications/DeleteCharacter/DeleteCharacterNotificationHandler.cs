using MediatR;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Notifications.DeleteCharacter
{
    public class DeleteCharacterNotificationHandler(ICharacterAuditRepository auditRepo) : INotificationHandler<DeleteCharacterNotification>
    {
        public async Task Handle(DeleteCharacterNotification notification, CancellationToken cancellationToken)
        {
            var character = notification.characterDto;

            var log = new CharacterAuditLog
            {
                CharacterId = character.Id,
                Name = character.Name,
                OperationType = "Delete"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
