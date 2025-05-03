using MediatR;
using MedievalGame.Application.Features.Weapons.Commands.CreateWeapon;
using MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Notifications.DeleteWeapon
{
    public class DeleteWeaponNotificationHandler(IWeaponAuditRepository auditRepo) : INotificationHandler<DeleteWeaponNotification>
    {
        public async Task Handle(DeleteWeaponNotification notification, CancellationToken cancellationToken)
        {
            var weapon = notification.weaponDto;

            var log = new WeaponAuditLog
            {
                WeaponId = weapon.Id,
                Name = weapon.Name,
                OperationType = "Delete"
            };
            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
