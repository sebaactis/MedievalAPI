using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.DeleteCharacter
{
    public record DeleteCharacterNotification(CharacterDto characterDto) : INotification
    {
    }
}
