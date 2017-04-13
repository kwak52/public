// http://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and

using System;
using System.Collections;
using System.ComponentModel;

namespace Dsu.Common.Utilities.PropertyGrids
{
	class ListPropertyDescriptor<C, T> : PropertyDescriptor where C : IList
	{
		private readonly T _dataObject;

		public ListPropertyDescriptor( string name, T dataObject )
			: base( name, null )
		{
			this._dataObject = dataObject;
		}


		public override bool CanResetValue( object component )
		{
			return ( false );
		}

		public override Type ComponentType
		{
			get { return ( typeof( C ) ); }
		}

		public override object GetValue( object component )
		{
			return ( this._dataObject );
		}

		public override bool IsReadOnly
		{
			get { return ( true ); }
		}

		public override Type PropertyType
		{
			get { return ( this._dataObject.GetType() ); }
		}

		public override void ResetValue( object component )
		{
			// Nothing to do
		}

		public override void SetValue( object component, object value )
		{
			// Nothing to do
		}

		public override bool ShouldSerializeValue( object component )
		{
			return ( false );
		}
	}
}
