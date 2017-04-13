using System;
using System.Windows.Forms;


namespace Dsu.Common.Utilities
{
    public partial class Tools
    {
        static public Control FindAncestor(Control ctrl, Type t) { return FindAncestor(ctrl, t, 100);  }
        static public Control FindAncestor(Control ctrl, Type t, int nMaxStep)
        {
            Control parent = ctrl;
            for ( int i = 0; i < nMaxStep; i++ )
            {
                if ( parent == null )
                    return null;

                if ( parent.GetType() == t )
                    return parent;
                parent = parent.Parent;
            }

            return null;
        }

    }
}
