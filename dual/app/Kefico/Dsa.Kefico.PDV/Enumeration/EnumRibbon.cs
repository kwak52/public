using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dsa.Kefico.PDV.Enumeration
{
    public enum EnumRibbon
    {
        Overview,
        History,
        Analysis
    }

    public static class RibbonMWS
    {
        private const string Overview = "Home";
        private const string History = "History";
        private const string Analysis = "Analysis";

        public static string GetName(EnumRibbon enumRibbon)
        {
            string name = string.Empty; ;
            if (enumRibbon == EnumRibbon.Overview)
                name = RibbonMWS.Overview;
            if (enumRibbon == EnumRibbon.History)
                name = RibbonMWS.History;
            if (enumRibbon == EnumRibbon.Analysis)
                name = RibbonMWS.Analysis;

            return name;
        }

        public static EnumRibbon GetType(string name)
        {
            EnumRibbon enumRibbon = EnumRibbon.Overview;
            if (name == RibbonMWS.Overview)
                enumRibbon = EnumRibbon.Overview;
            if (name == RibbonMWS.History)
                enumRibbon = EnumRibbon.History;
            if (name == RibbonMWS.Analysis)
                enumRibbon = EnumRibbon.Analysis;

            return enumRibbon;
        }
    }
}
