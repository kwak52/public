:: 방화벽 열기.  http://snoopybox.co.kr/1627
netsh advfirewall firewall add rule name="MwsServicePort1" dir=in action=allow protocol=tcp localport=50001
netsh advfirewall firewall add rule name="MwsServicePort2" dir=in action=allow protocol=tcp localport=50002



:: 서비스 설치
::https://blogs.msdn.microsoft.com/ssehgal/2009/06/01/configuring-windows-services-using-command-prompt/

bin\MwsServiceApp.exe uninstall
bin\MwsServiceApp.exe install
sc config MWSWindowsService type= own type= interact depend= Winmgmt
sc failure MWSWindowsService actions= restart/180000/restart/180000/restart/180000 reset= 86400
net start MWSWindowsService 



:: netsh advfirewall firewall add rule name="MxPlcServicePort1" dir=in action=allow protocol=tcp localport=50011
:: bin\MxPlcServiceApp.exe uninstall
:: bin\MxPlcServiceApp.exe install
:: sc config MxPlcWindowsService type= own type= interact depend= Winmgmt
:: sc failure MxPlcWindowsService actions= restart/180000/restart/180000/restart/180000 reset= 86400
:: net start MxPlcWindowsService 

pause


