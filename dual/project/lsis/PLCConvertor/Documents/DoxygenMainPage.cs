/*!
@mainpage main page
메인 페이지


@section intro PLC 변환기
옴론 PLC 를 LS 산전의 XG5000 용 PLC 로 변환하는 프로그램으로,
옴론 CX-Programmer 에서 텍스트 프로젝트 형식으로 저장된 cxt 파일을 읽어 들여서
변환을 수행한 후, 결과를 XG5000 의 qtx 파일 형식으로 출력한다.

변환 과정에 사용자에게 표현해야 할 내용은 메시지 파일(*.txt 형식)로 함께 출력되며,
출력된 PLC ladder 에 주석으로 변환 관련된 내용이 함께 출력된다.

CX-Programmer 버젼 WrittenByCXPVersion=9.70 을 기준으로 작성되었으며, cxt 파일에서 버젼을 따로 검사하지 않으므로
호환성이 유지되는 범위안에서의 변환은 유효하다.  예전의 cxt 버젼 (WrittenByCXPVersion=7.03)의 포맷까지 변환 가능함을 확인하였음.


@subsection ssp 프로젝트 / 어셈블리 구성
솔루션(PLCConvertor.sln)에 포함된 project(*.prj, assembly) 는 다음과 같다. 
- PLCConvertor.exe
 * PLC 변환기 프로그램
- AddressMapper.exe
 * 옴론 PLC 와 산전 XG5000 PLC 변환을 위한 메모리 주소 영역 매핑 프로그램
- Dsu.PLCConvertor.Common
 * PLC 변환기 프로그램 및 주소 매핑 프로그램에서 공용으로 사용하는 assembly
- Dsu.PLCConverer.UI
 * 주소 매핑 프로그램에서 사용되는 UI control (사용자 정의 user control 포함) 정의한 assembly
- Dsu.Common.LSIS
 * 일반적인 공용 utility
- ConvertUnitTest
 * 변환에 관련된 단위 test project
  - F#, <a href=https://fsprojects.github.io/FsUnit/ FsUnit> 으로 작성

*/

/* \mainpage Drawing Shapes
 *
 * This project helps user to draw shapes.
 * Currently two types of shapes can be drawn:
 * - \subpage drawingRectanglePage "How to draw rectangle?"
 *
 * - \subpage drawingCirclePage "How to draw circle?"
 *
 */

/* \page drawingRectanglePage How to draw rectangle?
 *
 * Lorem ipsum dolor sit amet
 *
 */

/* \page drawingCirclePage How to draw circle?
 *
 * This page is about how to draw a circle.
 * Following sections describe circle:
 * - \ref groupCircleDefinition "Definition of Circle"
 * - \ref groupCircleClass "Circle Class"
 */

/*
@subsection convertor       PLCConvertor.exe
@subsection mapper          AddressMapper.exe

@subsection convertorCommon	Dsu.PLCConvertor.Common
@subsection ssfw			Abstract Framework : CGpFrame
@subsection sscrypto		Encryption/Decription
- CGpCrypto
@subsection sslm			License Management
@subsection sspdt			Primitive Data Type support for COM
@subsection ssscripting		COM Automation/Scripting support
- <a href=http://dualsoft.co.kr/trac/plcstudio/ticket/165>PLCStudio Automation Support</a>
- <a href=http://dualsoft.co.kr/trac/plcstudio/ticket/215>Task Tree </a>
@subsection ssntp			Network Time Client using NTP(Network Time Protocol)
@subsection ssur			Undo/Redo support
@subsection ssfs			File system utility : CGpFileSpec
@subsection sscb			Clipboard backup utility : CGpClipboardBackup

@section introduction Introduction

GlApi(or PmApi)는 Physical Model을 기술하는 모듈이다.   주된 기능은 다음과 같다.
- OpenGL rendering
 * Scene graph
   - Node : CSgNode*
   - Core : CSgCore*
- Physical model 기술
 * Joint, Axis, Robot 정의 : CKin*, CPmKin*
 * Material Handling : CSgMh*
 * Sequence of OPeration :CSgSOP*




 */