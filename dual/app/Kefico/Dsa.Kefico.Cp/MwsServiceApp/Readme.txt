���� > services.msc

�������͸� Ȯ��
	HKLM > System > Current Control Set > Services > MWSWindowsService ���� ��ϵ� service Ȯ��
		http://smallvoid.com/article/winnt-services-regedit.html
		http://www.c-sharpcorner.com/uploadfile/0f68f2/adding-service-dependency-on-a-windows-service-through-regis/
		���θ���� > ���� ���ڿ� �� > DependOnService �̸����� Winmgmt �߰�


https://support.microsoft.com/ko-kr/kb/193888
#https://github.com/Th4nat0s/eventlog-to-syslog/issues/66
Netman
Winmgmt
Dhcp
Dnscache
iphlpsvc
RpcSs
RpcEptMapper
lmhosts
LanmanWorkstation
NlaSvc
Server
Network Location Awareness (NlaSvc)
Server (Server)
Network List Service (netprofm)


	http://hodorii.blogspot.kr/2011/11/blog-post_22.html#!

sc qc ServiceA
sc qc MWSWindowsService



���� Ÿ�Ӿƿ� ���� ���
https://support.microsoft.com/ko-kr/kb/922918



http://stackoverflow.com/questions/11978054/cannot-start-windows-service-in-networkservice-account/11983513#11983513
 - MWSSer

http://stackoverflow.com/questions/27940912/c-sharp-topshelf-timeoutexception