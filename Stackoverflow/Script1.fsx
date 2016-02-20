open System
open System.IO
open System.Text.RegularExpressions

type Question = {
    CreationDate:DateTime
    Score:int
    Tags:string[]
}

let extractTags (text:string) = 
    let decoded = text.Replace("&lt;","").Replace("&gt","")
    decoded.Split([|";"|], StringSplitOptions.RemoveEmptyEntries)

let extractQuestion row = 
    Regex.Matches(row, @"CreationDate=""(.*?)"".*Score=""(\d+)"".*Tags=""(.*?)""", RegexOptions.Singleline)
    |> Seq.cast<Match>
    |> Seq.map (fun t -> { CreationDate = DateTime.Parse t.Groups.[1].Value
                           Score = Int32.Parse <| t.Groups.[2].Value
                           Tags = extractTags t.Groups.[1].Value
                    }
                )
    |> Seq.head    
