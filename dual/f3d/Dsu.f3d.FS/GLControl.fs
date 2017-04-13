namespace Dsu.f3d.FS

open OpenTK.Graphics
open Dsu.Common.Utilities.ExtensionMethods

type GLControlAntialiased(graphicsMode, major, minor, flags) as this =
    inherit OpenTK.GLControl(graphicsMode, major, minor, flags)
        
    static let _gcFlags = GraphicsContextFlags.ForwardCompatible
#if DEBUG
                            ||| GraphicsContextFlags.Debug
#endif

    static let graphicsMode = GraphicsMode(new ColorFormat(32), 24, 8, 8)
    let mutable components:System.ComponentModel.IContainer = null


    do
        components <- new System.ComponentModel.Container();
        this.AutoScaleMode <- System.Windows.Forms.AutoScaleMode.Font;

    override __.Dispose(disposing) =
            if (disposing && (components <> null)) then
                components.Dispose();
            base.Dispose(disposing);


    new() = new GLControlAntialiased(graphicsMode)
    new(graphicsMode) = new GLControlAntialiased(graphicsMode, 3, 0, _gcFlags)

    /// 현재 활성화된 OpenGL control 을 반환
    static member val ActiveContext:GLControlAntialiased option = None with get, set
    member x.MakeActive() = 
        GLControlAntialiased.ActiveContext <- Some(x)
        if not base.Context.IsCurrent then
            x.Do( fun () ->
                try 
                    x.MakeCurrent()
                with ex ->
                    ())
                    //ExceptionHider.SwallowException(ex))

open System.Windows.Forms
type F3dGLControl() =
    inherit UserControl()

    let mutable gLControlAA:GLControlAntialiased option = None
    member val GLControlAA = gLControlAA