namespace ParserAPI.Controllers

open Microsoft.AspNetCore.Mvc
open ParserAPI.Parsers
open MiddlewareLibrary.Models

[<ApiController>]
[<Route("api/[controller]")>]
type ParserController() = 
    inherit ControllerBase()
    
    [<HttpGet>]
    member _.Get() = JsonResult("Hi, How Are You")

    [<HttpPost>]
    member _.Post(car: CarFormModel) : JsonResult = 
        let cars = AutoParser.Parse car 

        JsonResult(cars)
