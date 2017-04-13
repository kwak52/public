using DotNetSiemensPLCToolBoxLibrary.DBF.Enums;

namespace DotNetSiemensPLCToolBoxLibrary.DBF.Index
{
    internal abstract class Field
    {
        public string get()
        {
            return string.Empty;
        }

        public dBaseType getType()
        {
            return dBaseType.C;
        }
    }
}
