// http://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and

using System;

namespace Dsu.Common.Utilities.PropertyGrids.Rule
{
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public abstract class RuleBaseAttribute : Attribute
	{
		#region DataMember

		private string _errorMessage;

		#endregion

		#region Properties

		/// <summary>
		/// Get/Set for ErrorMessage
		/// </summary>
		public string ErrorMessage
		{
			get { return ( _errorMessage ); }
			protected set { _errorMessage = value; }
		}

		#endregion

		#region Methods - Public

		/// <summary>
		/// Validate the input data
		/// </summary>
		/// <param name="dataObject"></param>
		/// <returns></returns>
		public abstract bool IsValid( object dataObject );

		#endregion
	}
}
