using MedievalGame.Domain.Entities;

namespace MedievalGame.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
