using AutoMapper;
using Fridge.Models.DTOs;
using Models.Models.DTOs;

namespace Fridge.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Fridge, FridgeServiceDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<FridgeProduct, FridgeProductDto>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
