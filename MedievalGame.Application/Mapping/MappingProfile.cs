using AutoMapper;
using MedievalGame.Application.Features.Characters.Commands.UpdateCharacter;
using MedievalGame.Application.Features.Characters.Dtos;
using MedievalGame.Application.Features.Items.Commands.UpdateItem;
using MedievalGame.Application.Features.Items.Dtos;
using MedievalGame.Application.Features.Weapons.Commands.UpdateWeapon;
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
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CharacterItems));

            CreateMap<CharacterItem, CharacterItemDto>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Item.Value))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Item.ItemType.Name))
                .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Item.Rarity.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<Weapon, WeaponDto>()
                .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Rarity.Name))
                .ForMember(dest => dest.WeaponType, opt => opt.MapFrom(src => src.WeaponType.Name));

            CreateMap<Item, ItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ItemType.Name))
                .ForMember(dest => dest.Rarity, opt => opt.MapFrom(src => src.Rarity.Name));

            CreateMap<UpdateCharacterCommand, Character>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null));

            CreateMap<UpdateWeaponCommand, Weapon>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null));

            CreateMap<UpdateItemCommand, Item>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null));
        }
    }
}
