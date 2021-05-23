namespace ParserAPI.Controllers

open Microsoft.AspNetCore.Mvc
open ParserAPI.Parsers
open MiddlewareLibrary.Models

[<ApiController>]
[<Route("api/[controller]")>]
type ParserController() = 
    inherit ControllerBase()
    
    [<HttpGet>]
    member _.Get() : JsonResult = 
        let cars = CarMarkTypeParser.Parse
        JsonResult(cars)

    [<HttpPost>]
    member _.Post(car: CarFormModel) : JsonResult = 
        let cars = AutoParser.Parse car 
        JsonResult(cars)
