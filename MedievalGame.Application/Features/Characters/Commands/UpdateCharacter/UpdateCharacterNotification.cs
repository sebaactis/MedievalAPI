using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.UpdateCharacter
{
    public record UpdateCharacterNotification(CharacterDto characterDto) : INotification
    {
    }
}
