using MediatR;
using MedievalGame.Application.Features.Auth.Dto;

namespace MedievalGame.Application.Features.Auth.Commands.Register
{
    public record RegisterUserNotification(UserDto userDto) : INotification
    {
    }
}
