using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CpTesterPlatform.CpTStepDev.Interface
{
    public interface IMotion : IDevice
    {
        //string CONTROLLER_ADDRESS { set; get; }
        //string AXIS_ID { set; get; }
        //string AXIS_DIRECTION { set; get; }
        ////List<string> AXIS_ROBOT_MULTI { set; get; }

        ///// <summary>
        ///// ACC/DEACCELERATION TIME
        ///// </summary>
        //double CFG_ACC_RPM_RER_SEC { set; get; }
        //double CFG_DEC_RPM_RER_SEC { set; get; }
        //double CFG_MAX_RPM_RER_SEC { set; get; }
        //double CFG_DISTANCE_PER_REVOLUTION { set; get; }
        //bool AXIS_ROBOT { set; get; }
        ///// <summary>
        ///// Functions for Initialization of Each Axis 
        ///// </summary>
        ///// <param name="bState"></param>
        //void SetRpmSpeed(double dMoveVel, bool overrideSpeed = false);
        ///// <summary>
        ///// Functions SetRobotSpeed
        ///// </summary>
        ///// <param name="bState"></param>
        //void SetRobotSpeed(short axis, double MAX, double ACC, double DEC, bool overrideSpeed = false);

        ///// <summary>
        ///// Functions for Initialization of Each Axis Position
        ///// </summary>
        ///// <param name="dCmdPos"></param>
        //void SetCommandPosition(double dCmdPos);
        //void SetEncoderPosition(double dEncPos);

        ///// <summary>
        ///// Functions for Checking Motor Status
        ///// </summary>
        ///// <returns></returns>
        //bool IsReady();
        //Nullable<bool> IsMove(short axis);
        //bool IsInposition();

        ///// <summary>
        ///// 모터 방향
        ///// </summary>
        ///// <param name="bCW"></param>
        //void SetDirection(bool bCW);
        ///// <summary>
        ///// 상대 이동
        ///// 현재 위치로 부터 이동할 펄스
        ///// </summary>
        ///// <param name="dPos"></param>
        //void SetRelMove(double dPos);
        ///// <summary>
        ///// 상대 이동
        ///// 현재 위치로 부터 이동할 펄스 use robot only
        ///// </summary>
        ///// <param name="dPos"></param>
        //void SetRelMove(short axis, double dPos);
        ///// <summary>
        ///// 절대 이동
        ///// 0을 기준으로 이동할 펄스 위치
        ///// </summary>
        ///// <param name="dPos"></param>
        //void SetAbsMove(double dPos);
        ///// <summary>
        ///// 조그 이동
        ///// 정지 신호 전까지 연속 이동
        ///// </summary>
        //void SetJogMove();

        //IEnumerable<short> GetAxesList();
        //int GetCmdPos(short axis);
        //int GetEncPos(short axis);

        //int[] GetCmdPositions();
        //int[] GetEncPositions();
        ///// <summary>
        ///// CMD ENC '0' Setting for the given axes.
        ///// </summary>
        ///// <param name="axes">if null, use default axes set</param>
        //void SetCommandEncorderZeroSetting(IEnumerable<short> axes);
        ///// <summary>
        ///// CMD '0' Setting for the given axes.
        ///// </summary>
        ///// <param name="axes">if null, use default axes set</param>
        //void SetCommandZeroSetting(IEnumerable<short> axes);
        //void VerifiyCommandEncoderZero(IEnumerable<short> axes, int tolerance);
        //bool IsCommandEncoderZero(IEnumerable<short> axes, int tolerance);

        //void OpenManualDialog();
        //void SetModePath(List<string> paths);
        //List<string> GetPath();
        
        //Task<bool> MovePath(string pathName, CancellationToken cancelToken);
        //bool ClearAlarm();
        
        ///// <summary>
        ///// 지령 펄스 위치
        ///// </summary>
        ///// <returns></returns>
        //double GetCommandPos();
        ///// <summary>
        ///// 실제 펄스 위치
        ///// </summary>
        ///// <returns></returns>
        //double GetCurrentPos();
        ///// <summary>
        ///// 지령 펄스 속도
        ///// </summary>
        ///// <returns></returns>
        //double GetCommandRpm();
        ///// <summary>
        ///// 실제 펄스 속도
        ///// </summary>
        ///// <returns></returns>
        //double GetCurrentRpm();
        ///// <summary>
        ///// Stop Axis Operation
        ///// Emergency = true? -> Stop without Deaccelartion : Can give overload to the target motor.
        ///// </summary>
        ///// <param name="bEmergency"></param>
        //void StopMotion(bool bEmergency);
        //Task<bool> HomeMove(CancellationToken cancelToken);
    }
}
