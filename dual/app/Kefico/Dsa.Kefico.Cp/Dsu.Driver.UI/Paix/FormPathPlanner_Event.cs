using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsu.Driver.UI.Paix
{
    public partial class FormPathPlanner
    {
        private void PositionSelectionChanged()
        {
            var n = gridViewPosition.GetFocusedDataSourceRowIndex();
            if (n < 0)
            {
                _selectedPose = null;
                return;
            }

            _selectedPose = Poses[n];
            gridControlAxes.DataSource = Poses[n].SpeedSpec.AxesSpec;
        }
    }
}
