using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using log4net.Core;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities
{
    public class LogEntryManager : IDisposable
    {
        private LoggingEvent[] _logEntries;
        public LoggingEvent[] LogEntries { get { return _logEntries; } }
        public int Capacity { get { return (int)_capacity; } set { _capacity = (ulong)value; } }
        private ulong _cursor = 0;
        private ulong _capacity = 0;

        public int Count { get { return (int)Math.Min(_cursor, (ulong)Capacity); } } 
        public LogEntryManager(int capacity)
        {
            Contract.Requires(capacity >= 0);
            Capacity = Math.Max(capacity, 1000);
            _logEntries = new LoggingEvent[Capacity];
        }

        public void AddLogEntry(LoggingEvent logEntry)
        {
            _logEntries[_cursor++ % _capacity] = logEntry;
        }

        public void Clear()
        {
            _logEntries = new LoggingEvent[Capacity];
            _cursor = 0;
        }

        /// <summary>
        /// logical index 를 physical index 로 변환해서 반환.
        /// e.g : capacity=1000, logical=1200 => physical=200 
        /// </summary>
        /// <param name="n">logical index number</param>
        /// <returns>physical index number : capacity 적용하여 rolling 한 결과 값</returns>
        public int LogicalIndex2PhysicalIndex(int n)
        {
            Contract.Requires(n.InRange(0, Capacity - 1));

            if (_cursor < _capacity)
                return n;

            return (int)((_cursor + (ulong)n) % _capacity);            
        }

        public int PhysicalIndex2LogicalIndex(int n)
        {
            if (_cursor < _capacity)
                return n;

            Debug.Assert((ulong)n < _cursor);

            return (int)((ulong) n + (_cursor/_capacity)*_capacity);
        }

        public LoggingEvent this[int n]
        {
            get { return _logEntries[LogicalIndex2PhysicalIndex(n)]; } 
        }

        public void Dispose()
        {
            _logEntries = null;
            _capacity = 0;
        }
    }
}
