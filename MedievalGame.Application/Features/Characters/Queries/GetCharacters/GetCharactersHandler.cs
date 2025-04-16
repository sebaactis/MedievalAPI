using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;
using AutoMapper;

namespace MedievalGame.Application.Features.Characters.Queries.GetCharacters
{
    public class GetCharactersHandler(ICharacterRepository repository, IMapper mapper) : IRequestHandler<GetCharacterQuery, List<CharacterDto>>
    {
        public async Task<List<CharacterDto>> Handle(GetCharacterQuery query, CancellationToken cn) {

            var characters = await repository.GetAllAsync() ?? new List<Character>();

            return mapper.Map<List<CharacterDto>>(characters);
        }
    }
}
