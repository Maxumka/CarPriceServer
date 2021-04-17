using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarBestDealsAPI.Models;
using System.Collections.Generic;
using CarBestDealsAPI.Domains;
using MiddlewareLibrary.Models;

namespace CarBestDealsAPI.Services
{
    public class ParserService : IParserService
    {
        private readonly HttpClient _client;

        public ParserService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Car>> GetCars(CarFormModel car)
        {
            IEnumerable<Car> cars = null;

            var response = await _client.PostAsJsonAsync("api/parser", car);

            if(response.IsSuccessStatusCode)
            {
                cars = await response.Content.ReadFromJsonAsync<IEnumerable<Car>>();
            }

            return cars;
        }
    }
}
