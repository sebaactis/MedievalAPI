using AutoMapper;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Dtos;
using MedievalGame.Domain.Entities;

namespace MedievalGame.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Character, CharacterDto>()
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.CharacterClass.Name))
                .ForMember(dest => dest.Weapons, opt => opt.MapFrom(src => src.Weapons))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<Weapon, WeaponDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.WeaponType.Name));

            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ItemType.Name))
                .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Rarity.Name));

            CreateMap<UpdateCharacterCommand, Character>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null));
        }
    }
}
