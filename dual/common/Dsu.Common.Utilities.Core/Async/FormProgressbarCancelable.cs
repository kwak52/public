using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Common.Utilities
{
    public class FormProgressbarCancelable : FormProgressbar, IFormProgressbarCancelable
    {
        public static new IFormProgressbarCancelable StartProgressbar(string title, string label = null)
        {
            var form = new FormProgressbarCancelable() { ProgressCaption = title, ProgressDescription = label };
            form.Show();
            return form;
        }
    }
}
