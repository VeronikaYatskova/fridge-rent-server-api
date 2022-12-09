using AutoMapper;
using Fridge.Data.Models;
using Fridge.Models.Responses;

namespace Fridge.Utils.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Model, FridgeModelModel>();
            CreateMap<Producer, FridgeProducerModel>();
            CreateMap<Product, ProductModel>();
        }
    }
}
