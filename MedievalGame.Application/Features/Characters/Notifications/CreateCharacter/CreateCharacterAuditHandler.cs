using MediatR;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Notifications.CreateCharacter
{
    public class CreateCharacterAuditHandler(ICharacterAuditRepository auditRepo) : INotificationHandler<CreateCharacterNotification>
    {
        public async Task Handle(CreateCharacterNotification notification, CancellationToken cancellationToken)
        {
            var character = notification.characterDto;

            var log = new CharacterAuditLog
            {
                CharacterId = character.Id,
                Name = character.Name,
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
