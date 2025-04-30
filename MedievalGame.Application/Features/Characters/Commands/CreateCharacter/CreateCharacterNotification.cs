using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Commands.CreateCharacter
{
    public record CreateCharacterNotification(CharacterDto characterDto) : INotification
    {
    }
}
