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

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> GetCarBestDeals(CarBestDealFormModel carModel)
        {
            if (carModel is null) return new(new Either<Car[], Error>(null, Errors.CarWasNull));

            var userLogin = HttpContext.User.Identity.Name;

            var car = _mapper.Map<Car>(carModel);

            var cars = await _parserService.GetCars(car);
            var carsJson = JsonSerializer.Serialize(cars);

            var carsBestDealDataModel = Predictor.GetBestDeals(carsJson);
            var finalCars = JsonSerializer.Deserialize<CarBestDealDataModel[]>(carsBestDealDataModel);

           var historyModel = _mapper.Map<CarHistoryModel>(car);
            historyModel.UserLogin = userLogin;

            await _historyService.AddCarHistoryDbAsync(historyModel);

            return new(new Either<CarBestDealDataModel[], Error>(finalCars, null));
        }
    }
}
