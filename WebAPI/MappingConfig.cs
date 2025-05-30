using AutoMapper;
using WebAPI.Models;
using WebAPI.Models.Dto;

namespace WebAPI
{
    public class MappingConfig  : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumVilla, NumVillaUpdateDto>().ReverseMap();
            CreateMap<NumVilla, NumVillaCreateDto>().ReverseMap();
            CreateMap<NumVilla, NumVillaDto>().ReverseMap();

        }
    }
}
