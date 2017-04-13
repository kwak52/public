using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.Controls
{
    /// <summary> Disabled 상태에서 tooltip 을 제공하는 control(->disabled) 을 포함하려는 control(supporter) 이 구현해야 할 interface </summary>
    /// 
    /// <br/> - disabled 가 supporter 의 직접 child 가 아닌 경우의 처리 : disabled 의 직접 parent 의 mouse move event 를 supporter 에게 전달해서 해결.
    [ComVisible(false)]
    public interface IDisabledChildControlTooltipSupporter
    {
        /// <summary> disabled control 과 해당 control 이 가지는 tooltip text 와의 map </summary>
        Dictionary<Control, string> DisabledChildControlToDescriptionMap { get; }

        /// <summary> Just value holder.  nothing to do with this property </summary>
        ToolTip TooltipForDisabledControl { get; set; }
        /// <summary> Just value holder.  nothing to do with this property </summary>
        Control LastTooltipShownControl { get; set; }
    }


    public static class DisabledChildControlTooltipInitializer
    {
        private static void MouseMoveProc(Control parent, Dictionary<Control, string> map)
        {
            var supporter = (IDisabledChildControlTooltipSupporter)parent.CollectAncestors(true).FirstOrDefault(x => x is IDisabledChildControlTooltipSupporter);
            var last = supporter.LastTooltipShownControl;
            var c = parent.GetChildrenUnderMouse().FirstOrDefault(x => map.ContainsKey(x));
            if (c == null)
            {
                if (last != null)
                {
                    supporter.TooltipForDisabledControl.Hide(last);
                    supporter.LastTooltipShownControl = null;
                }
                return;
            }

            if (c != last)
            {
                supporter.LastTooltipShownControl = c;
                if (map.ContainsKey(c))
                {
                    var description = map[c];
                    supporter.TooltipForDisabledControl.Show(description, parent, parent.PointToClient(Cursor.Position));
                }
            }            
        }
        public static void InitializeDisabledChildControlTooltipSupporter(this IDisabledChildControlTooltipSupporter supporter)
        {
            //
            // http://stackoverflow.com/questions/7887817/c-sharp-display-a-tooltip-on-disabled-textbox-form
            //
            supporter.TooltipForDisabledControl = new ToolTip();
            var map = supporter.DisabledChildControlToDescriptionMap;
            var s = (Control)supporter;

            s.CollectChildren()                     // Supporter 의 모든 children(recursive) 에 대해서
                .Where(x => map.ContainsKey(x))     // disabled tooltip 이 지정되어 있는 것들의 
                .Select(x => x.Parent).Distinct()   // parent 를 unique 하게 골라서
                .ForEach(container =>
            {
                if ( container != s )
                {
                    // mouse move event 를 supporter 가 알수 있도록 forwarding
                    container.MouseMove += (sender, args) => { MouseMoveProc(container, map); };
                }
            });

            // supporter 에서 직접 mouse event 처리하는 경우.
            s.MouseMove += (sender, e) => { MouseMoveProc(s, map); };
        }
    }
}
