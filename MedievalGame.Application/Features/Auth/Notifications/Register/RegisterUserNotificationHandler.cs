using MediatR;
using MedievalGame.Application.Features.Auth.Commands.Register;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Auth.Notifications.Register
{
    public class RegisterUserNotificationHandler(IUserAuditRepository auditRepo) : INotificationHandler<RegisterUserNotification>
    {
        public async Task Handle(RegisterUserNotification notification, CancellationToken cancellationToken)
        {
            var user = notification.userDto;

            var log = new UserAuditLog
            {
                UserId = user.Id,
                Username = user.Username,
                OperationType = "Register"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
