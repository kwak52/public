using CpTesterPlatform.CpTStepFncBase.BaseClass;
using CpTesterPlatform.CpTStepFncBase.Interface;

namespace CpTesterPlatform.Functions
{
	/// <summary>
	/// Comparison between String1 and String2.
	/// Starting from the first character in each case,
	/// the two strings are compared character by character until a non-matching pair of characters is found or the end of one of the strings is reached.
	/// No distinction is made between upper and lower-case characters;
	/// lower-case characters are treated as upper-case. The function can return any of the following results.
	/// 0 String1 = String2
	/// 1 String1 > String2
	/// 2 String1 < String2
	/// '<' means that String1 precedes String2 alphabetically. 
	/// </summary>    

	public class CpFnSTRCMP : CpTsMacroMsShell, ISTRCMP
    {
		public CpFnSTRCMP()
		{
			 
		}
    }
}
