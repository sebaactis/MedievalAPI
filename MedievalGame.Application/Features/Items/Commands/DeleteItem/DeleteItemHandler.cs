using AutoMapper;
using MediatR;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Domain.Interfaces;

namespace MedievalGame.Application.Features.Items.Commands.DeleteItem
{
    public class DeleteItemHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<DeleteItemCommand, ItemDto>
    {
        public async Task<ItemDto> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var item = await repository.GetByIdAsync(request.Id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {request.Id} not found.");
            }
            var deletedItem = await repository.DeleteAsync(request.Id);
            return mapper.Map<ItemDto>(deletedItem);
        }
    }
}
