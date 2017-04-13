using System;
using System.Collections.ObjectModel;


namespace Dsa.Hmc.Spc
{
    [Serializable]
    public class ConfigData
    {
        public string Name { get; set; } = "FENDER APRON & FRTSIDE MBR COMPL SPC";
        public string Ip { get; set; } = "192.168.1.99";
        public int Port { get; set; } = 5001;
        public float MinIN { get; set; } = 0.0f;
        public float MaxIN { get; set; } = 5000.0f;
        public float MinOut { get; set; } = 60.0f;
        public float MaxOut { get; set; } = 100.0f;
        public string DisplayCar { get; set; } = "AA";
        public string ExportPath { get; set; } = "D:\\";
        public string LHPath { get; set; }
        public string RHPath { get; set; }
        public bool DisplaySensor { get; set; } 
        public ConfigData()
        {
        }
        public ConfigData(string name, string ip, int port, string exportPath)
        {
            Name = name;
            Ip = ip;
            Port = port;
            ExportPath = exportPath;
        }
    }
}
