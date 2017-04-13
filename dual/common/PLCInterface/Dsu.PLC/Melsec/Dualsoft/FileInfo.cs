using System;

namespace Dsu.PLC.Melsec.Dualsoft
{
    internal class FileInfo
    {
        /// <summary>
        /// sh080170b.pdf, pp. 7-16, http://es-electro.ru/files/L_series_CPU_Module.pdf, pp.32
        /// EXTENSIONS : QPG, QCD, QDI, QDR, QDL, QTD, QTX, QTR, QFD, QAD, QTM and WTM
        /// SPECIAL FILES : QN.QSY, WORK.QSY, TRACE.QSY, PARAM.QPA, IPARAM.QPA, AUTOEXEC.QBT, Q00.DAT to Q99.DAT and MONITOR.Q00 to MONITOR.Q0F
        /// 
        /// System Area??
        ///     .QSY : e.g. {QN, WORK, TRACE}.QSY
        ///     .Q00 : e.g. MONITOR.Q00
        /// Program Memory (Device 0)
        ///     PARAM.QPA : Parameter
        ///     IPARAM.QPA : Intelligent function module parameters
        ///     .QPG : Program.  e.g MAIN.QPG
        ///     .QCD : Device comment.  e.g. COMMENT.QCD
        ///     .QDI : Initial device value
        ///     .CAB : ??? e.g. {SRCINF1I, SRCINF2I}.CAB
        /// Standard RAM
        ///     .QDR : File register
        ///     .QDL : Local device
        ///     .QTD : Sampling trace
        /// </summary>
        public string Name { get; set; }
        public int Size { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Readable { get; internal set; } = true;
        public bool Writable { get; internal set; } = true;

        public FileInfo(string name)
        {
            Name = name;
        }

        public FileInfo(FileInfo fi)
        {
            Name = fi.Name;
            Size = fi.Size;
            CreatedTime = fi.CreatedTime;
            Readable = fi.Readable;
            Writable = fi.Writable;
        }

        public override string ToString()
        {
            var att = (Readable ? "R" : "") + (Writable ? "W" : "");
            return $"{Name}: Size={Size}, Att={att}, CreatedTime={CreatedTime}";
        }
    }


    internal class FileSnapshot : FileInfo
    {
        public byte[] FileContents { get; }

        public FileSnapshot(string name, byte[] contents)
            : base(name)
        {
            FileContents = contents;
        }

        public FileSnapshot(FileInfo fi, byte[] contents)
            : base(fi)
        {
            FileContents = contents;
        }
    }
}
