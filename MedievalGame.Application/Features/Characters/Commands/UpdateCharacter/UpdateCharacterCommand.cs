
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Enums;

namespace MedievalGame.Application.Features.Characters.Commands.UpdateCharacter
{
    public record UpdateCharacterCommand(
            Guid Id,
            string? Name,
            int? Life,
            int? Attack,
            int? Defense,
            int? Level,
            CharacterClass? Class
        ) : IRequest<CharacterDto>;
}
