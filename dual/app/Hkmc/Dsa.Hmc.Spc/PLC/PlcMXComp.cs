using Dsu.PLC.Common;
using Dsu.PLC.Melsec;
using System;
using System.Collections.Generic;
using Dsu.PLC;

namespace Dsa.Hmc.Spc
{
    class PlcMXComp
    {
        private MxConnection _connection;
        private int PLCDelay = 400;
        public Dictionary<string, TagBase> Tags { get { return _connection?.Tags; } }
        public PlcMXComp() { }

        private IEnumerable<string> GenerateTagNames(TagDataSource tagDataSource)
        {
            foreach (var sensor in tagDataSource.Items)
                yield return sensor.Address;
        }

        public bool  IsConneted { get { return _connection == null ? false : true; } }

        public MxConnection Connect(ConfigData configData, TagDataSource tagDataSource)
        {
            _connection = new MxConnection(new MxConnectionParameters(configData.Ip, (ushort)configData.Port, TransportProtocol.Udp));
            _connection.PerRequestDelay = PLCDelay;
            if (_connection.Connect())
                _connection.CreateTags(GenerateTagNames(tagDataSource));

            return _connection;
        }

        public void DisConnect(IDisposable subscription)
        {
            if (subscription != null)
            {
                subscription.Dispose();
                subscription = null;
                _connection.StopDataExchangeLoop();
            }
        }

        public bool ReCollect(TagDataSource tagDataSource)
        {
            if (_connection != null && _connection.McProtocol != null)
            {
                _connection.Tags.Clear();
                _connection.CreateTags(GenerateTagNames(tagDataSource));
                _connection.PrepareDataExchangeLoop();
            }
            return true;
        }

    }
}
