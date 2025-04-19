
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Queries.GetCharacters
{
    public record GetCharacterQuery :  IRequest<List<CharacterDto>>
    {
    }
}
