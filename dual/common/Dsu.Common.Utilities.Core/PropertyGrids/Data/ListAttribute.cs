// http://www.codeproject.com/Articles/23242/Property-Grid-Dynamic-List-ComboBox-Validation-and

using System;

namespace Dsu.Common.Utilities.PropertyGrids
{
	[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property )]
	public abstract class ListAttribute : Attribute
	{
	}
}