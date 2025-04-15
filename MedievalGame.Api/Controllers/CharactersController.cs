using MediatR;
using MedievalGame.Application.Features.Characters.Commands.CreateCharacter;
using MedievalGame.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MedievalGame.Api.Controllers
{
    [ApiController]
    [Route("api/characters/v1")]
    public class CharactersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CharactersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterCommand command)
        {
            var characterResponse = await _mediator.Send(command);
            return Ok(characterResponse);
        }
    }
}
