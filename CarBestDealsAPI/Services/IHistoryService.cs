using MiddlewareLibrary.Models;
using System.Threading.Tasks;

namespace CarBestDealsAPI.Services
{
    public interface IHistoryService
    {
        Task AddCarHistoryDbAsync(CarHistoryModel carHistoryModel);
    }
}
