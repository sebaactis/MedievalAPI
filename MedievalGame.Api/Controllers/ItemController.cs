using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Items.Commands.CreateItem;
using MedievalGame.Application.Features.Items.Commands.DeleteItem;
using MedievalGame.Application.Features.Items.Commands.UpdateItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Items.Queries.GetItem;
using MedievalGame.Application.Features.Items.Queries.GetItems;
using Microsoft.AspNetCore.Mvc;

namespace MedievalGame.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ItemDto>>> CreateItem(CreateItemCommand command)
        {
            var item = await mediator.Send(command);
            return ApiResponse<ItemDto>.SuccessResponse(
            item,
            "Item created successfully",
                StatusCodes.Status201Created
            );
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ItemDto>>>> GetItems()
        {
            var items = await mediator.Send(new GetItemsQuery());
            return ApiResponse<List<ItemDto>>.SuccessResponse(
                items,
                "Items retrieved successfully",
                StatusCodes.Status200OK
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> GetItemById(Guid id)
        {
            var weapon = await mediator.Send(new GetItemByIdQuery(id));
            return ApiResponse<ItemDto>.SuccessResponse(
                weapon,
                "Item found successfully",
                StatusCodes.Status200OK
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> UpdateWeapon(Guid id,
            [FromBody] UpdateItemCommand command)
        {
            if (id != command.Id)
                return ApiResponse<ItemDto>.ErrorResponse("ID mismatch", StatusCodes.Status400BadRequest);

            var itemUpdate = await mediator.Send(command);
            return ApiResponse<ItemDto>.SuccessResponse(
                    itemUpdate,
                    "Item updated successfully",
                    StatusCodes.Status200OK
                );
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<ItemDto>>> DeleteItem(Guid id)
        {
            var itemDeleted = await mediator.Send(new DeleteItemCommand(id));
            return ApiResponse<ItemDto>.SuccessResponse(
                itemDeleted,
                "Item deleted successfully",
                StatusCodes.Status200OK
            );
        }
    }
}
