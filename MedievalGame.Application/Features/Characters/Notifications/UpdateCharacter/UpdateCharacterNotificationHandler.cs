using MediatR;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Notifications.UpdateCharacter
{
    public class UpdateCharacterNotificationHandler(ICharacterAuditRepository auditRepo) : INotificationHandler<UpdateCharacterNotification>
    {
        public async Task Handle(UpdateCharacterNotification notification, CancellationToken cancellationToken)
        {
            var character = notification.characterDto;

            var log = new CharacterAuditLog
            {
                CharacterId = character.Id,
                Name = character.Name,
                OperationType = "Update"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
