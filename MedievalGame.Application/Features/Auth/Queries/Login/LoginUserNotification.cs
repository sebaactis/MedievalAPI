using MediatR;
using MedievalGame.Application.Features.Auth.Dto;

namespace MedievalGame.Application.Features.Auth.Queries.Login
{
    public record LoginUserNotification(UserDto userDto) : INotification
    {
    }
}
