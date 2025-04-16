
using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacter;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Characters.Queries.GetCharacterById
{
    public class GetCharacterByIdHandler(ICharacterRepository repository, IMapper mapper) : IRequestHandler<GetCharacterByIdQuery, CharacterDto>
    {
        public async Task<CharacterDto> Handle(GetCharacterByIdQuery request, CancellationToken cancellationToken)
        {
            var character = await repository.GetByIdAsync(request.Id);
            if (character == null)
            {
                throw new NotFoundException($"Character with ID {request.Id} not found.");
            }
            return mapper.Map<CharacterDto>(character);
        }
    }
}
