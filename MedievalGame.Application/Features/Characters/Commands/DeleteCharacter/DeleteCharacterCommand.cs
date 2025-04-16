using MediatR;
using MedievalGame.Application.Features.Characters.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedievalGame.Application.Features.Characters.Commands.DeleteCharacter
{
    public record DeleteCharacterCommand(Guid Id) : IRequest<CharacterDto>
    {
    }
}
