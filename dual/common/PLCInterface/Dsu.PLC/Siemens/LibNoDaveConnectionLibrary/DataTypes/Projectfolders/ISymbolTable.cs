using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Projectfolders
{
    internal interface ISymbolTable : IProjectFolder
    {
        String Folder { get; set; }
        
        SymbolTableEntry GetEntryFromOperand(string operand);
        SymbolTableEntry GetEntryFromSymbol(string symbol);

        List<SymbolTableEntry> SymbolTableEntrys { get; set; }
    }
}
