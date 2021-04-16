namespace ParserAPI.Parsers

open ParserAPI.Models
open ParserAPI.HelperTryParse
open FSharp.Data
open FSharp.Collections.ParallelSeq
open System.Collections.Concurrent
open System.Threading.Tasks

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
        let volume = value.[0..3].Replace('.', ',')
        TryParseDoubleOption(volume)

    let private parseDescriptionCar(node: HtmlNode) company model = 
        let year = parseInt (defaultParse node yearClass)
        let price = parseInt (defaultParse node priceClass)
        let mileage = parseMileage (defaultParse node mileageClass)
        let link = parseLink node linkClass
        let power, volume, transmission = 
            match (parseTransmissionPowerVolume node) with
            | Some (x, y) -> parsePower x, parseVolume x, Some (parseTransmission y)
            | None -> None, None, None

        match (year, price, mileage, transmission, power, volume, link) with 
        | (Some y, Some pr, Some m, Some t, Some po, Some v, Some l) -> Some {Company = company; Model = model; Mileage = m; 
                                                                              EnginePower = po; EngineVolume = v; Year = y; 
                                                                              Transmission = t; Price = pr; Link = l}
        | _ -> None

    let private parseCarDoc company model (doc: HtmlDocument) = 
        doc.Descendants()
        |> Seq.filter (fun x -> x.HasClass(descriptionClass))
        |> Seq.map (fun x -> parseDescriptionCar x company model)

    let private getCountPage (doc: HtmlDocument) = 
        let maybeCountPage = doc.Descendants()
                            |> Seq.filter (fun x -> x.HasClass(pageClass))
                            |> Seq.tryLast 
                            |> Option.map (fun y -> y.InnerText())
        match maybeCountPage with
        | Some count -> TryParseIntOption count
        | None -> None
    
    let loadDocument (url: string) = 
        try 
            let watch = System.Diagnostics.Stopwatch.StartNew()
            let doc = HtmlDocument.Load(url)
            watch.Stop()
            printfn "%f" watch.Elapsed.TotalSeconds
            Some doc 
        with _ -> printfn "%s" url 
                  None

    let Parse company model = 
        let url = $"https://auto.ru/cars/{company}/{model}/all/"
        let doc = loadDocument(url)
        let count = match doc with 
                    | Some d -> getCountPage d 
                    | None -> None 
        printfn "%A" count
        let pages = match count with
                    | Some count -> seq {for i in 2 .. count -> url + $"?page={i}"}
                    | None -> Seq.empty
              
        pages
        |> PSeq.map loadDocument
        |> PSeq.filter (fun x -> x.IsSome)
        |> PSeq.map (fun x -> x.Value)
        |> PSeq.collect (fun x -> parseCarDoc company model x)
        |> PSeq.filter (fun x -> x.IsSome)
        |> PSeq.map (fun x -> x.Value)