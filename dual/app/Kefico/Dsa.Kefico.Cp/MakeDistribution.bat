rmdir Distribution
mkdir Distribution

xcopy bin Distribution\bin\ /sy
xcopy Cpt.Winform Distribution\Cpt.Winform\ /sy
xcopy pruef_cp Distribution\pruef_cp\ /sy

xcopy InstallAsWindowsService.bat Distribution\ /y
xcopy Dsa.Kefico.Cp.sln Distribution\ /y

rd /s /q Distribution\Cpt.Winform\obj
rd /s /q Distribution\pruef_cp


pushd Distribution\bin
del logMwsService-*
del DevExpress.*.xml
del *.pdb
del Dsu.Driver.*
del CpMng* CpAppl* CpFunction* CpLog* CpMath* CpSample* CpSystem* CpTesterSs.* CpTStep* CpUtility.* CpXbarChart.*
del Dsa.NiDaq.* Dsu.PLC.* NationalInstruments.* DriverTest* TestConsole*
del log*.txt
rd /s /q Configure
rd /s /q Devices
rd /s /q MngDev
rd /s /q x86
rd /s /q x64
popd
