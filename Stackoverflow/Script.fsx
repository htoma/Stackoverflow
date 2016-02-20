open System.IO
open System.Text.RegularExpressions
open System
open System.Diagnostics

let readLines(path:string) = seq {
    use stream = new StreamReader(path)
    while not stream.EndOfStream do
        yield stream.ReadLine()
}

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
                           Tags = extractTags t.Groups.[3].Value
                    }
                )

let lines = readLines @"C:\data\stackoverflow.com-Posts\posts.xml"
                |> Seq.take 1000
                |> Seq.map (fun v -> extractQuestion v)
                |> Seq.concat
                |> List.ofSeq
