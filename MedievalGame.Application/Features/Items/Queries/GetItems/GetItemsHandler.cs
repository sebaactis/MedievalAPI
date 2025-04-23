using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Entities;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Queries.GetItems
{
    public class GetItemsHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<GetItemsQuery, List<ItemDto>>
    {
        public async Task<List<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await repository.GetAllAsync() ?? new List<Item>();

            return mapper.Map<List<ItemDto>>(items);
        }
    }
}

