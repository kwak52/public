namespace DotNetSiemensPLCToolBoxLibrary.DataTypes.Projectfolders
{
    /// <summary>
    /// This is the Main Folder, it Contains a BlockInfos Folder and a Symbol Table!
    /// </summary>
    internal interface IProgrammFolder : IRootProgrammFolder
    {
        ISymbolTable SymbolTable { get; set; }
        IBlocksFolder BlocksFolder { get; set; }
        OnlineBlocksFolder OnlineBlocksFolder { get; set; }
    }
}
