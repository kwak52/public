// http://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and

using System.ComponentModel;
using System.Text;

namespace Dsu.Common.Utilities.PropertyGrids
{
	public class OrderedDisplayNameAttribute : DisplayNameAttribute
	{
		public OrderedDisplayNameAttribute( int position, int total, string displayName )
		{
			StringBuilder sb = new StringBuilder( displayName );

			for ( int index = position; index < total; index++ )
			{
				sb.Insert( 0, '\t' );
			}

			base.DisplayNameValue = sb.ToString();
		}
	}
}
