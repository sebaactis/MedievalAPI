using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public record CreateCharacterCommand(
            string Name,
            int Life,
            int Attack,
            int Defense,
            int Level,
            Guid CharacterClassId
        ) : IRequest<CharacterDto>;
}
