using System;
using System.Collections.Generic;


namespace Dsa.Hmc.SensorDB
{

    [Serializable]
    public class TagData
    {
        public string Name { get; private set; }
        public string Group { get; private set; }
        public string Position { get; private set; }
        public string Address { get; set; }
        public float Offset { get; set; }
        public float Limit { get; set; }
        public string CarType { get; set; }
        public bool IsSensor { get; set; }
        public bool IsLogging { get; set; }
        public object Value { get; set; }
        public string LogName { get { return Name.Split(' ').Length > 1 ? Name.Split(' ')[1] : ""; } }

        public TagData(string name, string group, string position, string address, float offset, float limit, string carType, bool isSensor, bool isLogEvent)
        {
            Name = string.Format("{0}{1}", position == "" ? position : position + " ", name);
            Group = group;
            Position = position;
            Address = address;
            Offset = offset;
            Limit = limit;
            CarType = carType;
            IsSensor = isSensor;
            IsLogging = isLogEvent;
        }

        public TagData(int number, string group, string position, string address, float offset, float limit, string carType, bool isSensor, bool isLogEvent)
        {
            Name = string.Format("{0} {1}{2}", position, group, number);

            Group = group;
            Position = position;
            Address = address;
            Offset = offset;
            Limit = limit;
            CarType = carType;
            IsSensor = isSensor;
            IsLogging = isLogEvent;
        }
    }

    [Serializable]
    public class TagDataSource
    {
        public List<TagData> Items { get; private set; }
        public TagData PLCOn { get; private set; }
        public TagData SelectCarType { get; set; }

        public TagData PartTag { get; private set; }
        public TagData PartCarTypeA { get; private set; }
        public TagData PartCarTypeB { get; private set; }
        public TagData PartCarTypeC { get; private set; }
        public TagData PartCarTypeD { get; private set; }
        public TagData PartCarTypeE { get; private set; }
        public TagData PartCarTypeF { get; private set; }
        public TagData PartCarTypeG { get; private set; }
        public TagData PartCarTypeH { get; private set; }
        public TagData PartCarTypeI { get; private set; }
        public TagData PartCarTypeJ { get; private set; }
        public TagData PartCarTypeK { get; private set; }
        public TagData PartCarTypeL { get; private set; }
        public TagData PartCarTypeM { get; private set; }
        public TagData PartCarTypeN { get; private set; }
        public TagData PartCarTypeO { get; private set; }
        public TagData PartCarTypeP { get; private set; }
        public TagData PartCarNo { get; private set; }

        public TagData AssyTag { get; private set; }
        //public TagData AssyCarTypeA { get; private set; }
        //public TagData AssyCarTypeB { get; private set; }
        //public TagData AssyCarTypeC { get; private set; }
        //public TagData AssyCarTypeD { get; private set; }
        //public TagData AssyCarTypeE { get; private set; }
        //public TagData AssyCarTypeF { get; private set; }
        //public TagData AssyCarTypeG { get; private set; }
        //public TagData AssyCarTypeH { get; private set; }
        //public TagData AssyCarTypeI { get; private set; }
        //public TagData AssyCarTypeJ { get; private set; }
        //public TagData AssyCarTypeK { get; private set; }
        //public TagData AssyCarTypeL { get; private set; }
        //public TagData AssyCarTypeM { get; private set; }
        //public TagData AssyCarTypeN { get; private set; }
        //public TagData AssyCarTypeO { get; private set; }
        //public TagData AssyCarTypeP { get; private set; }
        public TagData AssyCarNo { get; private set; }

