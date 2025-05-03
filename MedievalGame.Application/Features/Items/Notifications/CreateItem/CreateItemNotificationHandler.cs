using MediatR;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Notifications.CreateItem
{
    public class CreateItemNotificationHandler(IItemAuditRepository auditRepo) : INotificationHandler<CreateItemNotification>
    {
        public async Task Handle(CreateItemNotification notification, CancellationToken cancellationToken)
        {
            var item = notification.itemDto;

            var log = new ItemAuditLog
            {
                ItemId = item.Id,
                Name = item.Name,
                OperationType = "Create"
            };
            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }

}
