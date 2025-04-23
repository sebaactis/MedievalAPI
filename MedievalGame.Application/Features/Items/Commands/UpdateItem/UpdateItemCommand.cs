using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Commands.UpdateItem
{
    public record UpdateItemCommand(
            Guid Id,
            string? Name,
            int? Value,
            Guid? RarityId,
            Guid? ItemTypeId
        ) : IRequest<ItemDto>;
}