        public string GroupA { get; private set; } = "RH";
        public string GroupB { get; private set; } = "LH";
        public string PositionA { get; private set; } = "PART";
        public string PositionB { get; private set; } = "ASSY";
        public string PartCarType
        {
            get
            {
                if (Convert.ToInt32(PartCarTypeA.Value) != 0) return PartCarTypeA.CarType;
                else if (Convert.ToInt32(PartCarTypeB.Value) != 0) return PartCarTypeB.CarType;
                else if (Convert.ToInt32(PartCarTypeC.Value) != 0) return PartCarTypeC.CarType;
                else if (Convert.ToInt32(PartCarTypeD.Value) != 0) return PartCarTypeD.CarType;
                else if (Convert.ToInt32(PartCarTypeE.Value) != 0) return PartCarTypeE.CarType;
                else if (Convert.ToInt32(PartCarTypeF.Value) != 0) return PartCarTypeF.CarType;
                else if (Convert.ToInt32(PartCarTypeG.Value) != 0) return PartCarTypeG.CarType;
                else if (Convert.ToInt32(PartCarTypeH.Value) != 0) return PartCarTypeH.CarType;
                else if (Convert.ToInt32(PartCarTypeI.Value) != 0) return PartCarTypeI.CarType;
                else if (Convert.ToInt32(PartCarTypeJ.Value) != 0) return PartCarTypeJ.CarType;
                else if (Convert.ToInt32(PartCarTypeK.Value) != 0) return PartCarTypeK.CarType;
                else if (Convert.ToInt32(PartCarTypeL.Value) != 0) return PartCarTypeL.CarType;
                else if (Convert.ToInt32(PartCarTypeM.Value) != 0) return PartCarTypeM.CarType;
                else if (Convert.ToInt32(PartCarTypeN.Value) != 0) return PartCarTypeN.CarType;
                else if (Convert.ToInt32(PartCarTypeO.Value) != 0) return PartCarTypeO.CarType;
                else if (Convert.ToInt32(PartCarTypeP.Value) != 0) return PartCarTypeP.CarType;
                else
                    return "";
            }
        }
        public string AssyCarType
        {
            get
            {
                return PartCarType;
            }
        }

