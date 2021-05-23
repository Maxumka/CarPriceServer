using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarMarkTypeAPI.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MiddlewareLibrary.Models;
using CarMarkTypeAPI.Database.Entity;
using Microsoft.EntityFrameworkCore;
using MiddlewareLibrary;

namespace CarMarkTypeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarMarkTypeController : ControllerBase
    {
        private readonly CarMarkTypeContext _context;

        private readonly ParserService _parserServie;

        public CarMarkTypeController(CarMarkTypeContext context, ParserService parserService)
        {
            _context = context;

            _parserServie = parserService;
        }

        [HttpGet]
        public JsonResult GetCarMarkType()
        {
            // var carsModel = await _parserServie.GetCars();

            // foreach (var carModel in carsModel)
            // {
            //     var car = new CarMark
            //     {
            //         Name = carModel.Name,
            //         CarTypes = carModel.TypeModels.Select(c => new CarType {Name = c.Name, Link = c.Link}).ToList(),
            //     };

            //     _context.CarMarks.Add(car);
            // }

            // _context.SaveChanges();

            var carsMark = _context.CarMarks.Include(c => c.CarTypes).ToList().Select(c => new CarMarkModel 
            {
                Name = c.Name,
                TypeModels = c.CarTypes.Select(c => new CarTypeModel {Name = c.Name, Link = c.Link}).ToList()
            }).ToList();

            return new(new Either<List<CarMarkModel>, Error>(carsMark, null));
        }
    }
}
