all:
	mcs GaudiFileSystemWatcher.cs \
		-r:bin/Dsu.Common.Utilities.FS.dll,bin/Dsu.Kefico.CCS.FS.dll,bin/MySql.Data.dll,System.Data

	mv GaudiFileSystemWatcher.exe bin/


#	mcs GaudiFileSystemWatcher.cs
#	xbuild /p:Configuration=Release GaudiFileSystemWatcher.csproj

exe:
	mono bin/GaudiFileSystemWatcher.exe `pwd`

clean:
	rm -rf bin obj GaudiFileSystemWatcher.exe


none:
	bin/Dsu.Common.Utilities.Core.dll
	bin/Dsu.DB.FS.dll
	bin/FSharp.Core.dll
	bin/Google.ProtocolBuffers.dll
	bin/PsCommon.dll
	bin/PsCpUtility.dll
	bin/PsKGaudi.dll

