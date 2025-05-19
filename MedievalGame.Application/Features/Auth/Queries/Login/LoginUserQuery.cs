using MediatR;
using MedievalGame.Application.Features.Auth.Responses;

namespace MedievalGame.Application.Features.Auth.Queries.Login
{
    public record LoginUserQuery(string Username, string Password) : IRequest<AuthResponse>
    {
    }
}
