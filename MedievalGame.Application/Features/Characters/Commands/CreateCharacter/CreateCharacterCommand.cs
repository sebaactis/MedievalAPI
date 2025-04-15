using MediatR;
using MedievalGame.Domain.Enums;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public record CreateCharacterCommand(
            string Name,
            int Life,
            int Attack,
            int Defense,
            int Level,
            CharacterClass Class
        ) : IRequest<Guid>;
}
