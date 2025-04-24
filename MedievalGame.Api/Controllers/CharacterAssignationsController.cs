using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.AssignItem;
using MedievalGame.Application.Features.Characters.Commands.AssignWeapon;
using MedievalGame.Application.Features.Characters.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MedievalGame.Api.Controllers
{
    [Route("api/characters/v1/assignment")]
    [ApiController]
    public class CharacterAssignationsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("{characterId}/assign-item/{itemId}")]
        public async Task<ActionResult<ApiResponse<CharacterDto>>> AssignItemToCharacter(Guid characterId, Guid itemId)
        {
            var result = await mediator.Send(new AssignItemToCharacterCommand(characterId, itemId));
            return ApiResponse<CharacterDto>.SuccessResponse(
                result,
                "Item assigned to character successfully",
                StatusCodes.Status200OK
            );
        }

        [HttpPost("{characterId}/assign-weapon/{weaponId}")]
        public async Task<ActionResult<ApiResponse<CharacterDto>>> AssignWeaponToCharacter(Guid characterId, Guid weaponId)
        {
            var result = await mediator.Send(new AssignWeaponToCharacterCommand(characterId, weaponId));
            return ApiResponse<CharacterDto>.SuccessResponse(
                result,
                "Weapon assigned to character successfully",
                StatusCodes.Status200OK
            );
        }
    }
}
