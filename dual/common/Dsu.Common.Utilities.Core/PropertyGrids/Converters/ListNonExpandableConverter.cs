// http://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and

using System;
using System.ComponentModel;
using System.Globalization;

namespace Dsu.Common.Utilities.PropertyGrids
{
	public class ListNonExpandableConverter : CollectionConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destType )
		{
			if ( value is IDisplay )
			{
				return ( ( (IDisplay)value ).Text );
			}

			return ( string.Empty );
		}
	}
}

