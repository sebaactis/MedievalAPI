using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Commands.CreateItem
{
    public record CreateItemCommand(
            string Name,
            int Value,
            Guid RarityId,
            Guid ItemTypeId
        ) : IRequest<ItemDto>;
}
