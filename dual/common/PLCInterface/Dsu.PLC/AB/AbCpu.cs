using System;
using ControlLogixNET;

namespace Dsu.PLC.AB
{
    internal class AbCpu :ICpu
    {
        private LogixProcessor _processor;

        public string Model { get; }
        public bool IsRunning { get {throw new NotImplementedException();} }

        public void Stop() => _processor.SetProgramMode();
        public void Run() => _processor.SetRunMode();

        public AbCpu(LogixProcessor processor)
        {
            _processor = processor;
        }
    }
}
