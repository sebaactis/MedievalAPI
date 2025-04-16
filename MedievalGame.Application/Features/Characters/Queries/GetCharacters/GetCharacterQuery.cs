
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;

namespace MedievalGame.Application.Features.Characters.Queries.GetCharacters
{
    public class GetCharacterQuery :  IRequest<List<CharacterDto>>
    {
    }
}
