namespace Dsu.f3d.FS

open OpenTK.Graphics

type RenderingContext(glControl:OpenTK.GLControl) =
    static member val CurrentGLControl:OpenTK.GLControl option = None with get, set
    member val GLControl = glControl with get, set
    member x.MakeCurrent() =
        RenderingContext.CurrentGLControl <- Some(x.GLControl)
        glControl.MakeCurrent()
