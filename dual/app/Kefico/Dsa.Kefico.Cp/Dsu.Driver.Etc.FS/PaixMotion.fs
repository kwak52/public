namespace Dsu.Driver.Paix



/// 하나의 축에 대한 정보
type AxisSpec(startSpeed, acceleration, deceleration, driveSpeed) =
    member val StartSpeed: int64 = startSpeed with get, set
    member val Acceleraion: int64 = acceleration with get, set
    member val Deceleraion: int64 = deceleration with get, set
    member val DriveSpeed: int64 = driveSpeed with get, set
    member x.Duplicate() = new AxisSpec(x.StartSpeed, x.Acceleraion, x.Deceleraion, x.DriveSpeed)

/// 축집합에 대한 정보
type SpeedSpec(axesSpec:seq<AxisSpec>) =
    member val AxesSpec = axesSpec |> Array.ofSeq with get
    member x.Duplicate() =
        let ccs = x.AxesSpec |> Array.map(fun s -> s.Duplicate())
        new SpeedSpec(ccs)

type Pose4(v0, v1, v2, v3, group, comment, checked_, speedSpec) = 
    member val Checked: bool = checked_ with get, set
    /// X
    member val V0: int64 = v0 with get, set
    /// Y
    member val V1: int64 = v1 with get, set
    /// Z
    member val V2: int64 = v2 with get, set
    /// Tilt
    member val V3: int64 = v3 with get, set
    member val Group: string = group with get, set
    member val Comment: string = comment with get, set
    [<System.ComponentModel.Browsable(false)>]
    member x.Data with get() = [| x.V0; x.V1; x.V2; x.V3 |]
    [<System.ComponentModel.Browsable(false)>]
    member val SpeedSpec:SpeedSpec = speedSpec with get, set
    override x.ToString() = sprintf "G=%s, C=%s, %d, %d, %d, %d" x.Group x.Comment x.V0 x.V1 x.V2 x.V3
