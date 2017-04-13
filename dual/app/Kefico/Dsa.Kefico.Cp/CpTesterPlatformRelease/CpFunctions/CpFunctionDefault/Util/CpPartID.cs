using CpTesterPlatform.CpMngLib.BaseClass;
using CpTesterPlatform.CpSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CpTesterPlatform.CpCommon;
using CpTesterPlatform.CpApplication.CpApplicationIntrf;
using static CpCommon.ExceptionHandler;
using Dsu.Driver.Math;
using System.IO;
using CpTesterPlatform.CpMngLib.Interface;
using CpTesterPlatform.CpMngLib.Manager;
using CpTesterPlatform.CpTStepDev;
using System.Linq.Expressions;
using System.Data;
using static CpBase.CpLog4netLogging;
using Dsu.Driver.Base;

namespace CpTesterPlatform.Functions
{
    public class CpPartID
    {
        public DateTime CreateTime { get;  }
        public string Mark3 { get; }
        public string Mark4 { get; }
        public string MesID { get; }

        public CpPartID(DateTime createTime, string mark3, string mark4)
        {
            CreateTime = createTime;
            Mark3 = mark3;
            Mark4 = mark4;

            MesID = mark3.Replace(" ", "") + mark4.Replace(" ", "");
        }
    }
}
