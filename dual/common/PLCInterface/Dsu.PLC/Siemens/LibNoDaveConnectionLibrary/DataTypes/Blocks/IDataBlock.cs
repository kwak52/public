using DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks.Step7V5;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Blocks
{
    internal interface IDataBlock : IBlock
    {
        IDataRow Structure { get; set;}

        IDataRow GetArrayExpandedStructure(S7DataBlockExpandOptions myExpOpt);
    }
}
