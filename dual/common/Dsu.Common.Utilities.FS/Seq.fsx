open System.IO



seq {1..10}
seq {1..5..100}

seq { for i in 0 .. 10 -> (i, i * i) }

let checkerboardCoordinates n =
    seq {
        for row in 1 .. n do
            for col in 1 .. n do
                let sum = row + col
                if sum % 2 = 0 then
                    yield (row, col)}

checkerboardCoordinates 3


let fileInfo dir =
    seq {
        for file in Directory.GetFiles dir do
            let creationTime = File.GetCreationTime file
            let lastAccessTime = File.GetLastAccessTime file
            yield (file, creationTime.ToString(), lastAccessTime.ToString())}

fileInfo "C:\\"

