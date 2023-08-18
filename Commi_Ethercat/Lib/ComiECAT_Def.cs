//#define BitFieldUsing
using System;
using System.Runtime.InteropServices;


// Date : 2020.08.20
// Ver : 1.0.0
namespace ComiLib.EtherCAT
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public partial class SafeNativeMethods
    {
        public enum EEcVerCompatResult
        {
            ecVER_MISMATCH_HIGHER = -2,	// 해당 파일의 버전이 기준이 되는 파일과 호환되지 않는 상위 버전임을 나타냄
            ecVER_MISMATCH_LOWER = -1,	// 해당 파일의 버전이 기준이 되는 파일과 호환되지 않는 하위 버전임을 나타냄
            ecVER_NOT_FOUND = 0,	// 해당 파일의 버전을 찾을 수 없음(파일이 없거나, 버전 정보를 얻을 수 없는 경우)
            ecVER_MATCH = 1,	// 해당 파일의 버전이 기준이 되는 파일과 호환이 되는 버전임을 나타냄 
            ecVER_INVALID
        }

        public enum EEcFastFuncType
        {
            ecFF_TYPE1 = 1,
            ecFF_TYPE2 = 2
        }


        public enum EEcEcatCmd
        {
            ecCMD_APRD = 0x01,
            ecCMD_APWR = 0x02,
            ecCMD_FPRD = 0x04,
            ecCMD_FPWR = 0x05,
            ecCMD_BRD = 0x07,
            ecCMD_BWR = 0X08,
            ecCMD_LRD = 0x0a,
            ecCMD_LWR = 0x0b,
            ecCMD_LRW = 0x0c,
            ecCMD_ARMW = 0x0d,
            ecCMD_FRMW = 0x0e
        }

        public enum EEcSlvInfoType
        {
            ecSLV_INFO_VENDOR_ID = 0,
            ecSLV_INFO_PROD_CODE = 1,
            ecSLV_INFO_REV_NO = 2,
            ecSLV_INFO_SER_NO = 3
        }

        public enum EEcAlState
        {
            DISCONNECTED = 0,
            INITIAL = 0x1,
            PREOP = 0x2,
            BOOTSTRAP = 0x3,
            SAFEOP = 0x4,
            OP = 0x8,
            ERROR = 0x10,
            INITIAL_ERROR = INITIAL | ERROR,
            PREOP_ERROR = PREOP | ERROR,
            SAFEOP_ERROR = SAFEOP | ERROR,
        }

        // OutPDO 데이터의 초기값 셋팅 모드 (OP모드로 전환될 때 셋팅되는 초기값) //
        public enum EEcOPDOInitMode
        {
            ecOPDO_INIT_KEEPLAST, // 마지막의 출력값을 그대로 유지한다.
            ecOPDO_INIT_ZERO, // 모든 값을 0으로 셋팅한다.
            ecOPDO_INIT_USERDEF, // 사용자가 미리 정의한 초기값을 적용한다.
            ecOPDO_INIT_INVALID, // 
        }

        // 디지털 I/O 채널 표현 형식 //
        public enum EEcIoChanType
        {
            ecIOCH_TYPE_GLOBAL, ///< Global Channel 방식 (슬레이브 구분없이 연속된 채널 번호를 부여하여 표현하는 방식)
            ecIOCH_TYPE_LOCAL,	///< Local Channel 방식 (슬레이브를 지정하고, 해당 슬레이브 내에서의 로컬 채널 번호로 표현하는 방식
            ecIOCH_TYPE_ONBOARD, ///< Onboard Cahnnel (마스터보드내에서 제공하는 I/O 채널을 표현) 
            ecIOCH_TYPE_UNDEF	///< 채널형식이 특별히 지정되지 않았음을 의미함.
        };


        // Debug Logging Type //
        public enum EEcDlogType
        {
            ecDLOG_TYPE_FILE = 0, // Text file에 로그 결과를 기록하는 모드 
            ecDLOG_TYPE_TRACE = 1,  //Debug 스트링 뷰어를 통해서 로그 결과를 보여 주는 모드 
            ecDLOG_TYPE_COMITOOL = 2,  // COMIZOA에서 제작한 디버깅 툴을 사용하여 로그 결과를 보여주는 모드 (현재 지원되지 않으며 항후 지원 예정)
            ecDLOG_TYPE_MEMORY = 3, // 메모리에 로그하고 필요시에 덤프하는 모드
            ecDLOG_TYPE_INVALID
        }

        // Debug Logging Level //
        public enum EEcDlogLevel
        {
            ecDLOG_LEVEL_DISALBE = 0, // No debug logging
            ecDLOG_LEVEL_ERR = 1,   // 에러 발생했을 때만 로깅 
            ecDLOG_LEVEL_CMD = 2,   // ecDLOG_LEVEL_ERR레벨을 포함하며 더해서 Set 함수 및 각종 커맨드 함수 로깅 
            ecDLOG_LEVEL_GET = 3,    // ecDLOG_LEVEL_CMD레벨을 포함하며 더해서 Get 함수도 로깅 (단 일부 상태 체크 함수들은 제외)
            ecDLOG_LEVEL_ALL = 4,   // 모든 함수에 대해서 로깅 수행 
            ecDLOG_LEVEL_INVALID
        }

        // Debug Logging Level //
        public enum EEcDlogErrLogType
        {
            ecDLOG_ERR_LOG_NONE = 0, // 에러가 발생하더라도 로깅을 남기지 않는다. 
            ecDLOG_ERR_LOG_CMD = 1,  // Command 함수에서만 에러가 발생했을 때 로깅을 남긴다.
            ecDLOG_ERR_LOG_GET = 2,  // 모든 함수에 대해서 에러가 발생했을 때 로깅을 남긴다.
        }

        // InPDOQue 기능에서 Queueing start/stop trigger 모드 종류에 대한 아이디값 //
        public enum EEcQueTrgMode
        {
            ecTRG_MODE_NONE, // 트리거모드 사용 안함.
            ecTRG_MODE_COMMON, // 모든 채널이 공통 트리거 소스를 사용함. 이 때에는 TInPDOQueChannel::Cfg.StaTrg.pCommonTrgSig 포인터가 가리키는 Signal 변수의 값이 1이 되면 
            ecTRG_MODE_COUNT, // InputPDO 데이터 전송 이벤트 카운트를 이용해서 트리거함. 이 때에 Count 조건 값은 TInPDOQueChannel::Cfg.StaTrg.TrgRefVal 멤버 변수를 통해서 설정한다. 
            ecTRG_MODE_PDODATA, // 특정 PDO 데이터의 값을 체크하여 트리거 조건을 결정하는 모드. 
            ecTRG_MODE_INVALID
        }

        public enum EEcQueCommTrgMode
        {
            ecCOMTRG_MODE_NONE, // 트리거모드 사용 안함.
            ecCOMTRG_MODE_MANUAL, // Manual trigger 모드 
            ecCOMTRG_MODE_COUNT, // InputPDO 데이터 전송 이벤트 카운트를 이용해서 트리거함. 이 때에 Count 조건 값은 TInPDOQueChannel::Cfg.StaTrg.TrgRefVal 멤버 변수를 통해서 설정한다. 
            ecCOMTRG_MODE_PDODATA, // 특정 PDO 데이터의 값을 체크하여 트리거 조건을 결정하는 모드. 
            ecCOMTRG_MODE_INVALID
        }

        // InPDOQue 기능에서 Start/Trigger 사용할 때 데이터를 비교하는 방법의 종류에 대한 아이디값 //
        public enum EEcTrgLevelType
        {
            ecTRG_LEV_TYPE_EQ, // 비교데이터가 Ref 데이터와 같으면(Equal) 트리거 셋
            ecTRG_LEV_TYPE_GT, // 비교데이터가 Ref 데이터보다 크면(Greater than) 트리거 셋
            ecTRG_LEV_TYPE_GE, // 비교데이터가 Ref 데이터보다 크거나 같으면 트리거 셋
            ecTRG_LEV_TYPE_LT, // 비교데이터가 Ref 데이터보다 작으면(Less than) 트리거 셋
            ecTRG_LEV_TYPE_LE, // 비교데이터가 Ref 데이터보다 작거나 같으면 트리거 셋
            ecTRG_LEV_TYPE_RE, // 비교데이터가 Ref 데이터를 기준으로 상승에지(Rising edge, 비교데이터의 한 샘플 전/후 값이 Ref값 보다 작은 상태에서 큰 상태로 변화한 경우)가 발생하면 트리거 셋
            ecTRG_LEV_TYPE_FE, // 비교데이터가 Ref 데이터를 기준으로 하강에지(Rising edge, 비교데이터의 한 샘플 전/후 값이 Ref값 보다 큰 상태에서 작은 상태로 변화한 경우)가 발생하면 트리거 셋
            ecTRG_LEV_TYPE_INVALID
        }

        // Axis Connection Status Detailed ID //
        public enum EEcmConnStsDetail
        {
            ecmCONN_STS_NOCONFIG = -1, // 해당축 번호로 맵핑된 Slave가 Network Configuration에서 정의되지 않음.
            ecmCONN_STS_DISCONN = 0, // 해당축의 통신이 끊어진 상태
            ecmCONN_STS_INITIAL = 1, // 해당축의 통신이 연결되어 있으며, AL State가 INITIAL 단계임.
            ecmCONN_STS_PREOP = 2, // 해당축의 통신이 연결되어 있으며, AL State가 PreOP 단계임.
            ecmCONN_STS_BOOTSTRAP = 3, // 해당축의 통신이 연결되어 있으며, AL State가 BOOTSTRAP 단계임.
            ecmCONN_STS_SAFEOP = 4, // 해당축의 통신이 연결되어 있으며, AL State가 SafeOP 단계임.
            ecmCONN_STS_OP = 8  // 해당축의 통신이 연결되어 있으며, AL State가 OP 단계임.
        }

        public enum EEcAiDataType
        {
            ecAI_DT_INT,    // signed integer
            ecAI_DT_UINT,   // unsigned integer
            ecAI_DT_FLT,    // floating point data
            ecAI_DT_INVALID
        }

        public enum EEcAoDataType
        {
            ecAI_DT_INT,    // signed integer
            ecAI_DT_UINT,   // unsigned integer
            ecAI_DT_FLT,    // floating point data
            ecAI_DT_INVALID
        }

        public enum EEcmOperMode
        {
            ecmOPMODE_HM = 6,
            ecmOPMODE_CP = 8, // cyclic synchronous position mode
            ecmOPMODE_CV = 9, // cyclic synchronous velocity mode
            ecmOPMODE_CT = 10 // cyclic synchronous torque mode
        }

        // Motion Property ID //
        public enum EEcmMioPropId
        {
            ecmMPID_EL_STOP_MODE = 0, ///< External Limit 센서 감지 시의 정지 모드 [ 0:즉시정지(Default), 1:감속정지]
            ecmMPID_SWL_STOP_MODE = 10, ///< Software Limit 감지 시의 정지 모드: [ 0:즉시정지(Default), 1:감속정지]
            ecmMPID_ALM_STOP_MODE = 20, ///< Servo Alarm 감지 시의 정지 모드: [ 0:즉시정지(Default), 1:감속정지]
            ecmMPID_INP_ENABLE = 30, ///< 서보드라이버의 INP 신호의 모션의 완료 상태 체크에 반영할 것인지 설정. [ 0:Disable(Default), 1:Enable]
            ecmMPID_SVOFF_MOVE_MODE = 40, ///< 서보 OFF 상태에서의 이송 허용 모드(0:허용 안함(또한 이송 중에 Servo-off 불가), 1:허용함)
            ecmMPID_CLEAR_INITIAL_ALARM = 50, ///< ECAT AL State가 OP모드로 전환될 때 1회에 한해서 서보앰프의 알람을 클리어할 것인지에 대한 설정. [0:Disable, 1:Enable(Default)]
            ecmMPID_INPUTPDO_TYPE = 60, ///< 각 축의 InputPDO데이터를 마스터장치에서 PC측으로 전달할 때의 데이터 구조를 결정.
            ecmMPID_IGNORE_ELN = 70, ///< (-)Limit 신호를 무시하라는 옵션에 대한 설정. [ 0:Disable(Default), 1:Enable]
            ecmMPID_IGNORE_ELP = 71, ///< (+)Limit 신호를 무시하라는 옵션에 대한 설정. [ 0:Disable(Default), 1:Enable]
            ecmMPID_ELN_INPUT_SEL = 72, ///< (-)Limit 신호로 사용할 신호 입력 핀을 설정하는 옵션 [ 0:NOT신호(Default), 1:SIMON1/EXT1신호, 2:SIMON2/EXT2신호, 3:SIMON3, 4:SIMON4, 5:SIMON5] <= 파나소닉서보드라이버 기준
            ecmMPID_ELP_INPUT_SEL = 73, ///< (+)Limit 신호로 사용할 신호 입력 핀을 설정하는 옵션 [ 0:POT신호(Default), 1:SIMON1/EXT1신호, 2:SIMON2/EXT2신호, 3:SIMON3, 4:SIMON4, 5:SIMON5] <= 파나소닉서보드라이버 기준
            ecmMPID_NULL
        }

        public enum EEcmHomeOptID
        {
            ecmHOID_TPROB_EDGE_SEL = 0, ///< Homming시에 사용되는 Touch Probe의 Trigger Edge 설정.
            ecmHOID_NULL
        }

        // Signal logic //
        public enum EEcmSigLogic
        {
            ecmLOGIC_A = 0,
            ecmLOGIC_B = 1
        }

        // Trigger Signal Edge //
        public enum EEcmSigEdge
        {
            ecmEDGE_NEG = 0, // Positive Edge
            ecmEDGE_POS = 1	 // Negative Edge	
        }

        public enum EEcmDir
        {
            ecmDIR_N = 0,
            ecmDIR_P = 1
        }

        // Speed Mode //
        public enum EEcmSpeedMode
        {
            ecmSMODE_CONST = 0,
            ecmSMODE_TRAPE = 1,
            ecmSMODE_SCURVE = 2,
            ecmSMODE_INVALID
        }

        //  S-Curve 가감속 프로파일을 사용할 때에 Jerk를 설정하는 방법 //
        public enum EEcmdJerkSetMode
        {
            ecmJERK_MODE_LR1,	///< Linear Section 비율을 정의하여 Jerk를 설정하는 모드 (가속도 설정값이 전체 가속 구간의 시간을 결정하도록 하는 인자로 사용되는 모드)
            ecmJERK_MODE_LR2,	///< Linear Section 비율을 정의하여 Jerk를 설정하는 모드 (가속도 설정값이 가속구간에서의 최대가속도값으로 적용되는 모드)
            ecmJERK_MODE_JT,	///< Jerk Time을 정의하여 Jerk를 설정하는 모드 
            ecmJERK_MODE_JV,	///< Jerk Value를 정의하여 Jerk를 설정하는 모드 
            ecmJERK_INVALID
        }

        // 'SyncOther' 모드에서 Sync. Type 에 대한 ID //
        public enum EEcmSyncOtherType
        {
            ecmSYNC_OTHER_START,
            ecmSYNC_OTHER_ACC_INI,
            ecmSYNC_OTHER_ACC_END,
            ecmSYNC_OTHER_DEC_INI,
            ecmSYNC_OTHER_DEC_END,
            ecmSYNC_OTHER_POSITION,
            ecmSYNC_OTHER_INVALID
        }

        // 'SyncOther' 모드에서 Sync. Type 이 ecmSYNC_OTHER_POSITION인 경우에 Position을 비교하는 방법 종류에 대한 ID //
        public enum EEcmPosSyncMethod
        {
            ecmPOS_SYNC_GT, // sync축의 position이 ref. position보다 큰(Greater Than) 경우.
            ecmPOS_SYNC_GE, // sync축의 position이 ref. position보다 크거나 같은(Greater or Equal) 경우.
            ecmPOS_SYNC_LT, // sync축의 position이 ref. position보다 작은(Less Than) 경우.
            ecmPOS_SYNC_LE, // sync축의 position이 ref. position보다 크거나 같은(Less or Equal) 경우.
            ecmPOS_SYNC_POS_CROSS, // sync축의 position이 ref. position보다 작은 값에서 큰 값으로 변하는 경우.
            ecmPOS_SYNC_NEG_CROSS, // sync축의 position이 ref. position보다 큰 값에서 작은 값으로 변하는 경우.
            ecmPOS_SYNC_INVALID
        }

        public enum EEcmAutoTorqValMode
        {
            ecmATRQ_VAL_SINGLE, // single torque value output mode
            ecmATRQ_VAL_MULTI,  // Multiple level torque value output mode
            ecmATRQ_VAL_INVALID
        }

        public enum _EEcmAtrqLimMask
        {
            ecmATRQ_LMBIT_HIGHSPD, // single torque value output mode
            ecmATRQ_LMBIT_LOWSPD,  // Multiple level torque value output mode
            ecmATRQ_LMBIT_TIME
        }

        // Counter name //
        public enum EEcmCntr
        {
            ecmCNT_COMM, // 최종좌표계 Command 위치 (기계적 원점, 사용자 좌표계 원점, PosCorrTable이 모두 적용된 이후의 위치)
            ecmCNT_FEED, // 최종좌표계 Feedback 위치 (기계적 원점, 사용자 좌표계 원점, PosCorrTable 적용이 된 이후의 위치)
            ecmCNT_COMM_CORR, // PosCorrTable 적용전 사용자좌표계 Command 위치 (기계적 원점과 사용자좌표계 원점이 적용되고, PosCorrTable은 적용되지 않은 위치)
            ecmCNT_FEED_CORR, // PosCorrTable 적용전 사용자좌표계 Feedback 위치 (기계적 원점과 사용자좌표계 원점이 적용되고, PosCorrTable은 적용되지 않은 위치)
            ecmCNT_COMM_MOTOR, // 모터좌표계 Command 위치 (모터에서 전달받은 그대로의 위치값. 기계적 원점 적용하기 전의 위치)
            ecmCNT_FEED_MOTOR // 모터좌표계 Command 위치 (모터에서 전달받은 그대로의 위치값. 기계적 원점 적용하기 전의 위치)
        }

        // Motion State //
        public enum EEcmMotStateId
        {
            STOP = 0,
            ACCEL = 1,
            WORK_SPEED = 2,
            DECEL = 3,
            INIT_SPEED = 4,
            WAIT_INP = 5,
            SD_SPEED = 6,
            HOMMING = 10,
            SLAVE_MODE = 13,  ///<해당 축이 Master/Slave 기능의 Slave축으로 동작하는 상태
            WAIT = 14,
            AUTO_TORQ_MDOE = 15,
            PTMOTION = 16,
            NORMAL_TORQ_MODE = 17, ///< Normal Torque Control Mode가 진행중인 상태
            MULTORQ1_MODE = 18, ///< Multi-Torque1 기능이 활성화된 상태
            NORMAL_CV_MODE = 21  ///< Normal cyclic velocity control mode 가 진행중인 상태
        }

        // Interpolation Map Index //
        public enum EEcmIxMap
        {
            ecmIX_MAP0 = 0,
            ecmIX_MAP1 = 1,
            ecmIX_MAP2 = 2,
            ecmIX_MAP3 = 3,
            ecmIX_MAP4 = 4,
            ecmIX_MAP5 = 5,
            ecmIX_MAP6 = 6,
            ecmIX_MAP7 = 7,
            ecmIX_MAP8 = 8,
            ecmIX_MAP9 = 9,
            ecmIX_MAP10 = 10,
            ecmIX_MAP11 = 11,
            ecmIX_MAP12 = 12,
            ecmIX_MAP13 = 13,
            ecmIX_MAP14 = 14,
            ecmIX_MAP15 = 15,
        }

        // interpolation mode(using to IxMapAxes) //
        public enum EEcmIxMODE
        {
            ecmIX_MODE_NONE = 0,
            ecmIX_MODE_LINEAR = 1,
            ecmIX_MODE_CIRCULAR = 2,
            ecmIX_MODE_HELICARL = 3,
            ecmIX_MODE_SPLINE = 4,
        }

        public enum EEcmArcDir
        {
            ecmARC_CW = 0,
            ecmARC_CCW = 1
        }

        public enum EEcmSplJsType
        {
            ecmJSTYPE_DISABLED = 0,
            ecmJSTYPE_USE_JERKTHRESH = 1,
            ecmJSTYPE_INVALID,
        }

        public enum EEcmSplJsPropId
        {
            ecmJSPROP_DEC_TIME_ms, // 저속으로 감속할때의 감속 시간을 msec 단위로 설정한다.
            ecmJSPROP_LOWVEL_RATIO, // 저속구간의 속도를 고속구간 속도에 대한 비율값으로 설정한다.
            ecmJSPROP_LOWVEL_DUR_ms,// 저속구간의 유지시간을 msec단위로 설정한다. 
            ecmJSPROP_INVALID
        }

        // Round Data Type ID //
        public enum EEcmRoundDataType
        {
            NONE = 0,
            RADIUS = 1,
            OFFSET = 2,
            INVALID
        }


        // Round Position Type ID (MoveVia2X에서 사용) //
        public enum EEcmRoundPosType
        {
            ecmROUND_PT_START = 0, // 경유점에서 원호가 시작됨
            ecmROUND_PT_END = 1, // 경유점에서 원호가 종료됨
            ecmROUND_PT_INVALID
        }


        public enum EEcmLmQueFullMode
        {
            ecmLM_QUEFULL_SKIP,
            ecmLM_QUEFULL_WAIT,
            ecmLM_QUEFULL_INVALID,
        }

        // 하나의 리스트모션맵의 현재 동작 상태 아이디 //
        public enum EEcmLmSts
        {
            ecmLM_STS_DISABLED,	// List Motion 기능이 비활성화된 상태 (LmCtl_Begin() 실행하기 전)
            ecmLM_STS_PAUSED,	// List Motion 기능이 활성화(LmCtl_Begin()) 되었지만 아직 Run (LmCtl_Run()) 되지 않은 상태
            ecmLM_STS_RUN,		// List Motion 기능이 활성화(LmCtl_Begin()) 되고, 또한  Run (LmCtl_Run()) 된 상태.
            ecmLM_STS_STOPPING, // List Motion 정지 명령(ecmLmCtl_Stop)이 하달되어서 정지 작업을 진행하고 있는 상태(감속중)
            ecmLM_STS_STEPCOMPTING, // IsComptCurStep인자를 true로 하여  List Motion 정지 명령(ecmLmCtl_Stop)이 하달되어서 그 시점에 진행되고 있던 스텝은 그대로 진행하고 있는 상태임을 나타냄
            ecmLM_STS_STANDBY,		// List Motion 기능이 Run된 상태에서 등록된 모든 스텝을 완료한 상태
            ecmLM_STS_INVALID
        }

        // 리스트모션 각 커맨드 아이템의 실행 상태 //
        public enum EEcmLmCmdItemSts
        {
            ecmLM_CMDITEM_STS_READY, 		// 아직 실행되지 않음
            ecmLM_CMDITEM_STS_BUSY,			// 현재 실행되고 있음.
            ecmLM_CMDITEM_STS_PAUSED,		// 실행 중에(완료되기 전에) 중지됨.
            ecmLM_CMDITEM_STS_COMPLETED,	// 해당 커맨드의  실행이 완료되었음.
            ecmLM_CMDITEM_STS_INVALID
        }

        // 하나의 리스트모션맵의 현재 동작 상태 아이디 //
        public enum EEcmPtmSts
        {
            ecmPTM_STS_DISABLED,	// PT-Motion 기능이 비활성화된 상태 (ecmPtmCtl_Begin() 실행하기 전)
            ecmPTM_STS_PAUSED,		// PT-Motion 기능이 활성화(ecmPtmCtl_Begin()) 되었지만 Hold된 상태
            ecmPTM_STS_RUN_IDLE,	// PT-Motion 기능이 활성화(ecmPtmCtl_Begin()) 되고 Hold상태도 아니지만 아직 등록된 커맨드가 하나도 없는 경우.
            ecmPTM_STS_RUN_BUSY,	// PT-Motion 기능이 활성화(ecmPtmCtl_Begin()) 되고 Hold상태도 아닌 상태에서 등록된 커맨드가 실행중에 있는 경우.
            ecmPTM_STS_RUN_COMPT,	// PT-Motion 기능이 활성화(ecmPtmCtl_Begin()) 되고 Hold상태도 아닌 상태에서 등록된 커맨드가 모두 실행된 경우(참고로 ecmPTM_STS_RUN_COMPT은 1개 이상의 등록된 커맨드가 있었던 경우에 해당하며, 등록된 커맨드가 아예 없는 경우는 ecmPTM_STS_RUN_IDLE 상태로 반환된다)
            ecmPTM_STS_INVALID
        }

        public enum EEcmPtmCmdItemSts
        {
            ecmPTM_CMDITEM_STS_READY, 		// 아직 실행되지 않음
            ecmPTM_CMDITEM_STS_BUSY,			// 현재 실행되고 있음.
            ecmPTM_CMDITEM_STS_PAUSED,		// 실행 중에(완료되기 전에) 중지됨.
            ecmPTM_CMDITEM_STS_COMPLETED,	// 해당 커맨드의  실행이 완료되었음.
            ecmPTM_CMDITEM_STS_INVALID
        }

        // EEcmRingCntrDir: RingCounter 모드가 활성화된 축에 대해서 절대 좌표 이송 명령이 내려졌을 때 이송방향을 결정하는 Direction Mode의 아이디값 //
        // 참고: 링카운터 모드에서는 지정한 위치를 (-)방향으로 이송해서 갈수도 있고, (+)방향으로 이송해서 갈 수도 있다.
        public enum EEcmRingCntrDir
        {
            ecmRING_DIR_NEG,	// (-)방향으로 이송하여 지정한 절대 위치로 이송
            ecmRING_DIR_POS,	// (-)방향으로 이송하여 지정한 절대 위치로 이송
            ecmRING_DIR_NEAR,	// (-)방향과 (+)방향 중에서 현재 위치로부터 지정한 절대 위치까지 거리가 더 가까운 방향으로 이송.
            ecmRING_DIR_FAR,	// (-)방향과 (+)방향 중에서 현재 위치로부터 지정한 절대 위치까지 거리가 더 먼 방향으로 이송.
            ecmRING_DIR_INVALID
        }

        // Position Compare 기능의 Counter Type //
        public enum EEcmCmpCntrType
        {
            ecmCMP_CNTR_COMM,
            ecmCMP_CNTR_FEED,
            ecmCMP_CNTR_INVALID
        }

        // EEcmCmpMethod: Compare Method for 'Position Compare Output' function //
        public enum EEcmCmpMethod
        {
            ecmCMP_METH_EQ_NDIR,	// (-)방향 이송 중에 CP == RP 일때 Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_EQ_PDIR,	// (+)방향 이송 중에 CP == RP 일때 Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_EQ_BIDIR,	// 방향에 상관없이 CP == RP 일때 Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_LT,			// CP < RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_GT,			// CP > RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_LE,			// CP <= RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_GE,			// CP >= RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
        }

        public enum EEcmMTQ1CmpMode
        {
            ecmCMP_METH_LT,			// CP < RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_GT,			// CP > RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_LE,			// CP <= RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
            ecmCMP_METH_GE,			// CP >= RP 일때  Compare Output Active 되는 모드. 여기서 CP:Current Position,  RP:Reference Position.
        }

        public enum EEcmHandlerType
        {
            ecmHT_DISABLE,	        // Handler를 사용하지 않는다(현재 등록된 핸들러는 삭제된다)
            ecmHT_MESSAGE,	        // Window Message 방식
            ecmHT_EVENT,			// Event 방식
            ecmHT_CALLBACK,			// Callback 방식
        }

        public enum EEcmSdOfsMode
        {
            ecmSD_OFS_NONE, // offset 적용 안함
            ecmSD_OFS_TIME, // time offset 적용(msec 단위)
            ecmSD_OFS_CMDPOS, // command position offset 적용
            ecmSD_OFS_FEEDPOS, // feedback position offset 적용
            ecmSD_OFS_INVALID
        }




        /// <summary>
        /// ComiEcatSdk_SysDef.h
        /// </summary>
        /// 

        public enum EMPMonSectID
        {
            MPMSECT_ACC, ///< 가속구간만을 누적하여 계산하는 구간
            MPMSECT_DEC, ///< 감속구간만을 누적하여 계산하는 구간
            MPMSECT_CONST, ///< 정속구간만을 누적하여 계산하는 구간
            MPMSECT_MOVING, ///< 이송중인구간을 종합하여 계산하는 구간
            MPMSECT_IDLE, ///< 정지해 있는 구간만 누적하여 계산하는 구간
            MPMSECT_TOTAL, ///< 모든 구간을 모두 누적하여 계산
            MPMSECT_INVALID
        };



        // EEcFwuDnldCmd: FW Download 함수를 수행할 때 매개변수로 전달되는 명령들의 ID //
        public enum EEcFwuDnldCmd
        {
            ecFWU_DNLD_START, // download 시작하라는 커맨드
            ecFWU_DNLD_CONTINUE, // 일반적인 download 데이터 전송 커맨드
            ecFWU_DNLD_LAST, // 마지막 데이터의 download 전송 커맨드
            ecFWU_DNLD_END // download 종료 커맨드
        }


        public enum EEcFwuUpldCmd
        {
            ecFWU_UPLD_FILEINFO, // FileInfo 정보만 업로드하는 커맨드
            ecFWU_UPLD_FILEDATA // File data를 업로드하는 커맨드
        }


        // EEcEcatCmd: COE SDOInfo Service 중에서 'OD List Get' 서비를 이용할 때 Get하는 Obect Dictionary List의 종류를 나타내는 ID값들을 정의한 것 //
        public enum EEcSdoiODListType
        {
            ecODLT_LEN_LIST = 0x0, // 각 OD List Type(1~5)에 대한 List의 크기에 대한 리스트(이때에는 리스트 아이템이 2바이트 정수 5개로 구성된다)
            ecODLT_ALL_OBJ = 0x1, // Object Dictionary에 있는 모든 Object의 Index 리스트 
            ecODLT_RXPDO_OBJ = 0x2, // RxPDO에 맵핑 가능한 Object들의 리스트 
            ecODLT_TXPDO_OBJ = 0x3, // TxPDO에 맵핑 가능한 Object들의 리스트 
            ecODLT_STORE_OBJ = 0x4, // 디바이스를 교체할 때 저장되어야만 하는 Object들의 리스트 
            ecODLT_STARTUP_OBJ = 0x5, // startup parameter로 사용되는 Object들의 리스트 
            ecODLT_INVALID
        }


        //============================================================================================
        //  Structures
        //============================================================================================

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RPMData
        {
            public byte isAlarm;
            public ushort rpmValue;
        }


        // COMIZOA EtherCAT 마스터 디바이스 정보 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcDevInfo
        {
            public ushort ProdId; // Product ID: 0xA55F(LX550), 0xA550(LX551), 0xA551(LX552), 0xA552(LX554)
            public ushort PhysId; // Board에 장착되어 있는 스위치에 의해서 결정되는 ID
            public ushort NumNets; // 해당 디바이스 하나에서 제공하는 네트워크 갯수 (1 또는 2)
            public ushort NetIdx; // 해당 디바이스의 첫번째 네트워크가 전체 네트워크 리스트상에서 차지하는 순서. 
            public ushort FwVerMinor; // Firmware version(minor)
            public ushort FwVerMajor; // Firmware version(major)
        }


        // File Version Info //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFileVersion
        {
            public ushort MajorVer;
            public ushort MinorVer;
            public ushort BuildNo;
            public ushort RevNo;
        }


        // SDK(DLL) File Version & Compatibility Info //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFileVerInfo_SDK
        {
            public TEcFileVersion CurVer;			// Current SDK File Version
            public TEcFileVersion CompWdmVer;	// WDM driver File(.sys) Version which is Compatible with SDK DLL
            public TEcFileVersion CompFwVer;	// Firmwar[Struct dsasdsadsade File Version which is Compatible with SDK DLL
            public int nWdmCompResult;		// WDM driver compatibity result, 'EEcVerCompatResult' enum 선언 참조
            public int nFwCompResult;		// Firmware compatibity result, 'EEcVerCompatResult' enum 선언 참조
        }


        // WDM(.sys) File Version & Compatibility Info //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFileVerInfo_WDM
        {
            public TEcFileVersion CurVer;			// Current WDM driver File(.sys) Version
            public TEcFileVersion CompSdkVer;	// SDK(DLL) File Version which is Compatible with WDM driver
            public TEcFileVersion CompFwVer;	// Firmware File Version which is Compatible with WDM driver
            public int nSdkCompResult;		// SDK compatibity result, 'EEcVerCompatResult' enum 선언 참조
            public int nFwCompResult;		// Firmware compatibity result, 'EEcVerCompatResult' enum 선언 참조
        }


        // Firmware File Version & Compatibility Info //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFileVerInfo_FW
        {
            public TEcFileVersion CurVer;			// Current WDM driver File(.sys) Version
            public TEcFileVersion CompSdkVer;	// SDK(DLL) File Version which is Compatible with Firmware
            public TEcFileVersion CompWdmVer;	// WDM driver File(.sys) Version which is Compatible with Firmware
            public int nSdkCompResult;		// SDK(DLL) compatibity result, 'EEcVerCompatResult' enum 선언 참조
            public int nWdmCompResult;		// WDM driver compatibity result, 'EEcVerCompatResult' enum 선언 참조
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcInQueCommonTrg
        {
            public sbyte TrgMode; // 트리거 모드, EEcQueCommTrgMode enum 데이터 참고
            public int TrgLevelType; // 트리거 레벨 타입, ETrgLevelType enum 데이터 참고 
            public ushort wReseved; // reserved field (사용 안함)
            public short TrgDataPdoPos; // (TrgMode==TRG_MODE_PDODATA)인 경우에 트리거 소스로 사용되는 데이터의 PDO 메모리 상의 offset 위치  
            public short TrgDataSize;
            public ushort TrgDataMask; // (TrgMode==TRG_MODE_PDODATA)인 경우에 사용하는 마스킹 값으로서, 트리거 데이터에 이 값을 마스킹하여 트리거 데이터로 사용한다. 트리거 데이터가 특정비트 값을 사용하는 경우에 사용한다. 단, 이 값이 0이면 무시됨.
            public int TrgRefVal; // Trigger의 비교대상 기준값. (TrgMode==TRG_MODE_COUNT)인 경우에는 Count 리미트값으로 사용된다. 
            public int Delay; // 트리거조건이 만족한 이후에 실제 Trg 행위를 하기 전까지 지연이 필요한 경우에 이 값을 사용한다. 이때 Delay의 단위는 PDO전송 횟수이다.
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcInQueTrgCfg
        {
            public int TrgMode; // 트리거 모드, EEcQueTrgMode enum 데이터 참고
            public int TrgLevelType; // 트리거 레벨 타입, EEcTrgLevelType enum 데이터 참고. (TrgMode==TRG_MODE_PDODATA)인 경우에만 사용함 
            public ushort wReseved; // reserved field (사용 안함)
            public short TrgDataPdoPos; // 트리거 소스로 사용되는 데이터의 PDO 메모리 상의 offset 위치. (TrgMode==TRG_MODE_PDODATA)인 경우에만 사용함
            public short TrgDataSize; // 트리거 소스로 사용되는 데이터의 데이터형 크기를 바이트 단위로 나타낸다. (TrgMode==TRG_MODE_PDODATA)인 경우에만 사용함
            public uint TrgDataMask; // 트리거 데이터에 이 값을 마스킹하여 트리거 데이터로 사용한다. 트리거 데이터가 특정비트 값을 사용하는 경우에 사용한다. 단, 이 값이 0이면 무시됨. (TrgMode==TRG_MODE_PDODATA)인 경우에만 사용함.
            public int TrgRefVal; // (TrgMode==TRG_MODE_PDODATA)인 경우 => Trigger의 비교대상 기준값. (TrgMode==TRG_MODE_COUNT)인 경우 => 이 값을 Count 리미트값으로 사용한다. 
            public int Delay; // 트리거조건이 만족한 이후에 실제 Trg 행위를 하기 전까지 지연이 필요한 경우에 이 값을 사용한다. 이때 Delay의 단위는 PDO전송 횟수이다.
            public int CommonTrgHandle; // (TrgMode == TRG_MODE_COMMON(공통트리거)) 인 경우에만 사용하는 멤버로서, 공통 트리거 신호 관리하는 오브젝트의 핸들을 나타낸다.
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcInQueDataDesc
        {
            public ushort PdoOfsPos; // 해당 데이터가 위치하는 Input PDO 논리 메모리상의 OFFSET 주소값 
            public ushort DataSize; // 해당 데이터의 크기(바이트 단위);
        }


        // TEcSlvProdInfo: 하나의 슬레이브에 대한 Product ID 정보 // 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSlvProdInfo
        {
            public uint VendId;
            public uint ProdCode;
            public uint RevNo;
            public uint SerNo;
        }


        // TEcSlvProdDesc: 하나의 슬레이브에 대한 Product 정보를 스트링으로 나타내는 구조체 // 
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSlvProdDesc
        {
            // buffer declaration //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szVendName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szDevName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] szDevPhysics;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szGroupType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] szURL;
        }


        // TEcSlvTypeInfo: Slave 모듈의 장치 형식에 대한 정보를 제공하는 구조체.
        // 여기서 제공하는 장치 형식은 DI, DO, AI, AO, Servo-Axis 의 채널에 대한 정보를 말한다.
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSlvTypeInfo
        {
            // Digital Input Function Info. //

            public DI DiInfo;
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct DI
            {
                public short StaGlobChannel; // start global channel no.
                public ushort NumChannels; // number of channels
            }
            // Digital Output Function Info. //

            public DO DoInfo;
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct DO
            {
                public short StaGlobChannel; // start global channel no.
                public ushort NumChannels; // number of channels
            }

            // Analog Input Function Info. //
            public AI aiInfo;
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct AI
            {
                public short StaGlobChannel; // start global channel no.
                public ushort NumChannels; // number of channels
            }

            // Analog Input Function Info. //
            public AO aoInfo;
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct AO
            {
                public short StaGlobChannel; // start global channel no.
                public ushort NumChannels; // number of channels
            }

            public Servo servoInfo;
            // Servo Motor Function Info. //
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Servo
            {
                public short StaAxisNo; // start global channel no.
                public ushort NumAxes; // number of channels
            }
            // Reserved for future //
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public int[] Resv;
        }



        // Homing 관련 상태 플래그들 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmHomeSt_Flags
        {
#if BitFieldUsing
            [BitFieldInfo(0, 1)]    public bool Moving { get; set; } // 1 - 모터가 구동중임을 나타냄. 단, 이 플래그는 Homing이 아닌 다른 일반 이송중일때도 1로 된다.
            [BitFieldInfo(1, 1)]    public bool HomeBusy { get; set; }// 1 - Homing 동작이 진행중임을 나타냄. 
            [BitFieldInfo(2, 1)]    public bool HomeAttained { get; set; }// 1 - Homing 동작이 성공적으로 완료되었음을 나타냄.
            [BitFieldInfo(3, 1)]    public bool HomingError { get; set; }// 1 - Homing 동작 중에 에러가 발생함.
#else
            public ushort word;
            public ushort Moving { get { return SafeNativeMethods.GetBit(word, 0); } }    // 1 - 모터가 구동중임을 나타냄. 단, 이 플래그는 Homing이 아닌 다른 일반 이송중일때도 1로 된다.
            public ushort HomeBusy { get { return SafeNativeMethods.GetBit(word, 1); } }    // 1 - Homing 동작이 진행중임을 나타냄. 
            public ushort HomeAttained { get { return SafeNativeMethods.GetBit(word, 2); } }    // 1 - Homing 동작이 성공적으로 완료되었음을 나타냄.
            public ushort HomingError { get { return SafeNativeMethods.GetBit(word, 3); } }    // 1 - Homing 동작 중에 에러가 발생함.
#endif
        }


        // Motion InputPDO Group의 헤더 정보: 이 정보가 Motion InputPDO 영역의 맨처음에 자리하게 된다. //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmInPDO_Header
        {
            private uint data;
            public uint HEmgState
            {
                get { return data & 0x1; }
                set { data = data & (value & 0x1); }
            }

            public uint SEmgState
            {
                get { return (data >> 1) & 0x1; }
                set { data = (data & 0xFFFFFFFD) | ((value & 0x1) << 1); }
            }

            public uint Resv
            {
                get { return (data >> 2); }
                set { data = (data & 0x3) | (value << 2); }
            }

            //t_ui32	HEmgState:	1; ///< Motion Hard-EMG 상태
            //t_ui32	SEmgState:	1; ///< Motion Soft-EMG 상태
            //t_ui32	Resv:	30;
        }


        /////////////////////////////////////////////////////////////////////////////////////
        // Flag: Bit 별로 Axis의 상태를 알리는 플래그들의 집합
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmSxSt_Flags
        {
#if BitFieldUsing

            public ushort word;

            [BitFieldInfo(0, 1)]    public bool RdyToSwOn { get; set; }            
            [BitFieldInfo(1, 1)]    public bool SwOn { get; set; }
            [BitFieldInfo(2, 1)]    public bool OperEnabled { get; set; }
            [BitFieldInfo(3, 1)]    public bool ServoFault { get; set; }
            [BitFieldInfo(4, 1)]    public bool VoltEnabled { get; set; }
            [BitFieldInfo(5, 1)]    public bool QuickStop { get; set; }
            [BitFieldInfo(6, 1)]    public bool SwOnDisabled { get; set; }
            [BitFieldInfo(7, 1)]    public bool ServoWarn { get; set; }
            [BitFieldInfo(8, 1)]    public bool CtlrFault { get; set; }
            [BitFieldInfo(9, 1)]    public bool HomeError { get; set; }
            [BitFieldInfo(10, 1)]   public bool OMS1 { get; set; }
            [BitFieldInfo(11, 1)]   public bool IntLimit { get; set; }
            [BitFieldInfo(12, 1)]   public byte OMS2 { get; set; }
            [BitFieldInfo(13, 1)]   public bool HomeBusy { get; set; }
            [BitFieldInfo(15, 1)]   public bool HomeAttained { get; set; }
#else
            public ushort word;
            public ushort RdyToSwOn { get { return SafeNativeMethods.GetBit(word, 0); } }
            public ushort SwOn { get { return SafeNativeMethods.GetBit(word, 1); } }
            public ushort OperEnabled { get { return SafeNativeMethods.GetBit(word, 2); } }
            public ushort ServoFault { get { return SafeNativeMethods.GetBit(word, 3); } }
            public ushort VoltEnabled { get { return SafeNativeMethods.GetBit(word, 4); } }
            public ushort QuickStop { get { return SafeNativeMethods.GetBit(word, 5); } }
            public ushort SwOnDisabled { get { return SafeNativeMethods.GetBit(word, 6); } }
            public ushort ServoWarn { get { return SafeNativeMethods.GetBit(word, 7); } }
            public ushort CtlrFault { get { return SafeNativeMethods.GetBit(word, 8); } }
            public ushort HomeError { get { return SafeNativeMethods.GetBit(word, 9); } }
            public ushort OMS1 { get { return SafeNativeMethods.GetBit(word, 10); } }
            public ushort IntLimit { get { return SafeNativeMethods.GetBit(word, 11); } }
            public ushort OMS2 { get { return SafeNativeMethods.GetBit(word, 12); } }
            public ushort HomeBusy { get { return SafeNativeMethods.GetBit(word, 14); } }
            public ushort HomeAttained { get { return SafeNativeMethods.GetBit(word, 15); } }
#endif
        }


        // Digital Input of an Axis //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmSxSt_DI
        {
#if BitFieldUsing
        [StructLayout(LayoutKind.Explicit)]
        public struct bit
        {
            [FieldOffsetAttribute(0)]
            public ushort NOT;
            [FieldOffsetAttribute(1)]
            public ushort POT;
            [FieldOffsetAttribute(2)]
            public ushort HOME;
            [FieldOffsetAttribute(3)]
            public ushort INP;
            [FieldOffsetAttribute(4)]
            ushort b4;
            [FieldOffsetAttribute(5)]
            ushort b5;
            [FieldOffsetAttribute(6)]
            ushort b6;
            [FieldOffsetAttribute(7)]
            ushort b7;
            [FieldOffsetAttribute(8)]
            ushort b8;
            [FieldOffsetAttribute(9)]
            ushort b9;
            [FieldOffsetAttribute(10)]
            ushort b10;
            [FieldOffsetAttribute(11)]
            ushort b11;
            [FieldOffsetAttribute(12)]
            ushort b12;
            [FieldOffsetAttribute(13)]
            ushort b13;
            [FieldOffsetAttribute(14)]
            ushort b14;
            [FieldOffsetAttribute(15)]
            ushort b15;
        }
#endif
            public ushort word;
            public ushort NOT { get { return SafeNativeMethods.GetBit(word, 0); } }
            public ushort POT { get { return SafeNativeMethods.GetBit(word, 1); } }
            public ushort HOME { get { return SafeNativeMethods.GetBit(word, 2); } }
            public ushort INP { get { return SafeNativeMethods.GetBit(word, 3); } }
            public ushort b4 { get { return SafeNativeMethods.GetBit(word, 4); } }
            public ushort b5 { get { return SafeNativeMethods.GetBit(word, 5); } }
        }


        // Motion InputPDO에서 하나의 Axis의 Status 정보 구조체 형식 0 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmInPDO_AxisType0
        {
            private ushort data;
            public ushort PDOTypeID
            {
                get { return (ushort)(data & 0xFF); }
                set { data = (ushort)((data & 0xFF00) | (value & 0xFF)); }
            }

            public ushort TouchProbSts1
            {
                get { return (ushort)((data >> 8) & 0x7); }
                set { data = (ushort)((data & 0xF8FF) | ((value & 7) << 8)); }
            }

            public ushort TouchProbSts2
            {
                get { return (ushort)((data >> 11) & 0x7); }
                set { data = (ushort)((data & 0xC7FF) | ((value & 7) << 11)); }
            }

            public ushort SdInputSts //< SD 센서 입력 상태
            {
                get { return (ushort)((data >> 14) & 0x1); }
                set { data = (ushort)((data & 0xBFFF) | ((value & 1) << 14)); }
            }

            public ushort Dir
            {
                get { return (ushort)((data >> 15) & 0x1); }
                set { data = (ushort)((data & 0x7FFF) | ((value & 1) << 15)); }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            // Mst: Motion의 상태를 알리는 코드값. 이 값은 Mio의 일부 비트 상태에 따라서 그 의미가 달라진다.
            // 1) Mio.ServoFault=1인 경우: Servo driver의 알람코드를 나타낸다.
            // 2) Mio.CtlrFault=1인경우: Controller의 알람코드를 나타낸다.
            // 3) Mio.ServoFault=0, Mio.CtlrFault=0 인 경우: Motion Control 동작 상태 코드
            // 참고) Mio.ServoFault 와 Mio.CtlrFault 가 둘다 1인 경우에는 Servo driver의 알람코드를 나타낸다.	
            public short Mst;
            // Motion Status Flags //
            public TEcmSxSt_Flags Flags;
            //// Digital Input //
            public TEcmSxSt_DI DI;

            // Position 정보 //
            public int CmdCnt, FeedCnt;
            // 속도 정보 //
            public int CmdVel, FeedVel;
        }


        // Motion InputPDO에서 하나의 Axis의 Status 정보 구조체 형식 1 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmInPDO_AxisType1
        {
            private ushort data;
            public ushort PDOTypeID
            {
                get { return (ushort)(data & 0xFF); }
                set { data = (ushort)((data & 0xFF00) | (value & 0xFF)); }
            }

            public ushort TouchProbSts1
            {
                get { return (ushort)((data >> 8) & 0x7); }
                set { data = (ushort)((data & 0xF8FF) | ((value & 7) << 8)); }
            }

            public ushort TouchProbSts2
            {
                get { return (ushort)((data >> 11) & 0x7); }
                set { data = (ushort)((data & 0xC7FF) | ((value & 7) << 11)); }
            }

            public ushort SdInputSts //< SD 센서 입력 상태
            {
                get { return (ushort)((data >> 14) & 0x1); }
                set { data = (ushort)((data & 0xBFFF) | ((value & 1) << 14)); }
            }

            public ushort Dir
            {
                get { return (ushort)((data >> 15) & 0x1); }
                set { data = (ushort)((data & 0x7FFF) | ((value & 1) << 15)); }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            // Mst: Motion의 상태를 알리는 코드값. 이 값은 Mio의 일부 비트 상태에 따라서 그 의미가 달라진다.
            // 1) Mio.ServoFault=1인 경우: Servo driver의 알람코드를 나타낸다.
            // 2) Mio.CtlrFault=1인경우: Controller의 알람코드를 나타낸다.
            // 3) Mio.ServoFault=0, Mio.CtlrFault=0 인 경우: Motion Control 동작 상태 코드
            // 참고) Mio.ServoFault 와 Mio.CtlrFault 가 둘다 1인 경우에는 Servo driver의 알람코드를 나타낸다.	
            public short Mst;
            // Motion Status Flags //
            public TEcmSxSt_Flags Flags;
            // Digital Input //
            public TEcmSxSt_DI DI;
            // Position 정보 //
            public int CmdCnt, FeedCnt;
            // 속도 정보 //
            public int CmdVel, FeedVel;
            public short Torque;
        }


        // Motion InputPDO에서 하나의 Axis의 Status 정보 구조체 형식 2 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmInPDO_AxisType2
        {
            private ushort data;
            public ushort PDOTypeID
            {
                get { return (ushort)(data & 0xFF); }
                set { data = (ushort)((data & 0xFF00) | (value & 0xFF)); }
            }

            public ushort TouchProbSts1
            {
                get { return (ushort)((data >> 8) & 0x7); }
                set { data = (ushort)((data & 0xF8FF) | ((value & 7) << 8)); }
            }

            public ushort TouchProbSts2
            {
                get { return (ushort)((data >> 11) & 0x7); }
                set { data = (ushort)((data & 0xC7FF) | ((value & 7) << 11)); }
            }

            public ushort SdInputSts //< SD 센서 입력 상태
            {
                get { return (ushort)((data >> 14) & 0x1); }
                set { data = (ushort)((data & 0xBFFF) | ((value & 1) << 14)); }
            }

            public ushort Dir
            {
                get { return (ushort)((data >> 15) & 0x1); }
                set { data = (ushort)((data & 0x7FFF) | ((value & 1) << 15)); }
            }
            ////////////////////////////////////////////////////////////////////////////////////
            // Mst: Motion의 상태를 알리는 코드값. 이 값은 Mio의 일부 비트 상태에 따라서 그 의미가 달라진다.
            // 1) Mio.ServoFault=1인 경우: Servo driver의 알람코드를 나타낸다.
            // 2) Mio.CtlrFault=1인경우: Controller의 알람코드를 나타낸다.
            // 3) Mio.ServoFault=0, Mio.CtlrFault=0 인 경우: Motion Control 동작 상태 코드
            // 참고) Mio.ServoFault 와 Mio.CtlrFault 가 둘다 1인 경우에는 Servo driver의 알람코드를 나타낸다.	
            public short Mst;
            // Motion Status Flags //
            public TEcmSxSt_Flags Flags;
            // Digital Input //
            public TEcmSxSt_DI DI;
            // Position 정보 //
            public int CmdCnt, FeedCnt;
            // 속도 정보 //
            public int CmdVel, FeedVel;
            public short Torque;
            public int TouchProbPos1;
        }


        // Motion InputPDO에서 하나의 Axis의 Status 정보 구조체 형식 3 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmInPDO_AxisType3
        {
            private ushort data;
            public ushort PDOTypeID
            {
                get { return (ushort)(data & 0xFF); }
                set { data = (ushort)((data & 0xFF00) | (value & 0xFF)); }
            }

            public ushort TouchProbSts1
            {
                get { return (ushort)((data >> 8) & 0x7); }
                set { data = (ushort)((data & 0xF8FF) | ((value & 7) << 8)); }
            }

            public ushort TouchProbSts2
            {
                get { return (ushort)((data >> 11) & 0x7); }
                set { data = (ushort)((data & 0xC7FF) | ((value & 7) << 11)); }
            }

            public ushort SdInputSts //< SD 센서 입력 상태
            {
                get { return (ushort)((data >> 14) & 0x1); }
                set { data = (ushort)((data & 0xBFFF) | ((value & 1) << 14)); }
            }

            public ushort Dir
            {
                get { return (ushort)((data >> 15) & 0x1); }
                set { data = (ushort)((data & 0x7FFF) | ((value & 1) << 15)); }
            }

            ////////////////////////////////////////////////////////////////////////////////////
            // Mst: Motion의 상태를 알리는 코드값. 이 값은 Mio의 일부 비트 상태에 따라서 그 의미가 달라진다.
            // 1) Mio.ServoFault=1인 경우: Servo driver의 알람코드를 나타낸다.
            // 2) Mio.CtlrFault=1인경우: Controller의 알람코드를 나타낸다.
            // 3) Mio.ServoFault=0, Mio.CtlrFault=0 인 경우: Motion Control 동작 상태 코드
            // 참고) Mio.ServoFault 와 Mio.CtlrFault 가 둘다 1인 경우에는 Servo driver의 알람코드를 나타낸다.	
            public short Mst;
            // Motion Status Flags //
            public TEcmSxSt_Flags Flags;
            // Digital Input //
            public TEcmSxSt_DI DI;
            // Position 정보 //
            public int CmdCnt, FeedCnt;
            // 속도 정보 //
            public int CmdVel, FeedVel;
            public short Torque;
            public int TouchProbPos1;	// TouchProbe1 positive edge position
            public int TouchProbPos1_n;	// TouchProbe1 negative edge position
            public int TouchProbPos2;	// TouchProbe2 positive edge position
            public int TouchProbPos2_n;	// TouchProbe2 negative edge position
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TMPMonSectData
        {
            public Torq torq;
            public PosDev posDev;
            public struct Torq
            {
                public ushort Max; ///< 최대 토크 (0.1% 단위. 즉 1은 0.1%를 의미한다)
                public ushort Avg; ///< 평균 토크 (0.1% 단위. 즉 1은 0.1%를 의미한다) 
                public uint Dummy; ///< 64비트 정렬을 위한 dummy data
                public ulong ThreshCount; ///< 문턱값(threshold)을 넘어서는 토크값이 발생한 카운트
            }

            public struct PosDev
            {
                public uint Max; ///< 최대 위치편차
                public uint Avg; ///< 평균 위치편차
                public ulong ThreshCount; ///< 문턱값(threshold)을 넘어서는 위치편차값이 발생한 카운트
            }
            public ulong DataCount; ///< 해당 구간의 누적시간
        }


        public struct TMPMonData
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public TMPMonSectData[] Section;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcDateTime
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek; // 이 필드는 사용 안할 수 있음
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds; // 이 필드는 사용 안할 수 있음
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFwuFileInfo
        {
            // ecFWU_SIGNATURE	= 0x56981532; 
            public uint Signature; // 파일의 유효성을 나타내기 위해 사용하는 signature 0x56981532
            public uint FileSize;
            public TEcDateTime FileTime;
            public TEcDateTime DnldTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] Resv;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFwuStatus
        {
            // State 
            // bit 1: IsAbort, 
            // bit 3 : resv
            // bit 4 : phase (0 : idle. 1 : Downloading. 2 : Uploading. 3 : Verifying ) 
            // bit 4 : state (0 : before job, 1 : busy. 2 : normally complete(OK). 3 : Abnormally complete (Error)
            public ushort state;
            public ushort iProgress; // 진행율: 0.01% 단위의 값을 정수값으로 표현        
            public uint targSize;
            public uint doneSize;
        }


        // TEcSdoiODList: COE SDO Object Dictionary List 데이터 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSdoiODList
        {
            public ushort SturctType; // 구조체 타입: TEcSdoiODList, TEcSdoiObjDesc, TEcSdoiEntryDesc 의 세가지 구조체에 대한 메모리 해제를 할 때 ecGn_ReleaseSdoInfoData() 함수 하나를 통해서 수행하므로 전달된 구조체의 타입을 파악하기 위해서 필요하다.
            public ushort ListType;
            public ushort NumItems;
            private UIntPtr Items;
        }


        // TEcSdoiODList: COE SDO Object Description 데이터 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSdoiObjDesc
        {
            public ushort SturctType; // 구조체 타입: TEcSdoiODList, TEcSdoiObjDesc, TEcSdoiEntryDesc 의 세가지 구조체에 대한 메모리 해제를 할 때 ecGn_ReleaseSdoInfoData() 함수 하나를 통해서 수행하므로 전달된 구조체의 타입을 파악하기 위해서 필요하다.
            public ushort ObjIndex; // Object Index
            public ushort DataType; // Data Type of the object
            public byte MaxSubIdx; // Maximum number of sub indices
            public byte ObjCode;	 // Object Code: 7-Variable, 8-Array, 9-Record
            private UIntPtr pObjName; // Name of the object //ysy 확인필요 / t_char *pObjName; // Name of the object
        }


        // TEcSdoiODList: COE SDO Object Entry Description 데이터 //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSdoiEntryDesc
        {
            public ushort SturctType; // 구조체 타입: TEcSdoiODList, TEcSdoiObjDesc, TEcSdoiEntryDesc 의 세가지 구조체에 대한 메모리 해제를 할 때 ecGn_ReleaseSdoInfoData() 함수 하나를 통해서 수행하므로 전달된 구조체의 타입을 파악하기 위해서 필요하다.
            public ushort ObjIndex; // Object Index
            public byte SubIndex;
            public byte ValueInfo; // 비트별로 pEntryData 값에 담기는 데이터의 종류를 나타내는데, 이 값이 0이면 pEntryData에는 SubIndex의 Object 이름이 담겨진다.
            public ushort DataType; // 해당 Object의 data type index
            public ushort BitLen; // bit length of the object
            public ushort ObjAccess; // 어떤 AL State에서 해당 object를 access할 수 있는지를 나타내는 값.
            private UIntPtr pEntryData; // Entry Description Data    //ysy 확인필요 / t_byte *pEntryData; // Entry Description Data    
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmMTQ1Item
        {
            public short TorqVal; // 조건이 만족되었을 때 출력될 target torque value. 이 값의 단위는 0.1% 이다.
            public ushort RefAxis; // 조건 만족을 판단할 때 감시 대상이 되는 축 번호.
            public int RefCmpMode; // 조건 만족을 판단하는 방법. 이 값에 적용 가능한 값의 종류는 0-LT, 1-GT, 2-LE, 3-GE 이다(EEcmMT1CmpMode 선언 참조)
            public double RefPos; // 조건 만족 검사를 위한 기준 위치
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcmPC2DHeader
        {
            // Target Axis Information //
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct TA
            {
                public double StartPos; // 첫 번째 열(Column)의 기준 위치 값
                public float StepSize; // 각 열(Column)의 기준 위치값의 거리 간격. 예를 들어서 두 번째 열의 기준 위치값은 (StartPos+StepSize)가 되고, 세 번째 열의 기준 위치값은 (StartPos+2*StepSize)가 되는 것이다.
                public uint StepCount; // 대상축의 티칭 포인트 갯수. 이 값은 결국 열(Column)의 갯수를 의미한다.
            };

            // Reference Axis information //
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct RA
            {
                public uint Axis; // 참조축(Ref. Axis)의 번호
                public double StartPos; // 첫 번째 행(Row)에 해당하는 참조축의 기준 위치 값
                public float StepSize; // 각 행(Row)의 기준 위치값의 거리 간격. 예를 들어서 두 번째 행의 참조축 기준 위치값은 (StartPos+StepSize)가 되고, 세 번째 행의 참조축 기준 위치값은 (StartPos+2*StepSize)가 되는 것이다.
                public uint StepCount; // 참조축의 티칭 포인트 갯수. 이 값은 결국 행(Row)의 갯수
            }

            public TA TargetAxis;
            public RA ReferenceAxis;
        }

        public struct TEcLogicAddr
        {
            public ushort AddrOfs; ///< Offset address
            public ushort BitIdx; ///< AddrOfs이 가리키는 영역의 바이트내에서 원하는 데이터가 위치한 bit index를 나타낸다. 
        }

        public struct TEcLogBitAddr
        {
            private uint data;

            public uint AddrOfs
            {
                get { return data & 0x3FFF; }
                set { data = (data & 0xFFFFC000) | (value & 0x3FFF); }
            }

            public uint ChanType
            {
                get { return ((data >> 14) & 0x3); }
                set { data = (data & 0xFFFFCFFF) | ((value & 3) << 14); }
            }

            public uint BitIdx
            {
                get { return (data >> 16); }
                set { data = (data & 0x0000FFFF) | ((value & 0xFFFF) << 16); }
            }
            //public uint AddrOfs;  //:	14; ///< 논리메모리(PDO영역) 상에서의 Offset address
            //public uint ChanType; //:	2;	///< LogBitAddr로 변환할 때 사용한 채널 형식,  0-Global, 1-Local, 2-Onboard, 3-Undefined 이와 관련하여 EEcIoChanType 선언 참고
            //public uint BitIdx;   //: 16; ///< AddrOfs이 가리키는 영역의 바이트내에서 원하는 데이터가 위치한 bit index를 나타낸다. 
        };

        public struct TEcLogBitAddrU
        {
            public uint d;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct TEcmEmgInputEnv
        {
            [FieldOffsetAttribute(0)]
            public uint LogBitAddr; // 논리비트주소(Logic Bit Address). 이 데이터의 구조는 TEcLogBitAddr 구조체와 같다.
            [FieldOffsetAttribute(4)]
            public bool InvertLogic; // 입력 신호의 로직을 반전하여 사용할지를 설정한다.  0:입력된 신호값을 그대로 사용.  1:입력된 신호의값을 반전하여 사용	
            [FieldOffsetAttribute(5)]
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte Resv0; // 현재는 사용하지 않는 예비 변수
            [FieldOffsetAttribute(6)]
            public byte Resv1;
            [FieldOffsetAttribute(7)]
            public byte Resv2;
        };

        public struct HEmgInputEnv
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public TEcmEmgInputEnv[] emgInputEnv;
        }

        //public static Func<dynamic, int, int, dynamic> SetBit = (v, n, s) => s == 1 ? v |= (1 << n) : v &= ~(1 << n);
        public static Func<ushort, int, ushort> GetBit = (v, n) => (ushort)((v >> n) & 0x1);


        public const int ecMAX_NUM_NET_PER_DEVICE = 2;
        public const int ecMAX_NUM_ONBOARD_INPUT = 8; // Onboard Digital Input 채널 갯수
        public const int ecMAX_NUM_ONBOARD_OUTPUT = 4; // Onboard Digital Output 채널 갯수
        public const int ecMAX_NUM_SLAVES = 1000;
        public const int ecMAX_NUM_AXES_PER_NET = 64;  // 하나의 Network에서 지원하는 최대 모션제어 축 수 
        public const int ecMAX_NUM_IX_AXES = 32;  // 하나의 보간맵에 맵핑할 수 있는 최대 축 수 
        public const int ecmMAX_NUM_MOT_IX_MAP = 32; ///< 최대 정의 가능한 보간 맵 갯수
        public const int ecMAX_NUM_InQUE_DATA_DESC = 32; ///< 하나의 InQUE채널에 할당할 수 있는 데이터 종류의 최대 갯수
        public const int ecmMAX_NUM_LIST_MOT_MAP = 8;
        public const int ecmMAX_NUM_PT_MOT_MAP = 8;
        public const int ecmMAX_NUM_AXES_PER_NET = ecMAX_NUM_AXES_PER_NET; ///< 최대 정의 가능한 보간 맵 갯수
        public const int ecmMAX_NUM_IX_AXES = ecMAX_NUM_IX_AXES;
        public const int ecmMAX_NUM_SPLINE_AXES = 8;
        public const int ecmMAX_NUM_SPLINE_OBJ = 100;
        public const int ecmMAX_NUM_SPLINE_INPUT_POINTS = 10000;
        public const int ecmMAX_NUM_EMG_INPUTS = 8;   // 최대 설정 가능한 EmgStop Input 채널 수
        public const int ecmMAX_NUM_COLLAS = 16;
        public const int ecmMAX_NUM_ATRQ_MULTIVALS = 1000;  // 최대 설정 가능한 AutoTorq 모드의 Multi-Value Items 갯수 
        public const int ecmMAX_POSCORR_TABLE_SIZE = 10000; //< 각 축별로 할당할 수 있는 Position Correction Table의 최대 크기                                                                      ///
        public const int ecmMAX_CMPCONT_TABLE_SIZE = 32768; //
        public const int ecmDEFAULT_LM_QUE_DEPTH = 50000; // default list motion queue depth
        public const int ecMASTER_PHYSADDR = 0xffff; // default list motion queue depth
    }
}
