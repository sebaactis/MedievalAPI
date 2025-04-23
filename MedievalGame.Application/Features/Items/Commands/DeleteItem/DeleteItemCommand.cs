using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Commands.DeleteItem
{
    public record DeleteItemCommand(Guid Id) : IRequest<ItemDto>
    {
    }
}
