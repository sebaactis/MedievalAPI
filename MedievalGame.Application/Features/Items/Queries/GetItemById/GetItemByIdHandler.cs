using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Exceptions;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Queries.GetItem
{
    public class GetItemByIdHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<GetItemByIdQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await repository.GetByIdAsync(request.Id);

            if (item == null)
            {
                throw new NotFoundException($"Weapon with ID {request.Id}");
            }

            return mapper.Map<ItemDto>(item);
        }
    }

}

