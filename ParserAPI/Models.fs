namespace ParserAPI

open MiddlewareLibrary.Models
open System

module Models = 

    type Conditions = { fromMileage   : int option 
                        toMileage     : int option
                        fromPower     : int option
                        toPower       : int option
                        fromVolume    : double option
                        toVolume      : double option
                        fromYear      : int option
                        toYear        : int option
                        fromPrice     : int option
                        toPrice       : int option
                        tranmission   : Transmission
                        engine        : Engine
                        gear          : Gear 
                        steeringWheel : SteeringWheel
                        carBody       : CarBody}
                        with static member FromCarFromModel (car : CarFormModel) = 
                                    { fromMileage   = Option.ofNullable car.FromMileage
                                      toMileage     = Option.ofNullable car.ToMileage
                                      fromPower     = Option.ofNullable car.FromEnginePower
                                      toPower       = Option.ofNullable car.ToEnginePower
                                      fromVolume    = Option.ofNullable car.FromEngineVolume
                                      toVolume      = Option.ofNullable car.ToEngineVolume
                                      fromYear      = Option.ofNullable car.FromYear
                                      toYear        = Option.ofNullable car.ToYear
                                      fromPrice     = Option.ofNullable car.FromPrice
                                      toPrice       = Option.ofNullable car.ToPrice
                                      tranmission   = car.Transmission
                                      engine        = car.Engine
                                      gear          = car.Gear 
                                      steeringWheel = car.SteeringWheel
                                      carBody       = car.CarBody}
                                
                            

    type CarModel = {Company: string; 
                     Model: string; 
                     Mileage: int; 
                     EnginePower: int; 
                     EngineVolume: double; 
                     Year: int; 
                     Transmission: bool; 
                     Link: string;
                     Price: int}