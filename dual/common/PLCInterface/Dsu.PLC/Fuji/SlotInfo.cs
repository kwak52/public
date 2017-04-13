using System.Diagnostics.Contracts;

namespace Dsu.PLC.Fuji
{
    /// <summary>
    /// IO card slot class
    /// </summary>
    internal class SlotInfo
    {
        /// <summary>
        /// Name of slot.  e.g "DC_Input_64points"
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// True on input, False on output card
        /// </summary>
        public bool IsInput { get; private set; }
        public int StartByteOffset { get; private set; }

        private int _byteLength;

        public int ByteLength
        {
            get { return _byteLength; }
            private set
            {
                Contract.Requires(value % 2 == 0);
                _byteLength = value;
            }
        }

        /// <summary>
        /// IO card 접점수 (word 단위)
        /// </summary>
        public int WordLength { get { return ByteLength/2;} }

        public SlotInfo(string name, bool isInput, int startByteOffset, int byteLength)
        {
            Name = name;
            IsInput = isInput;
            StartByteOffset = startByteOffset;
            ByteLength = byteLength;
        }
    }
}