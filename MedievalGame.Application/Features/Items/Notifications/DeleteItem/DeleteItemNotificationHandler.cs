using MediatR;
using MedievalGame.Application.Features.Items.Commands.DeleteItem;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Notifications.DeleteItem
{
    public class DeleteItemNotificationHandler(IItemAuditRepository auditRepo) : INotificationHandler<DeleteItemNotification>
    {
        public async Task Handle(DeleteItemNotification notification, CancellationToken cancellationToken)
        {
            var item = notification.itemDto;

            var log = new ItemAuditLog
            {
                ItemId = item.Id,
                Name = item.Name,
                OperationType = "Delete"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
