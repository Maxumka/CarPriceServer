using CarBestDealsAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using CarBestDealsAPI.Domains;
using MiddlewareLibrary.Models;

namespace CarBestDealsAPI.Services
{
    public interface IParserService
    {
        public Task<IEnumerable<Car>> GetCars(CarFormModel car);
    }
}
