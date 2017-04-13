using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ChartGantt_Form
{
    [Serializable]
    public class TagDataSource
    {
        public List<TagData> Items { get; set; }
        public TagDataSource()
        {
            Items = new List<TagData>();
            CreateTags();
        }

        private void CreateTags()
        {

            Items.Add(new TagData("RH1", EmFieldDB.rh1, "D4100", EmPoint.LOADING, EmEventType.DataDB, "", -79.2f));
            Items.Add(new TagData("RH2", EmFieldDB.rh2, "D4110", EmPoint.LOADING, EmEventType.DataDB, "", -82.2f));
            Items.Add(new TagData("RH3", EmFieldDB.rh3, "D4120", EmPoint.LOADING, EmEventType.DataDB, "", -81.1f));
            Items.Add(new TagData("RH4", EmFieldDB.rh4, "D4130", EmPoint.LOADING, EmEventType.DataDB, "", -76.4f));
            Items.Add(new TagData("LH1", EmFieldDB.lh1, "D4140", EmPoint.LOADING, EmEventType.DataDB, "", -95.5f));
            Items.Add(new TagData("LH2", EmFieldDB.lh2, "D4150", EmPoint.LOADING, EmEventType.DataDB, "", -79f));
            Items.Add(new TagData("LH3", EmFieldDB.lh3, "D4160", EmPoint.LOADING, EmEventType.DataDB, "", -82f));
            Items.Add(new TagData("LH4", EmFieldDB.lh4, "D4170", EmPoint.LOADING, EmEventType.DataDB, "", -79f));

            Items.Add(new TagData("RH1", EmFieldDB.rh1, "D4200", EmPoint.PART, EmEventType.DataDB, "", -79.2f));
            Items.Add(new TagData("RH2", EmFieldDB.rh2, "D4210", EmPoint.PART, EmEventType.DataDB, "", -82.2f));
            Items.Add(new TagData("RH3", EmFieldDB.rh3, "D4220", EmPoint.PART, EmEventType.DataDB, "", -81.1f));
            Items.Add(new TagData("RH4", EmFieldDB.rh4, "D4230", EmPoint.PART, EmEventType.DataDB, "", -76.4f));
            Items.Add(new TagData("LH1", EmFieldDB.lh1, "D4240", EmPoint.PART, EmEventType.DataDB, "", -95.5f));
            Items.Add(new TagData("LH2", EmFieldDB.lh2, "D4250", EmPoint.PART, EmEventType.DataDB, "", -79f));
            Items.Add(new TagData("LH3", EmFieldDB.lh3, "D4260", EmPoint.PART, EmEventType.DataDB, "", -82f));
            Items.Add(new TagData("LH4", EmFieldDB.lh4, "D4270", EmPoint.PART, EmEventType.DataDB, "", -79f));

            Items.Add(new TagData("RH1", EmFieldDB.rh1, "D4300", EmPoint.ASSY, EmEventType.DataDB, "", -79.2f));
            Items.Add(new TagData("RH2", EmFieldDB.rh2, "D4310", EmPoint.ASSY, EmEventType.DataDB, "", -82.2f));
            Items.Add(new TagData("RH3", EmFieldDB.rh3, "D4320", EmPoint.ASSY, EmEventType.DataDB, "", -81.1f));
            Items.Add(new TagData("RH4", EmFieldDB.rh4, "D4330", EmPoint.ASSY, EmEventType.DataDB, "", -76.4f));
            Items.Add(new TagData("LH1", EmFieldDB.lh1, "D4340", EmPoint.ASSY, EmEventType.DataDB, "", -95.5f));
            Items.Add(new TagData("LH2", EmFieldDB.lh2, "D4350", EmPoint.ASSY, EmEventType.DataDB, "", -79f));
            Items.Add(new TagData("LH3", EmFieldDB.lh3, "D4360", EmPoint.ASSY, EmEventType.DataDB, "", -82f));
            Items.Add(new TagData("LH4", EmFieldDB.lh4, "D4370", EmPoint.ASSY, EmEventType.DataDB, "", -79f));

            Items.Add(new TagData("RH1", EmFieldDB.rh1, "D4400", EmPoint.UNLOADING, EmEventType.DataDB, "", -79.2f));
            Items.Add(new TagData("RH2", EmFieldDB.rh2, "D4410", EmPoint.UNLOADING, EmEventType.DataDB, "", -82.2f));
            Items.Add(new TagData("RH3", EmFieldDB.rh3, "D4420", EmPoint.UNLOADING, EmEventType.DataDB, "", -81.1f));
            Items.Add(new TagData("RH4", EmFieldDB.rh4, "D4430", EmPoint.UNLOADING, EmEventType.DataDB, "", -76.4f));
            Items.Add(new TagData("LH1", EmFieldDB.lh1, "D4440", EmPoint.UNLOADING, EmEventType.DataDB, "", -95.5f));
            Items.Add(new TagData("LH2", EmFieldDB.lh2, "D4450", EmPoint.UNLOADING, EmEventType.DataDB, "", -79f));
            Items.Add(new TagData("LH3", EmFieldDB.lh3, "D4460", EmPoint.UNLOADING, EmEventType.DataDB, "", -82f));
            Items.Add(new TagData("LH4", EmFieldDB.lh4, "D4470", EmPoint.UNLOADING, EmEventType.DataDB, "", -79f));

            Items.Add(new TagData("PLCOn", EmFieldDB.None, "SM412", EmPoint.None, EmEventType.HeartBit));
            Items.Add(new TagData("CarNo", EmFieldDB.None, "W49", EmPoint.None, EmEventType.CarNumber));
            Items.Add(new TagData("ReadPart", EmFieldDB.None, "L4040", EmPoint.PART, EmEventType.ReadEvent));
            Items.Add(new TagData("ReadLoad", EmFieldDB.None, "L4041", EmPoint.LOADING, EmEventType.ReadEvent));
            Items.Add(new TagData("ReadAssy", EmFieldDB.None, "L4042", EmPoint.ASSY, EmEventType.ReadEvent));
            Items.Add(new TagData("ReadUnLoad", EmFieldDB.None, "L4043", EmPoint.UNLOADING, EmEventType.ReadEvent));


            Items.Add(new TagData(EmCar.CarTypeA.ToString(), EmFieldDB.None, "M4002", EmPoint.PART, EmEventType.CarType, "PD"));
            Items.Add(new TagData(EmCar.CarTypeB.ToString(), EmFieldDB.None, "M4003", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeC.ToString(), EmFieldDB.None, "M4004", EmPoint.PART, EmEventType.CarType, "UC"));
            Items.Add(new TagData(EmCar.CarTypeD.ToString(), EmFieldDB.None, "M4005", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeE.ToString(), EmFieldDB.None, "M4006", EmPoint.PART, EmEventType.CarType, "ID"));
            Items.Add(new TagData(EmCar.CarTypeF.ToString(), EmFieldDB.None, "M4007", EmPoint.PART, EmEventType.CarType, "FF"));
            Items.Add(new TagData(EmCar.CarTypeG.ToString(), EmFieldDB.None, "M4008", EmPoint.PART, EmEventType.CarType, "HCI"));
            Items.Add(new TagData(EmCar.CarTypeH.ToString(), EmFieldDB.None, "M4009", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeI.ToString(), EmFieldDB.None, "M4064", EmPoint.PART, EmEventType.CarType, "CB"));
            Items.Add(new TagData(EmCar.CarTypeJ.ToString(), EmFieldDB.None, "M4065", EmPoint.PART, EmEventType.CarType, "CK"));
            Items.Add(new TagData(EmCar.CarTypeK.ToString(), EmFieldDB.None, "M4067", EmPoint.PART, EmEventType.CarType, "IK"));
            Items.Add(new TagData(EmCar.CarTypeL.ToString(), EmFieldDB.None, "M4068", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeM.ToString(), EmFieldDB.None, "M4069", EmPoint.PART, EmEventType.CarType, "YB"));
            Items.Add(new TagData(EmCar.CarTypeN.ToString(), EmFieldDB.None, "M4070", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeO.ToString(), EmFieldDB.None, "M4071", EmPoint.PART, EmEventType.CarType, "OS"));
            Items.Add(new TagData(EmCar.CarTypeP.ToString(), EmFieldDB.None, "M4072", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeQ.ToString(), EmFieldDB.None, "M4073", EmPoint.PART, EmEventType.CarType, "MD"));
            Items.Add(new TagData(EmCar.CarTypeR.ToString(), EmFieldDB.None, "M4074", EmPoint.PART, EmEventType.CarType, "IG"));
            Items.Add(new TagData(EmCar.CarTypeS.ToString(), EmFieldDB.None, "M4075", EmPoint.PART, EmEventType.CarType, "JS"));
            Items.Add(new TagData(EmCar.CarTypeT.ToString(), EmFieldDB.None, "M4076", EmPoint.PART, EmEventType.CarType, "AB"));
            Items.Add(new TagData(EmCar.CarTypeU.ToString(), EmFieldDB.None, "M4077", EmPoint.PART, EmEventType.CarType, "HC"));
            Items.Add(new TagData(EmCar.CarTypeV.ToString(), EmFieldDB.None, "M4078", EmPoint.PART, EmEventType.CarType, "SP"));
            Items.Add(new TagData(EmCar.CarTypeW.ToString(), EmFieldDB.None, "M4079", EmPoint.PART, EmEventType.CarType, "SP"));

            //Items.Add(new TagData("CarTypeA", EmFieldDB.None, "M4065", EmPoint.ASSY, EmEventType.CarType, "CK"));
            //Items.Add(new TagData("CarTypeB", EmFieldDB.None, "M5066", EmPoint.ASSY, EmEventType.CarType, "BB"));
            //Items.Add(new TagData("CarTypeC", EmFieldDB.None, "M5067", EmPoint.ASSY, EmEventType.CarType, "CC"));
            //Items.Add(new TagData("CarTypeD", EmFieldDB.None, "M5068", EmPoint.ASSY, EmEventType.CarType, "DD"));
            //Items.Add(new TagData("CarTypeE", EmFieldDB.None, "M5069", EmPoint.ASSY, EmEventType.CarType, "EE"));
            //Items.Add(new TagData("CarTypeF", EmFieldDB.None, "M5070", EmPoint.ASSY, EmEventType.CarType, "FF"));

            //Items.Add(new TagData("CarTypeA", EmFieldDB.None, "M4065", EmPoint.LOADING, EmEventType.CarType, "CK"));
            //Items.Add(new TagData("CarTypeB", EmFieldDB.None, "M6066", EmPoint.LOADING, EmEventType.CarType, "BB"));
            //Items.Add(new TagData("CarTypeC", EmFieldDB.None, "M6067", EmPoint.LOADING, EmEventType.CarType, "CC"));
            //Items.Add(new TagData("CarTypeD", EmFieldDB.None, "M6068", EmPoint.LOADING, EmEventType.CarType, "DD"));
            //Items.Add(new TagData("CarTypeE", EmFieldDB.None, "M6069", EmPoint.LOADING, EmEventType.CarType, "EE"));
            //Items.Add(new TagData("CarTypeF", EmFieldDB.None, "M6070", EmPoint.LOADING, EmEventType.CarType, "FF"));

            //Items.Add(new TagData("CarTypeA", EmFieldDB.None, "M4065", EmPoint.UNLOADING, EmEventType.CarType, "CK"));
            //Items.Add(new TagData("CarTypeB", EmFieldDB.None, "M7066", EmPoint.UNLOADING, EmEventType.CarType, "BB"));
            //Items.Add(new TagData("CarTypeC", EmFieldDB.None, "M7067", EmPoint.UNLOADING, EmEventType.CarType, "CC"));
            //Items.Add(new TagData("CarTypeD", EmFieldDB.None, "M7068", EmPoint.UNLOADING, EmEventType.CarType, "DD"));
            //Items.Add(new TagData("CarTypeE", EmFieldDB.None, "M7069", EmPoint.UNLOADING, EmEventType.CarType, "EE"));
            //Items.Add(new TagData("CarTypeF", EmFieldDB.None, "M7070", EmPoint.UNLOADING, EmEventType.CarType, "FF"));

            //Items.Add(new TagData("CarTypeG", EmFieldDB.None, "M4071", EmPoint.None, EmEventType.CarType));
            //Items.Add(new TagData("CarTypeH", EmFieldDB.None, "M4072", EmPoint.None, EmEventType.CarType));
            //Items.Add(new TagData("CarTypeI", EmFieldDB.None, "M4073", EmPoint.None, EmEventType.CarType));
            //Items.Add(new TagData("CarTypeJ", EmFieldDB.None, "M4074", EmPoint.None, EmEventType.CarType));
            //Items.Add(new TagData("CarTypeK", EmFieldDB.None, "M4075", EmPoint.None, EmEventType.CarType));
            //Items.Add(new TagData("CarTypeL", EmFieldDB.None, "M4076", EmPoint.None, EmEventType.CarType));
        }
    }
}