// http://www.codeproject.com/Articles/24385/Have-a-Great-DesignTime-Experience-with-a-Powerful

namespace Dsu.Common.Utilities.DesignSurfaceExt
{
    public interface IDesignSurfaceExt2 : IDesignSurfaceExt
    {
        //- Get the IDesignerHost of the .NET 2.0 DesignSurface
        ToolboxServiceImp GetIToolboxService();
        void EnableDragandDrop();
    }
}
