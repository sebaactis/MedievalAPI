using MediatR;
using MedievalGame.Application.Features.Weapons.Commands.DeleteWeapon;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Weapons.Notifications.UpdateWeapon
{
    public class UpdateWeaponNotificationHandler(IWeaponAuditRepository auditRepo) : INotificationHandler<UpdateWeaponNotification>
    {
        public async Task Handle(UpdateWeaponNotification notification, CancellationToken cancellationToken)
        {
            var weapon = notification.weaponDto;

            var log = new WeaponAuditLog
            {
                WeaponId = weapon.Id,
                Name = weapon.Name,
                OperationType = "Update"
            };
            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
