using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetSiemensPLCToolBoxLibrary.Projectfiles.TIA.UsingTiaDlls
{
    internal class TiaObject
    {
        public TiaObject(TiaObjectStructure tiaObjectStructure)
        {
            this.TiaObjectStructure = tiaObjectStructure;
        }

        public TiaObjectStructure TiaObjectStructure { get; private set; }        
    }
}
