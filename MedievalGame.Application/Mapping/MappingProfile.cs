using AutoMapper;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;

namespace MedievalGame.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Character, CharacterDto>()
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class.ToString()))
                .ForMember(dest => dest.Weapons, opt => opt.MapFrom(src => src.Weapons));

            CreateMap<Weapon, WeaponDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
