namespace Dsu.f3d.FS

open System.ComponentModel
open System.Runtime.InteropServices



/// OpenGL rendering 에 영향을 줄 수 있는 요소
type IGlApplicable =
    interface
    end

type IPushable =
    inherit IGlApplicable
    abstract PushAttribute: RenderingContext -> unit
    abstract PopAttribute: RenderingContext -> unit

/// OpenGL 직접 drawing 관련 요소
type IGlDrawable =
    inherit IGlApplicable
    abstract GetPushableAttributes: RenderingContext -> IPushable seq

    abstract RenderPrologue: RenderingContext -> obj -> bool
    abstract Render: RenderingContext -> obj -> bool
    abstract RenderEpilogue: RenderingContext -> obj -> bool
    abstract RenderDrawable: RenderingContext -> obj -> bool

type DrawableImpl(container:IGlDrawable) =
    let container = container
    member __.RenderDrawable(context:RenderingContext, sender:obj) = true
    interface IGlDrawable with
        member x.GetPushableAttributes context = container.GetPushableAttributes context
        override x.Render context sender =             
            (container.RenderPrologue context sender) && (container.RenderDrawable context sender) && (container.RenderEpilogue context sender)
        member x.RenderDrawable context sender = true
        member x.RenderEpilogue context sender = 
            failwith "Not implemented yet"
        member x.RenderPrologue context sender =
            failwith "Not implemented yet"
        

type IV3 =
    interface
    end


[<Guid("4B51869B-6E81-43F6-B7F0-4D342988FDA9")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IV3D =
    inherit IV3
    /// x, y, z 좌표값
    [<Browsable(false)>] abstract Cooridnates : float array with get, set

    /// X 좌표값
    [<Category("Coordinates")>] abstract X : float with get, set
    /// Y 좌표값
    [<Category("Coordinates")>] abstract Y : float with get, set
    /// Z 좌표값
    [<Category("Coordinates")>] abstract Z : float with get, set


[<Guid("245FE0E8-CA02-4624-B99F-8AF06336217D")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IV3F =
    inherit IV3
    /// x, y, z 좌표값
    [<Browsable(false)>] abstract Cooridnates : float32 array with get, set

    /// X 좌표값
    [<Category("Coordinates")>] abstract X : float32 with get, set
    /// Y 좌표값
    [<Category("Coordinates")>] abstract Y : float32 with get, set
    /// Z 좌표값
    [<Category("Coordinates")>] abstract Z : float32 with get, set


/// X, y, z, w 요소를 갖는 vertex 의 interface
[<Guid("130937D6-C458-46BD-8F4A-63D522A196F4")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IV4D =
    inherit IV3D
    /// W 좌표값
    [<Category("Coordinates")>] abstract W : float with get, set


/// X, y, z, w 요소를 갖는 vertex 의 interface
[<Guid("554F17F2-6F6D-4598-862C-14F04A3D6912")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IV4F =
    inherit IV3F
    /// W 좌표값
    [<Category("Coordinates")>] abstract W : float32 with get, set



[<Guid("746F57FE-7097-4BDE-91AD-E97B5A2A84B9")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IMatrix =
    inherit IGlApplicable

    abstract M11:float with get, set
    abstract M12:float with get, set
    abstract M13:float with get, set
    abstract M14:float with get, set
    abstract M21:float with get, set
    abstract M22:float with get, set
    abstract M23:float with get, set
    abstract M24:float with get, set
    abstract M31:float with get, set
    abstract M32:float with get, set
    abstract M33:float with get, set
    abstract M34:float with get, set
    abstract M41:float with get, set
    abstract M42:float with get, set
    abstract M43:float with get, set
    abstract M44:float with get, set

    /// X vector of the matrix
    abstract X:IV4D  with get, set
    /// Y vector of the matrix
    abstract Y:IV4D  with get, set
    /// Z vector of the matrix
    abstract Z:IV4D  with get, set
    /// T vector of the matrix
    abstract T:IV4D  with get, set



/// Triangle interface
[<Guid("E4A53A23-B56E-4788-809C-9F7B244341CF")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type ITriangle =
    /// 삼각형의 3 vertices
    abstract Vertices:IV3F array with get, set
    /// 삼각형의 첫 vertex
    abstract V0:IV3F with get, set
    /// 삼각형의 두번째 vertex
    abstract V1:IV3F with get, set
    /// 삼각형의 셋째 vertex
    abstract V2:IV3F with get, set

    /// 삼각형의 Normal
    abstract N:IV3F with get
    /// 삼각형의 3 vertex 의 x,y,z 좌표.  float 의 총갯수는 9
    abstract Coordinates:float32 array with get, set




[<Guid("17951319-6BEC-47E4-AF81-A3C588CC7EB0")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type IMesh =
        /// Mesh 내의 삼각형 갯수
        abstract NumTriangles:int with get
        /// Mesh 내의 vertex 갯수.  일반적으로 3 * NumTriangles
        abstract NumVertices:int with get

        /// Mesh 내의 삼각형
        abstract Triangles: ITriangle array with get
        /// Mesh 내의 vertex
        abstract Vertices:IV3F array with get

        /// Mesh 내의 모든 vertex 들의 x, y, z 좌표 값을 float 로 풀어 놓은 값
        abstract Coordinates: float array with get

        /// level, cooridnates
        abstract AddFromCoordinates: int -> float array -> unit


/// Core interface
[<Guid("23C62D60-5086-4D88-8067-321E1CC923CB")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type ICore =
    interface
    end

/// CoreFace interface
[<Guid("0EEA7C5A-85CA-47BB-AE73-F4A9370E889E")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type ICoreFace =
    inherit ICore
    abstract Mesh:ICoreMesh with get, set
and
 /// CoreMesh interface
 [<Guid("7A4547D8-5EDC-4984-B00A-980595C28211")>]
 [<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
 [<ComVisible(true)>]
 ICoreMesh =
    inherit ICore
    abstract Mesh:IMesh with get, set


/// CoreLine interface
[<Guid("4F523A32-5495-428B-B1B7-E295E99DB111")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type ICoreLine =
    inherit ICore


/// Cores(multiple core) interface
[<Guid("D38A5DE5-3E34-44B2-BADC-FF82B26B5E96")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type ICores =
    inherit ICore
    /// Number of cores
    abstract Count:int with get

    /// Child cores
    abstract Children:ICore array with get

    /// Get i-th core
    abstract GetAt: int -> ICore
    abstract Add: ICore->unit





[<Guid("56263952-EC2D-467A-BE50-73CD7D9B2B38")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type INode =
    inherit IGlDrawable
    abstract Parent: INodeStem with get, set
    abstract NodePath: string with get

and
 [<Guid("883761B2-1FB4-467B-9164-B4E420027E5D")>]
 [<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
 [<ComVisible(true)>]
 INodeStem =
    inherit INode
    abstract Children: INode array with get, set

[<Guid("049FBA7C-EB7C-4842-A384-29E9FA8B4951")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type INodeRoot =
    inherit INodeStem


/// Cell node 의 interface
[<Guid("E738079E-7404-47F1-89C4-62405096BAF9")>]
[<InterfaceType(ComInterfaceType.InterfaceIsDual)>]
[<ComVisible(true)>]
type INodeCell =
    inherit INodeRoot
