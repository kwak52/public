﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.Projectfiles.TIA.UsingTiaDlls.AttributeClasses
{
    [TiaAttributeId(Id = 77825, Name = "Siemens.Automation.ObjectFrame.ICoreAttributes")]
    internal class CoreAttributes : BaseTiaAttributeSet
    {
        internal CoreAttributes(TiaObjectStructure tiaObjectStructure)
            : base(tiaObjectStructure)
        { }

        public bool IsShell { get; set; }
        public bool Name { get; set; }
    }
}
