using MediatR;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Notifications.CreateWeapon
{
    public class CreateWeaponNotificationHandler(IWeaponAuditRepository auditRepo) : INotificationHandler<CreateWeaponNotification>
    {
        public async Task Handle(CreateWeaponNotification notification, CancellationToken cancellationToken)
        {
            var weapon = notification.weaponDto;

            var log = new WeaponAuditLog
            {
                WeaponId = weapon.Id,
                Name = weapon.Name,
                OperationType = "Create"
            };
            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
