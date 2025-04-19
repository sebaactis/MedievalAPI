using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.DeleteCharacter
{
    public record DeleteCharacterCommand(Guid Id) : IRequest<CharacterDto>
    {
    }
}
