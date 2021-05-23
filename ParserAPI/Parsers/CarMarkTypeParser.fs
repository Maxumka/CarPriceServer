namespace ParserAPI.Parsers 

open canopy.classic
open System.Text.Json
open canopy.configuration
open ParserAPI.Models

module CarMarkTypeParser = 
    [<Literal>]
    let private link = "https://auto.ru/cars/all/"
    let private cssExpandMark = ".ListingPopularMMM-module__expandLink"
    let private cssExpandType = ".ListingPopularMMM-module__expandLink"
    let private cssMark = "html body.react-page.body_controller_listing div#app.__react-app__ div.SusaninReact div.page.page_type_listing div.content div.Layout div.LayoutSidebar div.LayoutSidebar__content div#listing-filters.bZXQfryu7jv90_9HSeqKbWw0AZksfPSpUTeeuNj8 div#popularMMM.ListingPopularMMM-module__container.PageListing-module__popularMMM div.ListingPopularMMM-module__items div.ListingPopularMMM-module__column div.ListingPopularMMM-module__item a.Link.ListingPopularMMM-module__itemName"
    let private cssType = ".ListingPopularMMM-module__itemName"

    let parseTypeCar (nameMark : string) (linkType : string) = 
        url linkType

        match someElement cssExpandType with 
        | Some _ -> click cssExpandType
        | _      -> ()

        let types' = elements cssType |> List.map (fun x -> {Name = x.Text; Link = x.GetAttribute "href"})

        let mark = {Name = nameMark; TypeModels = types'}

        printfn "%A" mark

        mark

    let Parse = 
        chromiumDir <- System.AppContext.BaseDirectory
        start chromium

        url link

        click cssExpandMark 

        let namesAndLinks = elements cssType |> List.map (fun x -> (x.Text, (x.GetAttribute "href")))

        let marks = namesAndLinks |> List.map (fun (n, l) -> parseTypeCar n l)

        marks