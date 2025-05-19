using MedievalGame.Application.Features.Auth.Dto;

namespace MedievalGame.Application.Features.Auth.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }
}
