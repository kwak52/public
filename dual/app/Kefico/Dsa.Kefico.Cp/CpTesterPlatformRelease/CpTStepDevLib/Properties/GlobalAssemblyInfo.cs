/*
 * SVN commit 시에 assembly version 을 명기해야 Log.txt 에 해당 dll version 정보가 반영됩니다.
 *
 $Id: GlobalAssemblyInfo.cs 2572 2016-12-26 06:31:47Z brad-pitt $
 $Revision: 2572 $
 $Author: brad-pitt $
 $HeadURL: https://202.30.19.49/svn/MnS-Server/CPTester_K_2016/trunk/CpTesterPlatform/GlobalAssemblyInfo.cs $
 $Date: 2016-12-26 06:31:47 +0000 (?? 26 12 2016) $
*/

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyDescription("CP-Tester Component Dll")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Hyundai Kefico")]
[assembly: AssemblyCopyright("Copyright ⓒ  2016.12.26.")]
[assembly: AssemblyTrademark("Hyundai Kefico ME1: CP")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 표시되지 않습니다.  COM에서 이 어셈블리의 형식에 액세스하려면 
// 해당 형식에 대해 ComVisible 특성을 true로 설정하세요.
[assembly: ComVisible(false)]

// 이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
[assembly: Guid("df225b5c-433d-4c75-b7ef-4d22a2f216d8")]

// 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다.
//
//      주 버전
//      부 버전 
//      빌드 번호
//      수정 버전
//
// 모든 값을 지정하거나 아래와 같이 '*'를 사용하여 빌드 번호 및 수정 번호가 자동으로 
// 지정되도록 할 수 있습니다.
// [assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyInformationalVersion("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]
