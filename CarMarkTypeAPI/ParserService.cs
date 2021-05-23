using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CarMarkTypeAPI.Database.Entity;
using MiddlewareLibrary.Models;
using System.Threading;

namespace CarMarkTypeAPI
{
    public class ParserService 
    {
        private readonly HttpClient _client;

        public ParserService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.Timeout = Timeout.InfiniteTimeSpan;;
        }

        public async Task<IEnumerable<CarMarkModel>> GetCars()
        {
            List<CarMarkModel> cars = null;

            cars = await _client.GetFromJsonAsync<List<CarMarkModel>>("api/parser");

            // if(response.IsSuccessStatusCode)
            // {
            //     cars = await response.Content.ReadFromJsonAsync<IEnumerable<CarMarkModel>>();
            // }

            return cars;
        }
    }
}