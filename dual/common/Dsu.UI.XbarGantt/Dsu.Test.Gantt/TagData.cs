 using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ChartGantt_Form
{
    public enum EmCar
    {
        CarTypeA,
        CarTypeB,
        CarTypeC,
        CarTypeD,
        CarTypeE,
        CarTypeF,
        CarTypeG,
        CarTypeH,
        CarTypeI,
        CarTypeJ,
        CarTypeK,
        CarTypeL,
        CarTypeM,
        CarTypeN,
        CarTypeO,
        CarTypeP,
        CarTypeQ,
        CarTypeR,
        CarTypeS,
        CarTypeT,
        CarTypeU,
        CarTypeV,
        CarTypeW,
    }

    public enum EmPoint
    {
        None,
        LOADING,
        ASSY,
        PART,
        UNLOADING,
    }

    public enum EmEventType
    {
        None,
        HeartBit,
        CarType,
        CarNumber,
        DataDB,
        ReadEvent,
    }

    public enum EmFieldDB
    {
        None,
        time,
        carType,
        carNo,
        point,
        result,
        rh1,
        rh2,
        rh3,
        rh4,
        lh1,
        lh2,
        lh3,
        lh4,
    }

    public enum EmFieldTag
    {
        Name,
        FieldDB,
        Point,
        EventType,
        Address,
        Offset,
        Max,
        Min,
        Value,
        Comment,
        DisplayName,
        Car,
        lh3,
        lh4,
    }


    [Serializable]
    public class TagData
    {
        public string Name { get; private set; }
        public EmFieldDB FieldDB { get; private set; }
        public EmPoint Point { get; private set; }
        public EmEventType EventType { get; private set; }
        public string Address { get; set; }
        public float Offset
        {
            get
            {
                if (ListOffset.Count > 0)
                    return ListOffset[(int)Car];
                else
                    return 0;
            }
            set { if (ListOffset.Count > 0) ListOffset[(int)Car] = value; }
        }

        public double Max { get; set; } = 1.5;
        public double Min { get; set; } = -1.5;
        public object Value { get; set; }
        public string Comment { get; set; }
        public string DisplayName { get { return string.Format("{0} {1}", Name, Point.ToString().Substring(0, 1)); } }
        public EmCar Car { get; set; } = EmCar.CarTypeA;

        private List<float> ListOffset { get; set; } = new List<float>();
        public TagData(string name, EmFieldDB emFieldDB, string address, EmPoint emPoint, EmEventType emEventType, string comment = "", float offset = 0.0f)
        {
            Name = name;
            FieldDB = emFieldDB;
            Address = address;
            Point = emPoint;
            EventType = emEventType;
            Comment = comment;
            Offset = offset;

            foreach (var car in Enum.GetNames(typeof(EmCar)))
                ListOffset.Add(offset);
        }
    }
}