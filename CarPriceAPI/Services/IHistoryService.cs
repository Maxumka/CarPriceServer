using CarPriceAPI.Models;
using MiddlewareLibrary.Models;
using System.Threading.Tasks;

namespace CarPriceAPI.Services
{
    public interface IHistoryService
    {
        Task AddCarHistoryDbAsync(CarHistoryModel carHistoryModel);
    }
}
