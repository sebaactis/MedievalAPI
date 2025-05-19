using MediatR;
using MedievalGame.Application.Features.Auth.Commands.Register;
using MedievalGame.Application.Features.Auth.Queries.Login;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Auth.Notifications.Login
{
    public class LoginUserNotificationHandler(IUserAuditRepository auditRepo) : INotificationHandler<LoginUserNotification>
    {
        public async Task Handle(LoginUserNotification notification, CancellationToken cancellationToken)
        {
            var user = notification.userDto;

            var log = new UserAuditLog
            {
                UserId = user.Id,
                Username = user.Username,
                OperationType = "Login"
            };

            await auditRepo.SaveAsync(log, cancellationToken);
        }
    }
}
