using MediatR;
using MedievalGame.Application.Features.Items.Commands.UpdateItem;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Notifications.UpdateItem
{
    public class UpdateItemNotificationHandler(IItemAuditRepository auditRepo) : INotificationHandler<UpdateItemNotification>
    {
        public async Task Handle(UpdateItemNotification notification, CancellationToken cancellationToken)
        {
            var item = notification.itemDto;

            var log = new ItemAuditLog
            {
                ItemId = item.Id,
                Name = item.Name,
                OperationType = "Update"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