        public TagDataSource()
        {
            Items = new List<TagData>();

            // Sensors
            Items.Add(new TagData(1, GroupA, PositionA, "D4100", 79.2f, 1.5f, "", true, false));
            Items.Add(new TagData(1, GroupA, PositionB, "D4200", 79.2f, 1.5f, "", true, false));
            Items.Add(new TagData(2, GroupA, PositionA, "D4110", 82.2f, 1.5f, "", true, false));
            Items.Add(new TagData(2, GroupA, PositionB, "D4210", 82.2f, 1.5f, "", true, false));
            Items.Add(new TagData(3, GroupA, PositionA, "D4120", 81.1f, 1.5f, "", true, false));
            Items.Add(new TagData(3, GroupA, PositionB, "D4220", 81.1f, 1.5f, "", true, false));
            Items.Add(new TagData(4, GroupA, PositionA, "D4130", 76.4f, 1.5f, "", true, false));
            Items.Add(new TagData(4, GroupA, PositionB, "D4230", 76.4f, 1.5f, "", true, false));
            Items.Add(new TagData(1, GroupB, PositionA, "D4170", 79f, 1.5f, "", true, false));
            Items.Add(new TagData(1, GroupB, PositionB, "D4270", 79f, 1.5f, "", true, false));
            Items.Add(new TagData(2, GroupB, PositionA, "D4160", 82f, 1.5f, "", true, false));
            Items.Add(new TagData(2, GroupB, PositionB, "D4260", 82f, 1.5f, "", true, false));
            Items.Add(new TagData(3, GroupB, PositionA, "D4150", 79f, 1.5f, "", true, false));
            Items.Add(new TagData(3, GroupB, PositionB, "D4250", 79f, 1.5f, "", true, false));
            Items.Add(new TagData(4, GroupB, PositionA, "D4140", 95.5f, 1.5f, "", true, false));
            Items.Add(new TagData(4, GroupB, PositionB, "D4240", 95.5f, 1.5f, "", true, false));

            //HeartBit
            PLCOn = new TagData("PLC 연결확인", "", "", "SM412", 0, 0, "", false, false);
            Items.Add(PLCOn);

            //PART
            PartCarTypeA = new TagData("CAR A", "", PositionA, "M4065", 0, 0, "CK", false, false);
            PartCarTypeB = new TagData("CAR B", "", PositionA, "L4132", 0, 0, "BB", false, false);
            PartCarTypeC = new TagData("CAR C", "", PositionA, "L4145", 0, 0, "CC", false, false);
            PartCarTypeD = new TagData("CAR D", "", PositionA, "L4146", 0, 0, "DD", false, false);
            PartCarTypeE = new TagData("CAR E", "", PositionA, "L4146", 0, 0, "EE", false, false);
            PartCarTypeF = new TagData("CAR F", "", PositionA, "L4146", 0, 0, "FF", false, false);
            PartCarTypeG = new TagData("CAR G", "", PositionA, "L4146", 0, 0, "GG", false, false);
            PartCarTypeH = new TagData("CAR H", "", PositionA, "L4146", 0, 0, "HH", false, false);
            PartCarTypeI = new TagData("CAR I", "", PositionA, "L4146", 0, 0, "II", false, false);
            PartCarTypeJ = new TagData("CAR J", "", PositionA, "L4146", 0, 0, "JJ", false, false);
            PartCarTypeK = new TagData("CAR K", "", PositionA, "L4146", 0, 0, "KK", false, false);
            PartCarTypeL = new TagData("CAR L", "", PositionA, "L4146", 0, 0, "LL", false, false);
            PartCarTypeM = new TagData("CAR M", "", PositionA, "L4146", 0, 0, "MM", false, false);
            PartCarTypeN = new TagData("CAR N", "", PositionA, "L4146", 0, 0, "NN", false, false);
            PartCarTypeO = new TagData("CAR O", "", PositionA, "L4146", 0, 0, "OO", false, false);
            PartCarTypeP = new TagData("CAR P", "", PositionA, "L4146", 0, 0, "PP", false, false);
            PartCarNo = new TagData("차량 No", "", PositionA, "W49", 0, 0, "", false, false);
            PartTag = new TagData("측정시점", "", PositionA, "L4040", 0, 0, "", false, true);

            Items.Add(PartCarTypeA);
            Items.Add(PartCarTypeB);
            Items.Add(PartCarTypeC);
            Items.Add(PartCarTypeD);
            Items.Add(PartCarTypeE);
            Items.Add(PartCarTypeF);
            Items.Add(PartCarTypeG);
            Items.Add(PartCarTypeH);
            Items.Add(PartCarTypeI);
            Items.Add(PartCarTypeJ);
            Items.Add(PartCarTypeK);
            Items.Add(PartCarTypeL);
            Items.Add(PartCarTypeM);
            Items.Add(PartCarTypeN);
            Items.Add(PartCarTypeO);
            Items.Add(PartCarTypeP);
            Items.Add(PartCarNo);
            Items.Add(PartTag);

            //ASSY
            //AssyCarTypeA = new TagData("CAR A", "", PositionB, "L4131", 0, 0, "CK", false, false);
            //AssyCarTypeB = new TagData("CAR B", "", PositionB, "L4132", 0, 0, "BB", false, false);
            //AssyCarTypeC = new TagData("CAR C", "", PositionB, "L4145", 0, 0, "CC", false, false);
            //AssyCarTypeD = new TagData("CAR D", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeE = new TagData("CAR E", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeF = new TagData("CAR F", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeG = new TagData("CAR G", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeH = new TagData("CAR H", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeI = new TagData("CAR I", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeJ = new TagData("CAR J", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeK = new TagData("CAR K", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeL = new TagData("CAR L", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeM = new TagData("CAR M", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeN = new TagData("CAR N", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeO = new TagData("CAR O", "", PositionB, "L4146", 0, 0, "DD", false, false);
            //AssyCarTypeP = new TagData("CAR P", "", PositionB, "L4146", 0, 0, "DD", false, false);
            AssyCarNo = new TagData("차량 No", "", PositionB, "W49", 0, 0, "", false, false);
            AssyTag = new TagData("측정시점", "", PositionB, "L4041", 0, 0, "", false, true);

            //Items.Add(AssyCarTypeA);
            //Items.Add(AssyCarTypeB);
            //Items.Add(AssyCarTypeC);
            //Items.Add(AssyCarTypeD);
            //Items.Add(AssyCarTypeE);
            //Items.Add(AssyCarTypeF);
            //Items.Add(AssyCarTypeG);
            //Items.Add(AssyCarTypeH);
            //Items.Add(AssyCarTypeI);
            //Items.Add(AssyCarTypeJ);
            //Items.Add(AssyCarTypeK);
            //Items.Add(AssyCarTypeL);
            //Items.Add(AssyCarTypeM);
            //Items.Add(AssyCarTypeN);
            //Items.Add(AssyCarTypeO);
            //Items.Add(AssyCarTypeP);
            Items.Add(AssyCarNo);
            Items.Add(AssyTag);
        }
    }
}