using System;
using System.Net.Http;
using CarPriceAPI.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MiddlewareLibrary.Models;

namespace CarPriceAPI.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly HttpClient _client;

        public HistoryService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddCarHistoryDbAsync(CarHistoryModel carHistoryModel)
        {
            await _client.PostAsJsonAsync("api/history", carHistoryModel);
        }
    }
}
