using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Queries.GetCharacter
{
    public record GetCharacterByIdQuery(Guid Id) : IRequest<CharacterDto>
    {
    }
}
