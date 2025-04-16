using MediatR;
using MedievalGame.Api.Responses;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Characters.Queries.GetCharacters;
using Microsoft.AspNetCore.Mvc;

[Route("api/characters/v1")]
[ApiController]
public class CharactersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharactersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CharacterDto>>>> GetCharacters()
    {
        var characters = await _mediator.Send(new GetCharacterQuery());
        return ApiResponse<List<CharacterDto>>.SuccessResponse(
            characters,
            "Characters retrieved successfully",
            StatusCodes.Status200OK
        );
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateCharacter(
        [FromBody] CreateCharacterCommand command)
    {
        var characterId = await _mediator.Send(command);
        return ApiResponse<Guid>.SuccessResponse(
            characterId,
            "Character created successfully",
            StatusCodes.Status201Created
        );
    }
}