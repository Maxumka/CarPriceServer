using System;
using System.Linq;
using CarBestDealsAPI.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarBestDealsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using MiddlewareLibrary;
using CarBestDealsAPI.Domains;
using AutoMapper;
using CSharpPredictorML.Model;
using System.Text.Json;
using MiddlewareLibrary.Models;

namespace CarBestDealsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBestDealsController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        private readonly IParserService _parserService;

        private readonly IMapper _mapper;

        public CarBestDealsController(IHistoryService historyService, IParserService parserService, IMapper mapper)
        {
            _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));

            _parserService = parserService ?? throw new ArgumentNullException(nameof(parserService));

            _mapper = mapper;
        }

        private double? MulOneHundred(double? x) => x.HasValue ? x * 1000 : x;

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> GetCarBestDeals(CarFormModel carModel)
        {
            if (carModel is null) return new(new Either<Car[], Error>(null, Errors.CarWasNull));

            // Это костыль, может быть когда-нить я его исправлю, не думаю 
            carModel.FromEngineVolume = MulOneHundred(carModel.FromEngineVolume);
            carModel.ToEngineVolume = MulOneHundred(carModel.ToEngineVolume);
            
            var userLogin = HttpContext.User.Identity.Name;

            var cars = await _parserService.GetCars(carModel);

            var carsJson = JsonSerializer.Serialize(cars);

            var carsBestDealDataModel = Predictor.GetBestDeals(carsJson);
            var finalCars = JsonSerializer.Deserialize<CarBestDealDataModel[]>(carsBestDealDataModel);

            var historyModel = _mapper.Map<CarHistoryModel>(carModel);
            historyModel.UserLogin = userLogin;
            historyModel.Action = "Получить лучшие объявления";

            await _historyService.AddCarHistoryDbAsync(historyModel);

            return new(new Either<CarBestDealDataModel[], Error>(finalCars, null));
        }
    }
}
