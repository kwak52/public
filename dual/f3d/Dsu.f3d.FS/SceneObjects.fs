namespace Dsu.f3d.FS


open System.Collections.Concurrent
open System.Collections.Generic

type Node() =
    member val Parent:NodeStem option = None with get, set
    member val UserTag:obj option = None with get, set
and NodeStem() =
    inherit Node()
    let childrenMap = new ConcurrentDictionary<string, Node>();
