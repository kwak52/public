﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IDAQIO : IDevice
    {
		string ChannelID { set; get; }
		bool IsValidChannel(string strChannel);	
    }
}
