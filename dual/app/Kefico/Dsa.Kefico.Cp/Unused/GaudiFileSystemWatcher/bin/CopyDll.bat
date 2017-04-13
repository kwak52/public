for %%f in (
	Dsu.Common.Utilities.Core.dll
	Dsu.Common.Utilities.FS.dll
	Dsu.DB.FS.dll
	Dsu.Kefico.CCS.FS.dll
	FSharp.Core.dll
	MySql.Data.dll
	PsCommon.dll
	PsCpUtility.dll
	PsKGaudi.dll
) do (
	echo Copying %%f
	del %%f
	copy ..\..\Dsa.Kefico.CCS\bin\%%f .
)


pause
