using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Commands.CreateItem
{
    public record CreateItemNotification(ItemDto itemDto) : INotification
    {
    }
}
