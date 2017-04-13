using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace Dsa.Hmc.Spc
{
    public delegate void UEventHandlerDataReceived(object sender, string msg);

    class SerialLVDT
    {
        private SerialPort serialLVDT = new SerialPort();
        public UEventHandlerDataReceived UEventDataReceived;

        public SerialLVDT()
        {
            serialLVDT.DataReceived += SerialLVDT_DataReceived;
            serialLVDT.PortName = "COM3";
            serialLVDT.BaudRate = 9600;
           // serialLVDT.Open();

        }

        private void SerialLVDT_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sRead = serialLVDT.ReadLine();

            if (sRead.Contains("\u0005") && sRead.Contains("\u0003"))
            {
                string convert = sRead.Split(new string[] { "\u0005" }, StringSplitOptions.None)[1];
                convert = convert.Split(new string[] { "\u0003" }, StringSplitOptions.None)[0];
                UEventDataReceived?.Invoke(sender, convert);
            }
        }
    }
}
