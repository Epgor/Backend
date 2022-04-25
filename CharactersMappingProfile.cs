using AutoMapper;
using Back.Entities;
using Back.Models;
namespace Back
{
    public class CharactersMappingProfile : Profile
    {
        public CharactersMappingProfile()
        {
            CreateMap<CreateCharacterDto, Character>();
        }
    }
}
