using AutoMapper;
using CarBestDealsAPI.Domains;
using CarBestDealsAPI.Models;
using MiddlewareLibrary.Models;

namespace CarBestDealsAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarBestDealFormModel, Car>();
            CreateMap<Car, CarBestDealDataModel>();
            CreateMap<CarFormModel, CarHistoryModel>();
        }
    }
}
