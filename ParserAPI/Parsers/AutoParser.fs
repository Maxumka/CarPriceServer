namespace ParserAPI.Parsers

open System.Text
open ParserAPI.Models
open ParserAPI.HelperTryParse
open ParserAPI.StringBuilderHelpers
open ParserAPI.OptionHelpers
open FSharp.Data
open FSharp.Collections.ParallelSeq
open MiddlewareLibrary.Models

module AutoParser =
    [<Literal>]
    let private sampleUrl = "https://auto.ru/cars/lifan/x70/all/"
    let private pageClass = "Button Button_color_whiteHoverBlue Button_size_s Button_type_link Button_width_default ListingPagination-module__page"
    let private descriptionClass = "ListingItem-module__description"
    let private yearClass = "ListingItem-module__year"
    let private priceClass = "ListingItemPrice-module__content"
    let private mileageClass = "ListingItem-module__kmAge"
    let private transmissionPowerVolumeClass = "ListingItemTechSummaryDesktop__cell"
    let private linkClass = "Link ListingItemTitle-module__link"

    let private paramFromMileage x = $"&km_age_from={x}"
    let private paramToMileage x = $"&km_age_to={x}"

    let private paramFromPower x = $"&power_from={x}"
    let private paramToPower x = $"&power_to={x}"

    let private paramFromVolume x = $"&displacement_from={x}"
    let private paramToVolume x = $"&displacement_to={x}"

    let private paramFromYear x = $"&year_from={x}"
    let private paramToYear x = $"&year_to={x}"

    let private paramFromPrice x = $"&price_from{x}"
    let private paramToPrice x = $"&price_to{x}"

    let private paramAutomatTransmission = "&transmission=AUTO&transmission=AUTOMATIC&transmission=ROBOT&transmission=VARIATOR"
    let private paramMechanicTranmission = "&transmission=MECHANICAL"

    let private paramGasolineEngine = "&engine_group=GASOLINE"
    let private paramDieselEngine = "&engine_group=DIESEL"
    let private paramHybridEngine = "&engine_group=HYBRID"
    let private paramElectroEngine = "&engine_group=ELECTRO"

    let private paramForwardGear = "&gear_type=FORWARD_CONTROL"
    let private paramBackGear = "&gear_type=REAR_DRIVE"
    let private paramAllGear = "&gear_type=ALL_WHEEL_DRIVE"

    let private paramRightSteeringWheel = "&steering_wheel=RIGHT"
    let private paramLeftSteeringWheel = "&steering_wheel=LEFT"
 
    let private paramSedanCarBody = "&body_type_group=SEDAN"
    let private paramHatchbackCarBody = "&body_type_group=HATCHBACK&body_type_group=HATCHBACK_3_DOORS&body_type_group=HATCHBACK_5_DOORS&body_type_group=LIFTBACK"
    let private paramCoupeCarBody = "&body_type_group=COUPE"
    let private paramPickupCarBody = "&body_type_group=PICKUP"
    let private paramConvertibleCarBody = "&body_type_group=CABRIO"


    let private defaultParse(node: HtmlNode) className = 
        node.Descendants()
        |> Seq.filter (fun x -> x.HasClass(className))
        |> Seq.tryHead
        |> Option.map (fun x -> x.InnerText())

    let private parseLink(node: HtmlNode) className = 
        node.Descendants() 
        |> Seq.filter (fun x -> x.HasClass(className))
        |> Seq.choose(fun x -> x.TryGetAttribute("href"))
        |> Seq.tryHead
        |> Option.map (fun x -> x.Value())

    let private parseTransmissionPowerVolume(node: HtmlNode) = 
        node.Descendants()
        |> Seq.filter (fun x -> x.HasClass(transmissionPowerVolumeClass))
        |> Seq.pairwise 
        |> Seq.tryHead
        |> Option.map (fun (x, y) -> x.InnerText(), y.InnerText())

    let private parseInt (value: string option) = 
        match value with
        | Some v -> v  
                    |> String.filter (fun x -> x >= '0' && x <= '9')
                    |> TryParseIntOption
        | None -> None
    
    let private parseMileage (value: string option) = 
        match value with 
        | Some v -> match v with 
                    | "Новый" -> Some 0
                    | _ -> parseInt (Some v)
        | None -> None 

    let private parseTransmission (value: string) = if value = "механика" then false else true

    let private parsePower(value: string) = 
        let power = value.[8..10]
        TryParseIntOption(power)

    let private parseVolume (value: string) = 
        let volume = value.[0..3]
        TryParseDoubleOption(volume)

    let private parseDescriptionCar (car : CarFormModel) (node: HtmlNode) = 
        let year = parseInt (defaultParse node yearClass)
        let price = parseInt (defaultParse node priceClass)
        let mileage = parseMileage (defaultParse node mileageClass)
        let link = parseLink node linkClass
        let power, volume, transmission = 
            match (parseTransmissionPowerVolume node) with
            | Some (x, y) -> parsePower x, parseVolume x, Some (parseTransmission y)
            | None -> None, None, None
        let car = match (year, price, mileage, transmission, power, volume, link) with 
                  | (Some y, Some pr, Some m, Some t, Some po, Some v, Some l) -> Some {Company = car.Company; Model = car.Model; Mileage = m; 
                                                                                        EnginePower = po; EngineVolume = v; Year = y; 
                                                                                        Transmission = t; Price = pr; Link = l}
                  | _ -> None
        car 

    let private parseCarDoc (car : CarFormModel) (doc: HtmlDocument) = 
        doc.Descendants()
        |> Seq.filter (fun x -> x.HasClass(descriptionClass))
        |> Seq.map (fun x -> parseDescriptionCar car x)

    let private getCountPage (doc: HtmlDocument) = 
        let maybeCountPage = doc.Descendants()
                            |> Seq.filter (fun x -> x.HasClass(pageClass))
                            |> Seq.tryLast 
                            |> Option.map (fun y -> y.InnerText())
        match maybeCountPage with
        | Some count -> TryParseIntOption count
        | None -> None
    
    let private loadDocument (url: string) = 
        try 
            let watch = System.Diagnostics.Stopwatch.StartNew()
            let doc = HtmlDocument.Load(url)
            watch.Stop()
            printfn "%s" $"time load document: {watch.Elapsed.TotalSeconds}"
            Some doc 
        with _ -> printfn "%s" url 
                  None

    let private buildUrl (urlBuilder : StringBuilder) (cond : Conditions) = 
        urlBuilder 
        ++ execFun paramFromMileage cond.fromMileage
        ++ execFun paramToMileage cond.toMileage
        ++ execFun paramFromPower cond.fromPower
        ++ execFun paramToPower cond.toPower
        ++ execFun paramFromVolume cond.fromVolume
        ++ execFun paramToPower cond.toVolume
        ++ execFun paramFromYear cond.fromYear
        ++ execFun paramToYear cond.toYear 
        ++ execFun paramFromPrice cond.fromPrice
        ++ execFun paramToPrice cond.toPrice 
        ++ match cond.tranmission with 
           | Transmission.Automatic -> Some paramAutomatTransmission
           | Transmission.Mechanic  -> Some paramMechanicTranmission
           | _                      -> None
        ++ match cond.engine with 
           | Engine.Gasoline -> Some paramGasolineEngine
           | Engine.Diesel   -> Some paramDieselEngine
           | Engine.Hybrid   -> Some paramHybridEngine
           | Engine.Electric -> Some paramElectroEngine
           | _               -> None
        ++ match cond.gear with 
           | Gear.Forward -> Some paramForwardGear
           | Gear.Back    -> Some paramBackGear
           | Gear.All     -> Some paramAllGear
           | _            -> None
        ++ match cond.steeringWheel with 
           | SteeringWheel.Right -> Some paramRightSteeringWheel
           | SteeringWheel.Left  -> Some paramLeftSteeringWheel
           | _                   -> None
        ++ match cond.carBody with 
           | CarBody.Sedan       -> Some paramSedanCarBody
           | CarBody.Hatchback   -> Some paramHatchbackCarBody
           | CarBody.Coupe       -> Some paramCoupeCarBody
           | CarBody.Pickup      -> Some paramPickupCarBody
           | CarBody.Convertible -> Some paramConvertibleCarBody
           | _                   -> None
        |> out

    let private buildLink (car : CarFormModel) = 
        let urlBuilder = new StringBuilder() 
        urlBuilder += Some $"https://auto.ru/cars/{car.Company}/{car.Model}/all/?"
        let conditions = Conditions.FromCarFromModel car      
        let url = buildUrl urlBuilder conditions
        printfn "%s" url 
        url 

    let Parse car = 
        let url = buildLink car 
        let doc = loadDocument url 
        let count = match doc with 
                    | Some d -> getCountPage d 
                    | None -> None 
        printfn "%A" count
        let pages = match count with
                    | Some count -> seq {for i in 1 .. count -> url + $"?page={i}"}
                    | None -> seq {url}
        
        pages
        |> PSeq.map loadDocument
        |> PSeq.filter (fun x -> x.IsSome)
        |> PSeq.map (fun x -> x.Value)
        |> PSeq.collect (fun x -> parseCarDoc car x)
        |> PSeq.filter (fun x -> x.IsSome)
        |> PSeq.map (fun x -> x.Value)