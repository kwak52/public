using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NationalInstruments.DAQmx;

namespace TestDAQCircular
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //void Test()
        //{
        //    using (var task = new Task())
        //    {
        //        var channel = task.AIChannels.CreateVoltageChannel(physicalChannelName, "", terminalConfiguration, min, max, units);
        //        var reader = new AnalogSingleChannelReader(task.Stream);

        //        // Verify the task
        //        task.Control(TaskAction.Verify);

        //        var value = reader.ReadSingleSample()

        //    }

        //}
    }
}
