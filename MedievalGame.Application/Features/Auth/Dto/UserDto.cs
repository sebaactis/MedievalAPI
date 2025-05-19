using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Auth.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}
