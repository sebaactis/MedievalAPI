using Azure.Core;
using MediatR;
using MedievalGame.Api.Requests.Characters;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Characters.Commands.DeleteCharacter;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacter;
using MedievalGame.Application.Features.Characters.Queries.GetCharacters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/characters/v1")]
[ApiController]
[Authorize]
public class CharactersController(IMediator mediator) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CharacterDto>>>> GetCharacters()
    {
        var characters = await mediator.Send(new GetCharacterQuery());
        return ApiResponse<List<CharacterDto>>.SuccessResponse(
            characters,
            "Characters retrieved successfully",
            StatusCodes.Status200OK
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CharacterDto>>> GetCharacterById(Guid id)
    {
        var character = await mediator.Send(new GetCharacterByIdQuery(id));

        return ApiResponse<CharacterDto>.SuccessResponse(
            character,
            "Character found successfully",
            StatusCodes.Status200OK
        );
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CharacterDto>>> CreateCharacter(
        [FromBody] CreateCharacterRequest request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
        {
            var error = ApiResponse<string>.ErrorResponse("Invalid or missing user ID in token", 401);
            return Unauthorized(error);
        }

        var command = new CreateCharacterCommand(
        request.Name,
        request.Life,
        request.Attack,
        request.Defense,
        request.Level,
        request.CharacterClassId,
        userId);


        var character = await mediator.Send(command);
        return ApiResponse<CharacterDto>.SuccessResponse(
            character,
            "Character created successfully",
            StatusCodes.Status201Created
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CharacterDto>>> UpdateCharacter(Guid id,
    [FromBody] UpdateCharacterCommand command)
    {
        if (id != command.Id)
            return ApiResponse<CharacterDto>.ErrorResponse("ID mismatch", StatusCodes.Status400BadRequest);

        var characterUpdate = await mediator.Send(command);
        return ApiResponse<CharacterDto>.SuccessResponse(
                characterUpdate,
                "Character updated successfully",
                StatusCodes.Status200OK
            );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<CharacterDto>>> DeleteCharacter(Guid id)
    {
        var characterDeleted = await mediator.Send(new DeleteCharacterCommand(id));
        return ApiResponse<CharacterDto>.SuccessResponse(
            characterDeleted,
            "Character deleted successfully",
            StatusCodes.Status200OK
        );
    }

}