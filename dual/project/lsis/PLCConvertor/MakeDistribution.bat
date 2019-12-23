bin\Dsu.PLCConvertor.DistributionChecker.exe
if %ERRORLEVEL% == 0 (
	echo ErrorLevel is zero
) else (
	echo ErrorLevel is Nonzero
	exit
)

rmdir Distribution
mkdir Distribution

xcopy Documents\ReleaseNote.txt Distribution\


xcopy bin Distribution\bin\ /sy
xcopy Documents\Manuals\UserManuals\UserManual.pptx Distribution\ /sy

pushd Distribution
del Dsu.PLCConvertor.DistributionChecker.exe
del xunit.*.dll
del log-convertor-*.txt
del dic.txt

	pushd bin
	for %%d in (hu ko ko-KR cs en fr it pl pt-BR tr zh-Hans zh-Hant Report) do (
		rd /s /q %%d
	)

	popd

popd


pushd Distribution\bin


:: ls -1 Akka.* DotNetty.* EnvDTE.* FSharp.* Google.* LanguageExt.Core.* log4net.* Microsoft.* MySql.Data.* Newton* Polly.* Quartz.* SQLite.Interop.* stdole.* System.* Topshelf.* WeifenLuo.* | sort >> ../../Distribution-Common-List.txt
for /F %%i IN (..\..\Distribution-Common-List.txt) DO del %%i
::del DevExpress.*


for %%d in (
	app.publish
	de es ja ru
) do (
	rd /s /q %%d
)


popd


pause
