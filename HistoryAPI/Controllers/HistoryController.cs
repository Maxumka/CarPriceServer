using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HistoryAPI.Domain.Entities;
using HistoryAPI.Domain.Interfaces;
using HistoryAPI.Repository.Contexts;
using Microsoft.AspNetCore.Authorization;
using HistoryAPI.Repository.Implementations;
using System.Linq;
using MiddlewareLibrary;
using AutoMapper;
using MiddlewareLibrary.Models;

namespace HistoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IRepository<CarHistory> _repository;

        private readonly IMapper _mapper;

        public HistoryController(HistoryContext historyContext, IMapper mapper)
        {
            _repository = new DBRepository<CarHistory>(historyContext);

            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public JsonResult GetHistory()
        {
            var userLogin = HttpContext.User.Identity.Name;

            var carHistoriesModel = _repository.FindWhere(c => c.UserLogin == userLogin)
                                               .Select(c => _mapper.Map<CarHistoryModel>(c))
                                               .ToArray();

            return new(new Either<CarHistoryModel[], Error>(carHistoriesModel, null));
        }

        [HttpPost]
        public async Task<JsonResult> AddCarDb(CarHistoryModel carHistoryModel)
        {
            if (carHistoryModel is null) return new(new Either<bool, Error>(false, Errors.CarWasNull));

            var car = _mapper.Map<CarHistory>(carHistoryModel);

            _repository.Add(car);

            await _repository.SaveAsync();

            return new(new Either<bool, Error>(true, null));
        }
    }
}
