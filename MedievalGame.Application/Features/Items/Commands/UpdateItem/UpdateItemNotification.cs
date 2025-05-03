
using MediatR;
using MedievalGame.Application.Features.Items.Dtos;

namespace MedievalGame.Application.Features.Items.Commands.UpdateItem
{
    public record UpdateItemNotification(ItemDto itemDto) : INotification
    {
    }
}
