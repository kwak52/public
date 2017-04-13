namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Projectfolders
{
    internal class SymbolTableEntry
    {
        public string Symbol { get; set; }
        public string Comment { get; set; }
        public string Operand { get; set; }
        public string OperandIEC { get; set; }
        public string DataType { get; set; }
        public MemoryArea DataSource { get; set; }
        public override string ToString()
        {
            return Symbol + " (" + Operand + ")";
        }
    }
}
