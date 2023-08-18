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
        const string dll = "ComiEcatSdk.dll";
        //====================== DLL LOAD/UNLOAD FUNCTIONS ======================================================//
        //EC_EXTERN t_success ecDll_Load (void);        
        //EC_EXTERN t_success ecDll_Unload (void);      
        //[DllImport("ComiEcatSdk.DLL", EntryPoint = "ecDll_Unload")]
        //EC_EXTERN BOOL		ecDll_IsLoaded(void);

        ////====================== GENERAL FUNCTIONS ==============================================================//SetVel

        //EC_EXTERN t_success (CECAT_API *ecGn_LoadDevices) (t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_LoadDevice(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_UnloadDevices) (t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_UnloadDevices(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN BOOL		(CECAT_API *ecGn_IsDevLoaded) (t_i32 DevIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern unsafe bool ecGn_IsDevLoaded(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_GetNumDevices) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecGn_GetNumDevices(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_GetNumNetworks) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecGn_GetNumNetworks(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecGn_GetDevInfo) (t_i32 DevIdx, TEcDevInfo *pDevInfo, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_GetDevInfo(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            ref TEcDevInfo pDevInfo,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecGn_SetEnableTimerResolSet) (t_bool IsEnable, t_i32 *ErrCode); 
        // 라이브러리 내부에서 Sleep()을 적용할 때 타이머 분해능을 1msec로 조절하고 Sleep()할 것인지를 설정한다. 0-1msec분해능 조정 없이 Sleep, 1(default값)-1msec분해능으로 조정하고 Sleep후 분해능 복원,  
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_SetEnableTimerResolSet(
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecGn_GetEnableTimerResolSet) (t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_GetEnableTimerResolSet(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //====================== GENERAL UNTILITY FUNCTIONS ============================================//
        //EC_EXTERN void		(CECAT_API *ecUtl_ShowError_A) (HWND hParentWnd, char *szFormat, ...);
        [DllImport(dll)]
        public static extern unsafe void ecUtl_ShowError_A(
            [MarshalAs(UnmanagedType.I4)] int hParentWnd,
            char[] szFormat);

        //EC_EXTERN void		(CECAT_API *ecUtl_ShowError_W) (HWND hParentWnd, WCHAR *szFormat, ...);
        [DllImport(dll)]
        public static extern unsafe void ecUtl_ShowError_W(
            [MarshalAs(UnmanagedType.I4)] int hParentWnd,
            [MarshalAs(UnmanagedType.LPWStr)] string szFormat);

        //EC_EXTERN void		(CECAT_API *ecUtl_ShowMessage_A) (HWND hParentWnd, WCHAR *szFormat, ...);
        [DllImport(dll)]
        public static extern unsafe void ecUtl_ShowMessage_A(
            [MarshalAs(UnmanagedType.I4)] int hParentWnd,
            ref char[] szFormat);

        //EC_EXTERN void		(CECAT_API *ecUtl_ShowMessage_W) (HWND hParentWnd, WCHAR *szFormat, ...);
        [DllImport(dll)]
        public static extern unsafe void ecUtl_ShowMessage_W(
            [MarshalAs(UnmanagedType.I4)] int hParentWnd,
            [MarshalAs(UnmanagedType.LPWStr)] string szFormat);

        //EC_EXTERN t_ui16	(CECAT_API *ecUtl_GetCntDiff_UI16) (t_ui16 PrvVal, t_ui16 CurVal);
        [DllImport(dll)]
        public static extern unsafe ushort ecUtl_GetCntDiff_UI16(
            [MarshalAs(UnmanagedType.U4)] uint PrvVal,
            [MarshalAs(UnmanagedType.U4)] uint CurVal);

        //EC_EXTERN t_bool	(CECAT_API *ecUtl_IsTimeOut) (t_ui32 dwStartTime_ms, t_ui32 dwTimeOutVal_ms);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecUtl_IsTimeOut(
            [MarshalAs(UnmanagedType.U4)] uint dwStartTime_ms,
            [MarshalAs(UnmanagedType.U4)] uint dwTimeOutVal_ms);
                
        public static string ecUtl_GetErrorString(int errorCode)
        {
            switch (errorCode)
            {
                case -5: return "ecERR_DEVICE_NOT_LOADED (지정된 디바이스가 로드되지 않은 경우)";
                case -6: return "ecERR_WDM_VER_READ_FAIL (WDM 드라이버에서 버전 정보를 읽는데 실패한 경우)";
                case -7: return "ecERR_FW_VER_READ_FAIL (Firmware 버전 정보를 읽는데 실패한 경우)";
                case -8: return "ecERR_DEV_BOOT_TIMEOUT (Master Device가 지정된 시   간 안에 부팅이 완료되지 않은 경우)";
                case -9: return "ecERR_DEV_BOOT_NOT_COMPT (Master Device가 아직 부트업이 완료되지 않은 경우)";
                case -10: return "ecERR_INVALID_BOARDID";
                case -11: return "ecERR_INVALID_DEVIDX";
                case -12: return "ecERR_INVALID_VERSION (SDK(DLL), WDM 드라이버, 펌웨어(Firmware) 상호간에 버전이 호환되지 않는 경우)";
                case -20: return "ecERR_INVALID_NETID";
                case -25: return "ecERR_INVALID_SLAVEID (슬레이브 인덱스 또는 슬레이브 주소가 잘못된 경우)";
                case -30: return "ecERR_INVALID_CHANNEL (축번호 또는 채널번호 등이 잘못된 경우)";
                case -40: return "ecERR_INVALID_IXMAP_IDX (Motion 제어 기능에서 Interpolation 보간 맵 번호가 잘못된 경우)";
                case -50: return "ecERR_INVALID_IXMAP_AXES (IXMAP에 포함된 축의 구성이 잘못된 경우)";
                case -55: return "ecERR_INVALID_LMMAP_IDX (리스트모션 보간 맵 번호가 잘못된 경우)";
                case -60: return "ecERR_INVALID_FUNC_ARG (함수의 매개변수가 유효하지 않은 값인 경우)";
                case -65: return "ecERR_INVALID_HANDLE (Invalid handle value가 매개변수로 전달된 경우)";
                case -66: return "ecERR_INVALID_RESULT_DATA (결과 데이터가 유효하지 않은 경우)";
                case -67: return "ecERR_INVALID_SIZE_INFO (크기 관련 데이터 정보가 유효하지 않은 경우)";
                case -69: return "ecERR_NULL_DLLNETCTXT";
                case -70: return "ecERR_NULL_WDMNETCTXT (WDM 드라이버에서 제공하는 공유메모리 포인터가 NULL인 경)";
                case -71: return "ecERR_NULL_INPDOLMEM (WDM 드라이버에서 제공하는 InPDO용 논리메모리 포인터가 NULL인 경우)";
                case -72: return "ecERR_NULL_OUTPDOLMEM *WDM 드라이버에서 제공하는 OutPDO용 논리메모리 포인터가 NULL인 경우)";
                case -73: return "ecERR_INVALID_AXIS_INPDO_TYPE (현재 설정되어 있는 Axis InputPDO 형식이 원하는 데이터를 지원하지 않음)";
                case -80: return "ecERR_INVALID_SECTION_ID (PDO Section ID가 올바르지 않거나, 환경설정된 섹션 리스트에 없는 경우)";
                case -90: return "ecERR_INVALID_LOG_MEM_ADDR (Logical Memory의 주소값이 올바르지 않은 경우)";
                case -95: return "ecERR_INVALID_BUFFER (버퍼의 주소값이 유효하지 않은 경우)";
                case -100: return "ecERR_INVALID_IO_CHAN_MAP_DATA (I/O 채널 맵핑 데이터의 일부 멤버값이 올바르지 않은 경우)";
                case -110: return "ecERR_INVALID_FILE_PATH (지정한 파일의 경로가 잘못되었거나 파일이 없는 경우)";
                case -111: return "ecERR_INTERNAL_PROCESS_FAULT (내부 연산 오류)";
                case -112: return "ecERR_MUTEX_CREATE_FAIL (Mutex 생성 실패)";
                case -113: return "ecERR_EVENT_CREATE_FAIL (이벤트 핸들 생성 실패)";
                case -114: return "ecERR_THREAD_CREATE_FAIL (쓰레드 생성 실패)";
                case -120: return "ecERR_FILE_OPEN_FAIL";
                case -125: return "ecERR_FILE_NOT_FOUND (지정한 파일이 검색되지 않은 경우)";
                case -130: return "ecERR_FILE_READ_ERROR (파일을 읽는 과정에서 에러가 발생한 경우)";
                case -140: return "ecERR_FILE_VERIF_DATA_ERROR (CFG파일 Verfication을 수행했을 때 상호간의 데이터가 일치하지 않는 경우)";
                case -150: return "ecERR_MEM_ALLOC_FAIL (Memory Allocation fail)";
                case -160: return "ecERR_XML_POS_SAVE_ERR (XML 데이터 읽기할 때 현재의 TREE position을 저장하는데 오류가 발생한 경우)";
                case -165: return "ecERR_XML_ELEM_NOT_FOUND (XML 데이터 읽기할 때 지정한 Element가 검색되지 않은 경우)";
                case -166: return "ecERR_INVALID_ODLIST_LEN_INFO (슬레이브로부터 전달된 SDO Info. OD List의 크기 정보가 유효하지 않은 경우)";
                case -180: return "ecERR_INVALID_FIRMWARE_SIGN (마스터장치에 다운로드 되어 있는 펌웨어의 signature가 유효하지 않은 경우)";
                case -181: return "ecERR_FW_VERIF_DATA_MISMATCH (F/W Download를 한 후에 Verify 과정에서 데이터가 일치하지 않은 경우)";
                case -182: return "ecERR_EEPROM_WRITE_FAIL (ESC의 EEPROM에 데이터 쓰기 실패)";
                case -183: return "ecERR_IMPROPER_AL_STATE (AL-STATE가 적절하지 않은 경우)";
                case -184: return "ecERR_INVALID_CHANNEL_TYPE (유효하지 않은 채널 형식인 경우)";

                //Motion 제어 전용 에러
                case -1010: return "ecERR_MOT_SERVO_ALARM (Servo driver에 알람이 발생한 경우)";
                case -1020: return "ecERR_MOT_SEQ_SKIPPED (Motion command has been skipped because the axis is already running)";
                case -1030: return "ecERR_MOT_LM_QUE_FULL (ListMotion Queue가 꽉차서 리스트모션 커맨드를 Queueing할 수 없는 경우)";
                case -1040: return "ecERR_MOT_LM_INVALID_OWNERSHIP (동일한 ListMotion 맵에 대한 제어권을 다른 프로세스에서 가로챈 경우)";

                //WDM 드라이버 프로그램에서 발생하는 에러코드
                case -5001: return "ERR_INVALID_DSP_IDX (잘못된 DSP Index를 매개변수로 커맨드를 요청한 경우)";
                case -5005: return "ERR_DSPCMD_IRQ_TIMEOUT (DSP Command 요청 인터럽트 요청이 제한된 시간내에 성공하지 못한 경우)";
                case -5006: return "ERR_DSPCMD_ACK_CLR_TIMEOUT (DSP 가 PCI Command에 대한 Ack 플래그를 정해진 시간 내에 클리어하지 않은 경우)";
                case -5007: return "ERR_DSPCMD_ACK_SET_TIMEOUT (DSP 가 PCI Command에 대한 Ack 플래그를 정해진 시간 내에 셋하지 않은 경우)";
                case -5010: return "ERR_PENDEDIRP_ADDTOLIST_FAIL (특정 IRP를 PendingList에 등록하려했으나 실패)";
                case -5020: return "ERR_DSPCMD_ANS_TIMEOUT (DSP 커맨드가 제한된 시간 내에 응답을 하지 않음)";

                //ETHERCAT 통신 처리 관련 에러 
                case -10010: return "ecERR_GEN_INVAL_ARGUMENT (함수 호출 시에 매개변수값이 잘못된 경우)";
                case -10020: return "ecERR_GEN_DGRAM_OBJ_ALLOC_FAIL (Slave에서 DataGram 메모리를 할당하는데 실패함)";
                case -10025: return "ecERR_GEN_MCMD_ALLOC_FAIL (Slave에서 DataGram 메모리를 할당하는데 실패함)";
                case -10030: return "ecERR_GEN_MCMD_TIMEOUT (MasterCmd 처리 타임아웃)";
                case -10040: return "ecERR_GEN_MCMD_ITEM_NOT_FOUND ( 지정한 MCmdIdx에 대한 MasterCmd Item을 리스트에서 찾을 수 없는 경우)";
                case -10050: return "ecERR_GEN_BUF_SIZE_TOO_SMALL";
                case -10060: return "ecERR_GEN_SLV_PHYSADDR_NOT_SET (Slave에 Physical Address가 셋팅되지 않음)";
                case -10070: return "ecERR_GEN_INVAL_REG_RW_SIZE (Slave Register Read/Write 시에 허용된 크기보다 큰 데이터를 Read/Write한 경우)";
                case -10080: return "ecERR_GEN_INVAL_SLAVE_ID";
                case -10090: return "ecERR_GEN_ECSLV_OBJ_NOT_FOUND (TEcSlave 객체 포인터가 null인 경우)";
                case -10100: return "ecERR_GEN_STATE_CHANGE_FAIL (EcSlave 객체 포인터가 null인 경우)";

                //커맨드 에러    
                case -10210: return "ecERR_CMD_VENDID_READ_FAIL (Vendor ID 읽기 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10220: return "ecERR_CMD_PRODCODE_READ_FAIL (Product Code 읽기 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10230: return "ecERR_CMD_REVNO_READ_FAIL (Revision No. 읽기 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10240: return "ecERR_CMD_SERNO_READ_FAIL (Serial No. 읽기 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10250: return "ecERR_CMD_SET_PHY_ADDR_FAIL (Physical Address 셋팅 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10260: return "ecERR_CMD_SET_DL_CTL_REG_FAIL (DL Control Register 셋팅 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10265: return "ecERR_CMD_SET_AL_STATE_CHANGE_FAIL (AL State 변경 과정 중에 에러 발생)";
                case -10270: return "ecERR_CMD_PDOMAP_ASSIGN_FAIL (In/Out PDO Map Assign 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10280: return "ecERR_CMD_FMMU_SETTING_FAIL (In/Out PDO Map Assign 실패. 세부 실패 원인은 서브데이터 참고)";
                case -10290: return "ecERR_CMD_PDO_SYNC_MODE_SET_FAIL (In/Out PDO Map Assign 실패. 세부 실패 원인은 서브데이터 참고)";

                //EtherCAT 통신관련 일반 에러
                case -10405: return "ecERR_ECG_DGRM_TIMEOUT (특정 EC 커맨드에 대해서 제한 시간 이내에 유효한 응답을 수신하지 못하  경우)";
                case -10410: return "ecERR_ECG_STATE_CHANGE_TIMEOUT (Slave의 State Change를 요청했으나 실제 변경하는데 제한된 시간을 초과한 경우)";
                case -10415: return "ecERR_ECG_SLAVE_DISCON (Slave가 disconnected 상태인 경우)";

                //Mailbox 통신관련 에러
                case -10610: return "ecERR_MBX_COE_NOT_SUPP (지정한  슬레이브가 CoE 프로토콜을 지원하지 않는 경우)";
                case -10620: return "ecERR_MBX_INVAL_SDO_RESP (SDO Command에 대한 응답 프레임이 올바르지 않음)";
                case -10630: return "ecERR_MBX_INVAL_SDO_SIZE (SDO의 Size 정보가 잘못된 경우)";
                case -10640: return "ecERR_MBX_SDO_DATA_SIZE_ERR (SDO 데이터가 제한된 크기보다 큰 경우)";
                case -10650: return "ecERR_MBX_INVAL_SLAVE_ID (Slave Id가 잘못 지정된 경우)";
                case -10660: return "ecERR_MBX_TXFER_ABORTED (Mailbox 통신이 abort됨, 이 때 Abort Code ID를 알려면 TEcSlave::LastErrSubData를 참조)";

                //PDO 통신관련에러
                case -10810: return "ecERR_INVALID_PDO_SIZE (PDO 크기 정보가 잘못된 경우)";
                case -10820: return "ecERR_PDO_BUF_ALLOC_FAIL (PDO 버퍼 할당 실패)";
                case -10830: return "ecERR_INVALID_OUTPDO_SM_IDX (OutPDO에 대한 SM 인덱스가 올바르지 않다)";
                case -10840: return "ecERR_INVALID_INPDO_SM_IDX (OutPDO에 대한 SM 인덱스가 올바르지 않다)";

                //---------------------------------------------------------------------------------------------------------------------------
                //			마스터장치내에서 APPLICATION 커맨드 처리 관련 에러코드
                //---------------------------------------------------------------------------------------------------------------------------

                // EcatApp 프로세서와의 IPC 통신 관련 에러 //
                case -11010: return "ecERR_IPC_MSGQ_HEAP_CRE_FAIL (IPC Message Que를 위한 Heap을 생성하는데 실패)";
                case -11020: return "ecERR_IPC_MSGQ_CRE_FAIL (IPC Message Que를 생성하느데 실패)";
                case -11030: return "ecERR_IPC_INVAL_ECM_DATASIZE (EcmCmd의 DataSize로 지정된 값이 너무 큰 경우)";
                case -11040: return "ecERR_IPC_MSGQ_PUT_FAIL (IPC Message Que에 메시지를 Put하는데 실패함 (MessageQ_put() 함수 실행 실패))";
                case -11050: return "ecERR_IPC_NOTIEVT_FAIL (IPC Notification event를 처리하는데 에러가 발생)";
                case -11060: return "ecERR_IPC_WAITACK_TIMEOUT (IPC Message 를 잘 받았다는 EcatMast측의 ACK 메시지 수신에 대한 타임아웃 발생)";

                // DSP1(APP)에서 사용하는 에러코드
                case -20010: return "eaERR_GEN_MALLOC_FAIL (memory allocation fail)";
                case -20030: return "eaERR_FLASH_DEV_OPEN_FAIL (Flash 메모리장치를 open하는데 에러가 발생한 경우)";
                case -20031: return "eaERR_FLASH_DEV_WRITE_FAIL (Flash 메모리장치에 데이터를 쓰기하는 과정에서 에러가 발생한 경우)";
                case -20032: return "eaERR_FLASH_DEV_READ_FAIL (Flash 메모리장치에 데이터를 읽기하는 과정에서 에러가 발생한 경우)";
                case -20035: return "eaERR_INVALID_FWU_SIGNATURE (Flash 메모리장치에 저장된 FW 정보가 유효하지 않은 경우)";
                case -20040: return "eaERR_INVALID_BUF_SIZE (Buffer size 정보가 잘못된 경우)";
                case -20041: return "eaERR_INVALID_BUFFER (지정된 버퍼가 NULL이거나 잘못된 버퍼가 지정된 경우)";

                // PCI 커맨드  처리 관련 에러 //
                case -20210: return "eaERR_PCICMD_TIMEOUT (PciCmd를 처리하는 과정에서 타임아웃 발생함. 이때 Error SubData는 CmdId를 나타낸다)";
                case -20220: return "eaERR_PCICMD_INVALID_CMDID (PciCmd의 CommandID가 유효하지 않음)";
                case -20230: return "eaERR_PCICMD_INVALID_ARG (PciCmd의 Argument가 유효하지 않음)";
                case -20240: return "eaERR_PCICMD_INVALID_SLVIDX (PciCmd의 SlaveIndex 가 잘못 전달된 경우)";
                case -20250: return "eaERR_PCICMD_INVALID_AXIS (Motion PciCmd의 Axis 매개변수가 잘못 전달된 경우)";
                case -20260: return "eaERR_PCICMD_INVALID_IXMAP_IDX (Motion PciCmd의 Axis 매개변수가 잘못 전달된 경우)";
                case -20270: return "eaERR_PCICMD_HOME_START_TIMEOUT (Homing을 시작시켰으나 서보드라이버가 Homing을 진행하지 않는 경우)";
                case -20280: return "eaERR_PCICMD_INVALID_AL_STATE (해당 커맨드를 실행할 수 있는 AL State가 아닌 경우)";

                // EcatMast(Net) 프로세서와의 IPC 통신 관련 에러 //
                case -20410: return "eaERR_IPC_MSGQ_HEAP_CRE_FAIL (IPC Message Que를 위한 Heap을 생성하는데 실패)";
                case -20420: return "eaERR_IPC_MSGQ_CRE_FAIL (IPC Message Que를 생성하느데 실패)";
                case -20430: return "eaERR_IPC_INVAL_EAPC_DATASIZE (EapCmd의 DataSize로 지정된 값이 너무 큰 경우)";
                case -20440: return "eaERR_IPC_MSGQ_PUT_FAIL (IPC Message Que에 메시지를 Put하는데 실패함 (MessageQ_put() 함수 실행 실패))";
                case -20450: return "eaERR_IPC_NOTIEVT_FAIL (IPC Notification event를 처리하는데 에러가 발생)";
                case -20460: return "eaERR_IPC_WAITACK_TIMEOUT (IPC Message 를 잘 받았다는 EcatMast측의 ACK 메시지 수신에 대한 타임아웃 발생)";

                // 모션제어 관련 에러코드 //
                case -21002: return "eaERR_AXIS_MOT_QUE_FULL (axis-motion-que가 꽉차서 모션을 예약하지 못하였음)";
                case -21010: return "eaERR_INVALID_PARAMETER (Some of the funcion parameters are invalid)";
                case -21011: return "eaERR_INVALID_AXIS (The axis setting parameter(s) is(are) invalid)";
                case -21012: return "eaERR_INVALID_SPEED_SET (Speed setting value is not valid)";
                case -21013: return "eaERR_INVALID_IXMAP (Invalid Interpolation Map)";
                case -21014: return "eaERR_INVALID_LMMAP (Invalid List-Motion Map)";
                case -21015: return "eaERR_INVALID_NUMAXIS (Invlaid number of axis(Mx))";
                case -21016: return "eaERR_INVALID_MAST_AXIS (Invlaid master axis settings for Master/Slave Motion)";
                case -21017: return "eaERR_INVALID_SPLINE_SETUP (SpineSetup() 함수가 수행되지 않은 상태에서 Spline보간 관련 다른 함수를 실행한 경우)";
                case -21038: return "eaERR_INVALID_POS_DATA (Position 데이터가 유효하지 않은 경우)";
                case -21030: return "eaERR_IX_AXES_NOT_DEFINED (보간 축 설정이 수행되지 않은 경우)";
                case -21035: return "eaERR_IX_OBJ_POOL_FULL (spline/MPRLin2X 등의  object 등록 pool에 현재 등록되어 있는 object가 최대 갯수만큼 등록되어서 더 이상 추가할 수 없는 경우)";
                case -21036: return "eaERR_IX_OBJ_NOT_FOUND (spline/MPRLin2X 등의  object pool에서 지정한 SPLINE OBJECT를 찾을 수 없음)";
                case -21037: return "eaERR_IX_OBJ_NOT_BUILDED (spline/MPRLin2X 등의  object Build를 수행하지 않고 이송 시작을 수행한 경우)";
                case -21047: return "eaERR_STOP_BY_COLLA (충돌방지조건에 의해서 정지된 경우)";
                case -21048: return "eaERR_STOP_BY_HEMG (HEMG(Hardware Emergnecy Input) 신호에 의해서 정지)";
                case -21049: return "eaERR_STOP_BY_SEMG (HEMG(Software Emergnecy Input) 신호에 의해서 정지)";
                case -21050: return "eaERR_STOP_BY_SLP (Abnormally stopped by positive soft limit)";
                case -21051: return "eaERR_STOP_BY_SLN (Abnormally stopped by negative soft limit)";
                case -21052: return "eaERR_STOP_BY_ELP (Abnormally stopped by (+) external limit)";
                case -21053: return "eaERR_STOP_BY_ELN (Abnormally stopped by (-) external limit)";
                case -21054: return "eaERR_STOP_BY_ALM (Abnormally stopped by alarm input signal)";
                case -21055: return "eaERR_STOP_BY_COMM_ERROR (Abnormally Stopped by communication error)";
                case -21056: return "eaERR_STOP_BY_OTHER_AXIS (다른 축과의 협업 이송 중에 다른 축의 에러에 의해서 정지된 경우)";
                case -21057: return "eaERR_STOP_BY_SVOFF (SERVO-OFF된 상태에서 이송명령이 내려진 경우, 또는 이송 중에 SERVO-OFF된 경우)";
                case -21060: return "eaERR_MOT_SEQ_SKIPPED (Motion command has been skipped because the axis is already running)";
                case -21062: return "eaERR_SKIPPED_BY_SERVO_FAULT (Motion Control Command is skipped because of servo-driver alarm)";
                case -21063: return "eaERR_SKIPPED_BY_SERVO_OFF (Motion Control Command is skipped because of 'Operation Enable(Servo-ON)' state is off)";
                case -21064: return "eaERR_SKIPPED_BY_SERVO_ON (서보온이 되어 있는 상태에서 처리할 수 없는 명령이 하달되어 해당 명령의 수행이 되지 않은 경우)";
                case -21070: return "eaERR_HOME_START_TIMEOUT (서보드라이버에 Homming start 명령을 내렸으나 지정된 시간 내에  Homming이 시작되지 않는 경우)";
                case -21071: return "eaERR_HOME_COMPT_FAIL (Servo driver의 Homing 작업을 완료하는 과정에서 에러가 발생함)";
                case -21072: return "eaERR_HOME_TPROBE_NOT_TRIGGERED (101번 이상의 원점복귀 모드를 수행할 때 Touch Probe가 정상적으로 Trigger되지 않은 경우.)";
                case -21080: return "eaERR_HOME_STEP_TIMEOUT0 (Touch Probe 셋팅이 제한시간내에 완료되지 않은 경우)";
                // 이 중간의에러코드는 HOME_STEP 번호에 따라서 매겨지므로 따로 정의하지 말것 //
                case -21099: return "eaERR_HOME_STEP_TIMEOUT19 (Touch Probe 셋팅이 제한시간내에 완료되지 않은 경우)";
                case -21110: return "eaERR_LM_QUE_FULL (리스트모션의 커맨드 큐가 꽉차서 커맨드를 등록하지 못한 경우)";
                case -21130: return "eaERR_PTM_QUE_FULL (PT모션의 커맨드 큐가 꽉차서 커맨드를 등록하지 못한 경우.)";
                case -21131: return "eaERR_PTM_MAP_DISABLED (PT모션 맵이 Begin(ecmLmCtl_Begin() 함수를 통해서 실행됨)되지 않은 상태에서 PT모션 관련 함수들을 실행한 경우.)";
                case -21150: return "eaERR_INAVLID_PDO_MAP (현재의 PDO Mapping에서 지원되지 않는 기능을 사용하려고 하는 경우)";


                //SW EtherCAT Only
                case -50001: return "secERR_NULL_FILENAME (파일 메모리 포인터가 NULL 인 경우)";
                case -50002: return "secERR_NULL_ECATMAINTASK (EcatCheckProc 쓰레드 생성 실패한 경우)";
                case -50003: return "secERR_NULL_ECATCHECKPROC (EcatCheckProc 쓰레드 생성 실패한 경우)";
                case -50004: return "secERR_NONE_NETADAPTER	(Network Adapter가 발견되지 않은 경우)";
                case -50005: return "secERR_NOT_OP_ALLSLAVE	(일부 또는 전체 Slave가 OP 상태에 있지 않는 경우)";

                case -50006: return "secERR_NULL_BUFFER	(버퍼가 NULL인 경우)";
                case -50007: return "secERR_EEPROM_READ	(EEPROM Read 실패한 경우)";
                case -50008: return "secERR_EEPROM_WRITE (EEPROM Write 실패한 경우)";
                case -50009: return "secERR_EEPROM_FILE	(EEPROM File 읽기가 실패한 경우)";
                case -50010: return "secERR_INVALID_EEPROM_RW_MODE	 지원되지 않는 EEPROM Read/Write Mode 요청시)";

                case -50011: return "secERR_OVERSIZE_FIRMWARE (펌웨어 크기가 너무 큰 경우)";
                case -50012: return "secERR_FOE_WRITE (FOE Write 실패한 경우)";
                case -50013: return "secERR_FIRMWARE_FILE (Firmware 파일 읽기가 실패한 경우)";
                case -50014: return "secERR_TIMEOUT_BOOTMODE (BOOT 모드로 요청에 대해 Timeout된 경우)";

                case -50015: return "secERR_SDO_READ (SDO Read 실패시)";
                case -50016: return "secERR_SDO_READ_TIMEOUT (SDO Read Timeout)";
                case -50017: return "secERR_SDO_WRITE (SDO Write 실패시)";
                case -50018: return "secERR_SDO_WRITE_TIMEOUT (SDO Write Timeout)";
                case -50019: return "secERR_NOT_SUPPORTED_DEVICE (지원되지 않는 Device)";

                case -50020: return "secERR_NET_CMD_SCAN (Scan Request 실패시)";
                case -50021: return "secERR_NET_CMD_SCAN_TIMEOUT (Scan Response Timeout)";
                case -50022: return "secERR_SLV_CONFIG_CHANGED (Scan 한 Slave와 Configuration 파일의 정보가 일치하지 않는 경우)";
                default: return "UnKnown Error Occured";
            }
        }

        //====================== NET INTERFACE FUNCTIONS =========================================================//


        //EC_EXTERN t_bool	(CECAT_API *ecNet_IsBootCompt) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_IsBootCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int errCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_GetDevIdx) (t_i32 NetID);
        [DllImport(dll)]
        public static extern unsafe int ecNet_GetDevIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_GetLocNetIdx) (t_i32 NetID);
        [DllImport(dll)]
        public static extern unsafe int ecNet_GetLocNetIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID);

        //EC_EXTERN t_success (CECAT_API *ecNet_GetVerInfo) (t_i32 NetID, TEcFileVerInfo_SDK *pVerInfo_SDK, TEcFileVerInfo_WDM *pVerInfo_WDM, TEcFileVerInfo_FW *pVerInfo_FW, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_GetVerInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            ref TEcFileVerInfo_SDK pVerInfo_SDK,
            ref TEcFileVerInfo_WDM pVerInfo_WDM,
            ref TEcFileVerInfo_FW pVerInfo_FW,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecNet_ScanSlaves) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_ScanSlaves(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecNet_GetCfgSlaveCount) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_GetCfgSlaveCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_SetCfgSlaveCount) (t_i32 NetID, t_ui32 SlaveCount, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SetCfgSlaveCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint SlaveCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecNet_SetAlState) (t_i32 NetID, EEcAlState AlState, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_SetAlState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            EEcAlState AlState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecNet_SetAlState_FF) (t_i32 NetID, EEcAlState AlState, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_SetAlState_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            EEcAlState AlState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN EEcAlState	(CECAT_API *ecNet_GetAlState) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcAlState ecNet_GetAlState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecNet_SlvComErrSum_GetEnable) (t_i32 NetID, int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SlvComErrSum_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_SlvComErrSum_SetEnable) (t_i32 NetID, t_bool IsEnable, int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SlvComErrSum_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool isEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_SlvComErrSum_ClearAll) (t_i32 NetID, int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SlvComErrSum_ClearAll(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_CheckReverseConnections) (t_i32 NetID, t_i32 *pnNumScanSlaves, t_i32 *ErrCode); // IN/OUT 역삽입된 슬레이브들 체크하는 함수. 반환값은 역삽입된 슬레이브의 갯수
        [DllImport(dll)]
        public static extern unsafe int ecNet_CheckReverseConnections(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int pnNumScanSlaves,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //====================== SLAVE I/F FUNCTIONS ============================================================//

        //EC_EXTERN t_i32			(CECAT_API *ecSlv_SlvIdx2PhysAddr) (t_i32 NetID, t_ui16 SlaveIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_SlvIdx2PhysAddr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_PhysAddr2SlvIdx) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_PhysAddr2SlvIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecSlv_GetProdInfo_A) (t_i32 NetID, t_i32 SlaveIndex, TEcSlvProdInfo *pProdInfoBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_GetProdInfo_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIndex,
            ref TEcSlvProdInfo pProdInfoBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecSlv_GetProdInfo) (t_i32 NetID, t_i32 SlvPhysAddr, TEcSlvProdInfo *pProdInfoBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_GetProdInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            ref TEcSlvProdInfo pProdInfoBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecSlv_GetTypeInfo) (t_i32 NetID, t_ui16 SlvPhysAddr, TEcSlvTypeInfo *pTypeInfoBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_GetTypeInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            ref TEcSlvTypeInfo pTypeInfoBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecSlv_GetTypeInfo_A) (t_i32 NetID, t_i32 SlaveIndex, TEcSlvTypeInfo *pTypeInfoBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_GetTypeInfo_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIndex,
            ref TEcSlvTypeInfo pTypeInfoBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui8		(CECAT_API *ecSlv_GetAlState) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcAlState ecSlv_GetAlState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui8		(CECAT_API *ecSlv_GetAlState_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcAlState ecSlv_GetAlState_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadReg) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 RegAddr, t_i32 DataSize, _out void *pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadReg(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadReg_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 RegAddr, t_i32 DataSize, _out void *pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadReg_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteReg) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 RegAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteReg(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteReg_FF) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 RegAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteReg_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteReg_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 RegAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteReg_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteReg_A_FF) (t_i32 NetID, t_i32 SlaveIdx, t_i32 RegAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteReg_A_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int RegAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadCoeSdo) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 Index, t_i32 SubIdx, t_i32 IsComptAccess, t_i32 DataSize, void* pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadCoeSdo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo")]
        public static extern unsafe int ecSlv_ReadCoeSdo_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo")]
        public static extern unsafe int ecSlv_ReadCoeSdo_Array(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadCoeSdo_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 Index, t_i32 SubIdx, t_i32 IsComptAccess, t_i32 DataSize, void* pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadCoeSdo_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo_A")]
        public static extern unsafe int ecSlv_ReadCoeSdo_A_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I8)] ref long pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo_A")]
        public static extern unsafe int ecSlv_ReadCoeSdo_A_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo_A")]
        public static extern unsafe int ecSlv_ReadCoeSdo_A_Array(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_ReadCoeSdo_A")]
        public static extern unsafe int ecSlv_ReadCoeSdo_A_Ptr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            ref IntPtr pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteCoeSdo) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 Index, t_i32 SubIdx, t_i32 IsComptAccess, t_i32 DataSize, void* pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteCoeSdo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo")]
        public static extern unsafe int ecSlv_WriteCoeSdo_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo")]
        public static extern unsafe int ecSlv_WriteCoeSdo_Array(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteCoeSdo_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo_FF")]
        public static extern unsafe int ecSlv_WriteCoeSdo_FF_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteCoeSdo_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 Index, t_i32 SubIdx, t_i32 IsComptAccess, t_i32 DataSize, void* pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteCoeSdo_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo_A")]
        public static extern unsafe int ecSlv_WriteCoeSdo_A_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I8)] ref long pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo_A")]
        public static extern unsafe int ecSlv_WriteCoeSdo_A_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo_A")]
        public static extern unsafe int ecSlv_WriteCoeSdo_A_Array(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteCoeSdo_A_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecSlv_WriteCoeSdo_A_FF")]
        public static extern unsafe int ecSlv_WriteCoeSdo_A_FF_Float(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int Index,
            [MarshalAs(UnmanagedType.I4)] int SubIdx,
            [MarshalAs(UnmanagedType.I4)] int IsComptAccess,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.R4)] ref float pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll)]
        public static extern unsafe int ecSlv_GetComErrSum_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort slaveIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16		(CECAT_API *ecSlv_GetComErrSum) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecSlv_GetComErrSum(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_ClearComErrSum_A) (t_i32 NetID, t_i32 SlaveIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_ClearComErrSum_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16		(CECAT_API *ecSlv_ClearComErrSum) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecSlv_ClearComErrSum(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool		(CECAT_API *ecSlv_IsReverseConnected_A) (t_i32 NetID, t_ui16 SlaveIdx, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_IsReverseConnected_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecSlv_SetEscWDT_A) (t_i32 NetID, t_ui16 SlaveIdx, t_i32 WDT_msec, _out t_i32 * ErrCode); // 이더캣슬레입칩의 와치독 타임아웃 값을 msec 단위로 설정
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_SetEscWDT_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIndex,
            [MarshalAs(UnmanagedType.I4)] int WDT_msec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32(CECAT_API* ecSlv_GetEscWDT_A) (t_i32 NetID, t_ui16 SlaveIdx, _out t_i32 * ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_GetEscWDT_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecSlv_SetEscWDT) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 WDT_msec, _out t_i32 * ErrCode); // 이더캣슬레입칩의 와치독 타임아웃 값을 msec 단위로 설정
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_SetEscWDT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int WDT_msec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32(CECAT_API* ecSlv_GetEscWDT) (t_i32 NetID, t_ui16 SlvPhysAddr, _out t_i32 * ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_GetEscWDT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        ////====================== Digital Input FUNCTIONS =======================================================//


        //--------------- Global Channel을 이용하는 함수군 ----------------------------------------------//

        //EC_EXTERN t_i32		(CECAT_API *ecdiGetSlaveIndex) (t_i32 NetID, t_ui32 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiGetSlaveIndex(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecdiGetSlaveID) (t_i32 NetID, t_ui32 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiGetSlaveID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecdiGetNumChannels) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiGetNumChannels(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecdiGetOne) (t_i32 NetID, t_ui32 DiChannel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiGetOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword	(CECAT_API *ecdiGetMulti) (t_i32 NetID, t_ui32 IniChannel, t_ui8 NumChannels, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecdiGetMulti(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint IniChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdiGetLogicAddr) (t_i32 NetID, t_ui32 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecdiGetLogicAddr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_AddChannel) (t_i32 NetID, t_ui16 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_AddChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_DelChannel) (t_i32 NetID, t_ui16 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_DelChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_SetFilter) (t_i32 NetID, t_ui16 DiChannel, t_i32 FilterCycles, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_SetFilter(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] int FilterCycles,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLtc_GetFilter) (t_i32 NetID, t_ui16 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiLtc_GetFilter(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_SetLogicInvert) (t_i32 NetID, t_ui16 DiChannel, t_bool IsInvertLogic, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_SetLogicInvert(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I1)] bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool		(CECAT_API *ecdiLtc_GetLogicInvert) (t_i32 NetID, t_ui16 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_GetLogicInvert(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLtc_GetOnCount) (t_i32 NetID, t_ui16 DiChannel, t_bool IsResetOnCount, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiLtc_GetOnCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I1)] bool IsResetOnCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_ResetOnCount) (t_i32 NetID, t_ui16 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_ResetOnCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //--------------- Slave Address와 Local Channel을 이용하는 함수군 ------------------------------//

        //EC_EXTERN t_bool		(CECAT_API *ecdiGetOne_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiGetOne_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdiGetMulti_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui32 IniLocalChannel, t_ui8 NumChannels, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecdiGetMulti_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U4)] uint IniLocalChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_AddChannel_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_AddChannel_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_DelChannel_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_DelChannel_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_SetFilter_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 FilterCycles, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_SetFilter_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] int FilterCycles,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLtc_GetFilter_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiLtc_GetFilter_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_SetLogicInvert_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_bool IsInvertLogic, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_SetLogicInvert_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I1)] bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool		(CECAT_API *ecdiLtc_GetLogicInvert_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_GetLogicInvert_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLtc_GetOnCount_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_bool IsResetOnCount, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdiLtc_GetOnCount_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I1)] bool IsResetOnCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdiLtc_ResetOnCount_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdiLtc_ResetOnCount_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //--------------- 논리비트주소(Logic Bit Address) 변환 함수군 ------------------------------//

        //EC_EXTERN t_dword		(CECAT_API *ecdiLogBitAddr_FromGlobalChannel) (t_i32 NetID, t_i32 GlobalChannel, t_i32 *ErrCode); 
        // Global Input Channel을 논리비트주소로 변환 
        [DllImport(dll)]
        public static extern unsafe uint ecdiLogBitAddr_FromGlobalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int GlobalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdiLogBitAddr_FromLocalChannel) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode); 
        // Local Input Channel을 논리비트주소로 변환 
        [DllImport(dll)]
        public static extern unsafe uint ecdiLogBitAddr_FromLocalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdiLogBitAddr_FromOnboardChannel) (t_i32 NetID, t_i32 OnboardChannel, t_i32 *ErrCode); 
        // Onboard Input Channel을 논리비트주소로 변환
        [DllImport(dll)]
        public static extern unsafe uint ecdiLogBitAddr_FromOnboardChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int OnboardChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLogBitAddr_ToGlobalChannel) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode); 
        // 논리비트주소를 Global Input Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdiLogBitAddr_ToGlobalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLogBitAddr_ToLocalChannel) (t_i32 NetID, t_dword LogBitAddr, t_ui16 *SlvPhysAddr, t_i32 *ErrCode); 
        // 논리비트주소를 Local Input Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdiLogBitAddr_ToLocalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.U2)] ref ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdiLogBitAddr_ToOnboardChannel) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode);
        // 논리비트주소를 Onboard Input Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdiLogBitAddr_ToOnboardChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN EEcIoChanType (CECAT_API *ecdiLogBitAddr_GetChanType) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcIoChanType ecdiLogBitAddr_GetChanType(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== Digital Output FUNCTIONS =======================================================//


        //--------------- Global Channel을 이용하는 함수군 ----------------------------------------------//

        //EC_EXTERN t_i32		(CECAT_API *ecdoGetSlaveIndex) (t_i32 NetID, t_ui32 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdoGetSlaveIndex(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecdoGetSlaveID) (t_i32 NetID, t_ui32 DiChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdoGetSlaveID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DiChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecdoGetNumChannels) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecdoGetNumChannels(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecdoPutOne) (t_i32 NetID, t_ui32 DoChannel, t_bool OutState, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoPutOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DoChannel,
            [MarshalAs(UnmanagedType.I1)] bool OutState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecdoPutMulti) (t_i32 NetID, t_ui32 IniChannel, t_ui8 NumChannels, t_dword dwOutStates, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoPutMulti(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint IniChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.U4)] uint dwOutStates,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecdoGetOne) (t_i32 NetID, t_ui32 DoChannel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoGetOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint DoChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword	(CECAT_API *ecdoGetMulti) (t_i32 NetID, t_ui32 IniChannel, t_ui8 NumChannels, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecdoGetMulti(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint IniChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //--------------- Slave Address와 Local Channel을 이용하는 함수군 ------------------------------//

        //EC_EXTERN t_success		(CECAT_API *ecdoPutOne_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_bool OutState, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoPutOne_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I1)] bool OutState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecdoPutMulti_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui32 IniLocalChannel, t_ui8 NumChannels, t_dword dwOutStates, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoPutMulti_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U4)] uint IniLocalChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.U4)] uint dwOutStates,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool		(CECAT_API *ecdoGetOne_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoGetOne_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U4)] uint LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //_EXTERN t_dword		(CECAT_API *ecdoGetMulti_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui32 IniLocalChannel, t_ui8 NumChannels, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecdoGetMulti_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U4)] uint IniLocalChannel,
            [MarshalAs(UnmanagedType.U1)] byte NumChannels,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //--------------- 논리비트주소(Logic Bit Address) 변환 함수군 ------------------------------//

        //EC_EXTERN t_dword		(CECAT_API *ecdoLogBitAddr_FromGlobalChannel) (t_i32 NetID, t_i32 GlobalChannel, t_i32 *ErrCode); 
        // Global Ouput Channel을 논리비트주소로 변환 
        [DllImport(dll)]
        public static extern unsafe uint ecdoLogBitAddr_FromGlobalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int GlobalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdoLogBitAddr_FromLocalChannel) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode); 
        // Local Output Channel을 논리비트주소로 변환 
        [DllImport(dll)]
        public static extern unsafe uint ecdoLogBitAddr_FromLocalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword		(CECAT_API *ecdoLogBitAddr_FromOnboardChannel) (t_i32 NetID, int OnboardChannel, t_i32 *ErrCode);
        // Onboard Output Channel을 논리비트주소로 변환
        [DllImport(dll)]
        public static extern unsafe uint ecdoLogBitAddr_FromOnboardChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int OnboardChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdoLogBitAddr_ToGlobalChannel) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode); 
        // 논리비트주소를 Global Output Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdoLogBitAddr_ToGlobalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdoLogBitAddr_ToLocalChannel) (t_i32 NetID, t_dword LogBitAddr, t_ui16 *SlvPhysAddr, t_i32 *ErrCode); 
        // 논리비트주소를 Global Output Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdoLogBitAddr_ToLocalChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.U2)] ref ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecdoLogBitAddr_ToOnboardChannel) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode); 
        // 논리비트주소를 Onboard Output Channel로 변환
        [DllImport(dll)]
        public static extern unsafe int ecdoLogBitAddr_ToOnboardChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN EEcIoChanType (CECAT_API *ecdoLogBitAddr_GetChanType) (t_i32 NetID, t_dword LogBitAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcIoChanType ecdoLogBitAddr_GetChanType(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== Analog Input FUNCTIONS =======================================================//

        //EC_EXTERN t_i32		(CECAT_API *ecaiGetSlaveIndex) (t_i32 NetID, t_ui32 Channel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaiGetSlaveIndex(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaiGetSlaveID) (t_i32 NetID, t_ui32 Channel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaiGetSlaveID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaiGetNumChannels) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaiGetNumChannels(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaiGetChanVal_I) (t_i32 NetID, t_ui32 Channel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaiGetChanVal_I(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecaiGetChanVal_F) (t_i32 NetID, t_ui32 Channel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecaiGetChanVal_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecaiGetChanVal_FS) (t_i32 NetID, t_ui32 Channel, t_f32 ScaleMin, t_f32 ScaleMax, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecaiGetChanVal_FS(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.R4)] float ScaleMin,
            [MarshalAs(UnmanagedType.R4)] float ScaleMax,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecaiSetPdoInfo_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, int LocalPdoAddr, int BitSize, int DataType, t_i32 *ErrCode); // 해당 채널의 AI 데이터를 PDO영역에서 추출하기 위한 정보를 설정하는 함수. 'DataType' 인자는 'EEcAiDataType' 선언문 참조할 것.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecaiSetPdoInfo_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] int LocalPdoAddr,
            [MarshalAs(UnmanagedType.I4)] int BitSize,
            [MarshalAs(UnmanagedType.I4)] int DataType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecaiSetScaleRange_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_f32 ScaleMin, t_f32 ScaleMax, t_i32 *ErrCode); // 해당 채널의 정수형 AI 데이터를 소수점형 데이터로 변환하기 위한 Scale 범위를 설정하는 함수.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecaiSetScaleRange_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.R4)] float ScaleMin,
            [MarshalAs(UnmanagedType.R4)] float ScaleMax,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecaiSetScaleGain_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_f32 ScaleGain, t_i32 *ErrCode); // 해당 채널의 정수형 AI 데이터를 소수점형 데이터로 변환할 때 특정 값을 곱해서 변환해야 하는 경우에 그 곱해지는 값(Gain)을 설정하는 함수.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecaiSetScaleGain_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.R4)] float ScaleGain,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaiGetChanVal_I_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode); // 해당 채널의 정수형 AI 데이터를 반환하는 함수.
        [DllImport(dll)]
        public static extern unsafe int ecaiGetChanVal_I_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecaiGetChanVal_F_L) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_i32 *ErrCode); // 해당 채널의 소수점형 AI 데이터를 반환하는 함수.
        [DllImport(dll)]
        public static extern unsafe double ecaiGetChanVal_F_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== Analog Output FUNCTIONS =======================================================//

        //EC_EXTERN t_i32		(CECAT_API *ecaoGetSlaveIndex) (t_i32 NetID, t_ui32 Channel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaoGetSlaveIndex(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaoGetSlaveID) (t_i32 NetID, t_ui32 Channel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaoGetSlaveID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaoGetNumChannels) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaoGetNumChannels(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecaoSetChanVal_I) (t_i32 NetID, t_ui32 Channel, t_i32 OutData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecaoSetChanVal_I(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] int OutData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecaoSetChanVal_F) (t_i32 NetID, t_ui32 Channel, t_f64 OutData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecaoSetChanVal_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.R8)] double OutData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecaoSetChanVal_FS) (t_i32 NetID, t_ui32 Channel, t_f64 OutData, t_f64 ScaleMin, t_f64 ScaleMax, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecaoSetChanVal_FS(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.R8)] double outData,
            [MarshalAs(UnmanagedType.R8)] double ScaleMin,
            [MarshalAs(UnmanagedType.R8)] double ScaleMax,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecaoGetOutValue_I) (t_i32 NetID, t_ui32 Channel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecaoGetOutValue_I(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecaoGetOutValue_F) (t_i32 NetID, t_ui32 Channel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecaoGetOutValue_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecaoGetOutValue_FS) (t_i32 NetID, t_ui32 Channel, t_f64 ScaleMin, t_f64 ScaleMax, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecaoGetOutValue_FS(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint Channel,
            [MarshalAs(UnmanagedType.R8)] double ScaleMin,
            [MarshalAs(UnmanagedType.R8)] double ScaleMax,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);





        ////====================== MOTION - GENEAL FUNCTIONS =======================================================//

        //EC_EXTERN t_i32		(CECAT_API *ecmGn_GetAxisList) (t_i32 NetID, t_ui8 AxisListBuf[], t_ui8 AxisListBufSize, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmGn_GetAxisList(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] AxisListBuf,
            [MarshalAs(UnmanagedType.U1)] byte AxisListBufSize,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcmInPDO_Header* (CECAT_API *ecmGn_GetInPDOHeader) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcmInPDO_Header ecmGn_GetInPDOHeader(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmGn_AxisToSlaveIndex) (t_i32 NetID, int Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmGn_AxisToSlaveIndex(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmGn_AxisToSlaveID) (t_i32 NetID, int Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmGn_AxisToSlaveID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmGn_SlaveIndexToAxis) (t_i32 NetID, int SlaveIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmGn_SlaveIndexToAxis(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmGn_SlaveIDToAxis) (t_i32 NetID, int SlaveID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmGn_SlaveIDToAxis(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmGn_InitFromFile) (t_i32 NetID, t_i32 AxisCnt, char *szMotCfgFile, t_i32 *ErrCode);
        [DllImport(dll, EntryPoint = "ecmGn_InitFromFile")]
        public static extern unsafe int ecmGn_InitFromFile_old(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int AxisCount,
            //[MarshalAs(UnmanagedType.LPWStr)] string szMotCfgFile,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szMotCfgFile,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecmGn_InitFromFile")]
        public static extern unsafe int ecmGn_InitFromFile(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szMotCfgFile,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //------ Hardware Emergency Stop --------------------------------------------------------//

        //EC_EXTERN t_success	(CECAT_API *ecmHEMG_SetInputEnv) (t_i32 NetID, t_dword LogBitAddr, t_bool InvertLogic, t_i32 FilterCount, t_i32 *ErrCode); 
        // Hardware EMG Stop 입력 신호에 대한 환경을 설정한다(단일 입력인 경우)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_SetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] bool InvertLogic,
            [MarshalAs(UnmanagedType.I4)] int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHEMG_GetInputEnv) (t_i32 NetID, t_dword *LogBitAddr, t_bool *InvertLogic, t_i32 *FilterCount, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_GetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] ref uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] ref bool InvertLogic,
            [MarshalAs(UnmanagedType.I4)] ref int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHEMG_SetInputEnv_Multi) (t_i32 NetID, t_i32 NumInputs, TEcmEmgInputEnv *pInputList, t_i32 FilterCount, t_i32 *ErrCode); 
        // Hardware EMG Stop 입력 신호에 대한 환경을 설정한다(복수 입력인 경우)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_SetInputEnv_Multi(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumInputs,
            TEcmEmgInputEnv[] TEcmEmgInputEnv,
            [MarshalAs(UnmanagedType.I4)] int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmHEMG_GetInputEnv_Multi) (t_i32 NetID, t_i32 MaxNumInputsToCopy, TEcmEmgInputEnv *pInputListBuf, t_i32 *FilterCount, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHEMG_GetInputEnv_Multi(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MaxNumInputsToCopy,
            //ref TEcmEmgInputEnv[] pInputListBuf,
            IntPtr startPosition,
            [MarshalAs(UnmanagedType.I4)] ref int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmHEMG_GetNumInputs) (t_i32 NetID, t_i32 *ErrCode); 
        // 'Hardware EMG Stop' 신호로 사용자가 등록한 입력 신호의 갯수를 반환한다.
        [DllImport(dll)]
        public static extern unsafe int ecmHEMG_GetNumInputs(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHEMG_SetStopMode) (t_i32 NetID, t_bool IsDecelStop, t_i32 *ErrCode); 
        // 'Hardware EMG Stop' 신호가 ON되었을 때 모터를 정지하는 방식을 설정한다(즉시정지 또는 감속정지)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_SetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool IsDecelStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmHEMG_GetStopMode) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_GetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHEMG_SetEnable) (t_i32 NetID, t_bool IsEnable, t_i32 *ErrCode); 
        //  'Hardware EMG Stop' 입력을 활성화/비활성화 시킨다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmHEMG_GetEnable) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmHEMG_GetState) (t_i32 NetID, t_i32 *ErrCode); 
        // 'Hardware EMG Stop' 입력의 동작 상태를 반환한다. 0-OFF, 1-ON
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHEMG_GetState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //------ Software Emergnecy Stop --------------------------------------------------------//

        //EC_EXTERN t_success	(CECAT_API *ecmSEMG_SetStopMode) (t_i32 NetID, t_bool IsDecelStop, t_i32 *ErrCode); 
        // 'Software EMG Stop' 신호가 ON되었을 때 모터를 정지하는 방식을 설정한다(즉시정지 또는 감속정지)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSEMG_SetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool IsDecelStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSEMG_GetStopMode) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSEMG_GetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSEMG_SetState) (t_i32 NetID, t_bool State, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSEMG_SetState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool State,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSEMG_GetState) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSEMG_GetState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //------ Collision Avoidance (CollA) ----------------------------------------------------//

        //EC_EXTERN t_success	(CECAT_API *ecmCollA_SetEnv) (t_i32 NetID, t_i32 CollAIdx, t_i32 MasterAxis, t_i32 SlvAxis, t_i32 SubOrAdd, t_i32 LessOrGreater, t_f64 Limit, t_i32 *ErrCode); // CollA의 환경을 설정한다.
        [DllImport(dll, EntryPoint = "ecmCollA_SetEnv")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_SetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I4)] int MasterAxis,
            [MarshalAs(UnmanagedType.I4)] int SlvAxis,
            [MarshalAs(UnmanagedType.I4)] int SubOrAdd,
            [MarshalAs(UnmanagedType.I4)] int LessOrGreater,
            [MarshalAs(UnmanagedType.R8)] double Limit,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmCollA_GetEnv) (t_i32 NetID, t_i32 CollAIdx, t_i32* MasterAxis, t_i32* SlvAxis, t_i32* SubOrAdd, t_i32* LessOrGreater, t_f64* Limit, t_i32 *ErrCode); // CollA의 환경 설정값을 읽는다.
        [DllImport(dll, EntryPoint = "ecmCollA_GetEnv")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_GetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I4)] ref int MasterAxis,
            [MarshalAs(UnmanagedType.I4)] ref int SlvAxis,
            [MarshalAs(UnmanagedType.I4)] ref int SubOrAdd,
            [MarshalAs(UnmanagedType.I4)] ref int LessOrGreater,
            [MarshalAs(UnmanagedType.R8)] ref double Limit,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmCollA_SetEnable) (t_i32 NetID, t_i32 CollAIdx, t_bool IsEnable, t_i32 *ErrCode); // CollA 기능을 활성화 또는 비활성화한다.
        [DllImport(dll, EntryPoint = "ecmCollA_SetEnable")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I1)] bool isEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmCollA_GetEnable) (t_i32 NetID, t_i32 CollAIdx, t_i32 *ErrCode); // CollA 기능의 활성화 여부를 반환한다. 
        [DllImport(dll, EntryPoint = "ecmCollA_GetEnable")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //t_success ecmCollA_SetStopMode (t_i32 NetID, t_i32 CollAIdx, t_bool IsDecelStop_M, t_bool IsDecelStop_S, t_i32 *ErrCode); // CollA의 Stop Mode를 설정한다.
        [DllImport(dll, EntryPoint = "ecmCollA_SetStopMode")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_SetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I1)] bool IsDecelStop_M,
            [MarshalAs(UnmanagedType.I1)] bool IsDecelStop_S,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //t_success ecmCollA_GetStopMode (t_i32 NetID, t_i32 CollAIdx, t_bool* IsDecelStop_M, t_bool* IsDecelStop_S, t_i32 *ErrCode); // CollA의 Stop Mode 설정값을 읽는다.
        [DllImport(dll, EntryPoint = "ecmCollA_GetStopMode")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmCollA_GetStopMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CollAIdx,
            [MarshalAs(UnmanagedType.I1)] ref bool IsDecelStop_M,
            [MarshalAs(UnmanagedType.I1)] ref bool IsDecelStop_S,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //*****************************************************************************************************************************
        //*
        //*							Functions for Advanced Users
        //*
        //*****************************************************************************************************************************


        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetJerkMode) (t_i32 NetID, t_i32 Axis, t_i32 JerkMode, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetJerkMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int JerkMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_GetJerkMode) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_GetJerkMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetJerk) (t_i32 NetID, t_i32 Axis, t_f64 AccJerk1, t_f64 AccJerk2, t_f64 DecJerk1, t_f64 DecJerk2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetJerk(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double AccJerk1,
            [MarshalAs(UnmanagedType.R8)] double AccJerk2,
            [MarshalAs(UnmanagedType.R8)] double DecJerk1,
            [MarshalAs(UnmanagedType.R8)] double DecJerk2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetJerk) (t_i32 NetID, t_i32 Axis, t_f64 *AccJerk1, t_f64 *AccJerk2, t_f64 *DecJerk1, t_f64 *DecJerk2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetJerk(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] ref double AccJerk1,
            [MarshalAs(UnmanagedType.R8)] ref double AccJerk2,
            [MarshalAs(UnmanagedType.R8)] ref double DecJerk1,
            [MarshalAs(UnmanagedType.R8)] ref double DecJerk2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetSecondaryAcc) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_f64 SecondaryAcc, t_f64 ThesholdVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetSecondaryAcc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] double SecondaryAcc,
            [MarshalAs(UnmanagedType.R8)] double ThesholdVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetSecondaryAcc) (t_i32 NetID, t_i32 Axis, t_bool *IsEnable, t_f64 *SecondaryAcc, t_f64 *ThesholdVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetSecondaryAcc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] ref double SecondaryAcc,
            [MarshalAs(UnmanagedType.R8)] ref double ThesholdVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetSecondaryDec) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_f64 SecondaryDec, t_f64 ThesholdVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetSecondaryDec(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] double SecondaryDec,
            [MarshalAs(UnmanagedType.R8)] double ThesholdVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetSecondaryDec) (t_i32 NetID, t_i32 Axis, t_bool *IsEnable, t_f64 *SecondaryDec, t_f64 *ThesholdVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetSecondaryDec(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] ref double SecondaryDec,
            [MarshalAs(UnmanagedType.R8)] ref double ThesholdVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedPatt_Ex1) (t_i32 NetID, t_i32 Axis, t_f64 VMid, t_f64 Acc2, t_bool IsApplyToDecelAlso, t_f64 Jerk, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedPatt_Ex1(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double VMid,
            [MarshalAs(UnmanagedType.R8)] double Acc2,
            [MarshalAs(UnmanagedType.I1)] bool IsApplyToDecelAlso,
            [MarshalAs(UnmanagedType.R8)] double Jerk,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        public static bool ecmSxCfg_SetRpmMode(int NetID, int Axis, bool isEnable, double PPR, ref int ErrCode)
        {
            if (isEnable)
            {
                double unit = PPR / 60;
                if (ecmSxCfg_SetUnitDist(NetID, Axis, unit, ref ErrCode))
                    ecmSxCfg_SetUnitSpeed(NetID, Axis, unit, ref ErrCode);
            }
            else
            {
                if (ecmSxCfg_SetUnitDist(NetID, Axis, 1, ref ErrCode))
                    ecmSxCfg_SetUnitSpeed(NetID, Axis, 1, ref ErrCode);
            }

            return ErrCode == 0;
        }


        ////====================== DEBUG LOGGING FUNCTIONS =========================================================//
        //EC_EXTERN t_success (CECAT_API *ecDlog_SetFilePath) (char* szLogFilePath, t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecDlog_SetFilePath( [MarshalAs(UnmanagedType.LPStr)] ref string szLogFilePath, [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        public static extern unsafe bool ecDlog_SetFilePath(
            [MarshalAs(UnmanagedType.LPStr)] string szLogFilePath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_GetFilePath) (char *pszLogFilePath, t_i32 BufLen, t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_GetFilePath(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] filePath,
            [MarshalAs(UnmanagedType.I4)] int BufLen,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_SetLogType) (int DebugLogType, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_SetLogType(
            [MarshalAs(UnmanagedType.I4)] int LogType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN int		(CECAT_API *ecDlog_GetLogType) (void);
        [DllImport(dll)]
        public static extern unsafe int ecDlog_GetLogType();

        //EC_EXTERN int		(CECAT_API *ecDlog_GetLogType) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecDlog_GetLogType(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_SetLogLevel) (int DebugLogLevel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_SetLogLevel(
            [MarshalAs(UnmanagedType.I4)] int logLevel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN int		(CECAT_API *ecDlog_GetLogLevel) (void);
        [DllImport(dll)]
        public static extern unsafe int ecDlog_GetLogLevel();

        //EC_EXTERN int		(CECAT_API *ecDlog_GetLogLevel) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecDlog_GetLogLevel(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_SetLogMemSize) (int SizeKB, int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_SetLogMemSize(
            [MarshalAs(UnmanagedType.I4)] int SizeKB,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecDlog_GetLogMemSize) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecDlog_GetLogMemSize(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_DumpMemoryLog) (int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_DumpMemoryLog(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecDlog_DumpMemoryLog2) (char* szFilePath, int *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_DumpMemoryLog2(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szFilePath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN char*		(CECAT_API *ecDlog_GetLastDumpFilePath) (t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe char[] ecDlog_GetLastDumpFilePath(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        //[return: MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)]
        //public static extern unsafe char[] ecDlog_GetLastDumpFilePath(
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecDlog_GetMemoryLogSts) (ULONGLONG *pBufSize, ULONGLONG *pWriteCount,  t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecDlog_GetMemoryLogSts(
            [MarshalAs(UnmanagedType.U8)] ref ulong pBufSize,
            [MarshalAs(UnmanagedType.U8)] ref ulong pWriteCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //====================== MOTION - Motor Performance Monitor(MPMon) FUNCTIONS ============================================//

        //EC_EXTERN t_success (CECAT_API *ecmMPMon_AddAxis) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_AddAxis(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMPMon_DelAxis) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_DelAxis(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMPMon_SetAxisEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_SetAxisEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32	    (CECAT_API *ecmMPMon_GetAxisStatus) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMPMon_GetAxisStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN ComiEcatSdk	(CECAT_API *ecmMPMon_ResetAxisData) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_ResetAxisData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMPMon_ResetSectData) (t_i32 NetID, t_i32 Axis, t_ui16 SectIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_ResetSectData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort SectIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMPMon_SetTorqThreshVal) (t_i32 NetID, t_i32 Axis, t_ui16 SectIdx, t_ui16 TorqThresholdVal, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_SetTorqThreshVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort SectIdx,
            [MarshalAs(UnmanagedType.U2)] ushort TorqThresholdVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16	(CECAT_API *ecmMPMon_GetTorqThreshVal) (t_i32 NetID, t_i32 Axis, t_ui16 SectIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecmMPMon_GetTorqThreshVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort SectIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMPMon_SetDevThreshVal) (t_i32 NetID, t_i32 Axis, t_ui16 SectIdx, t_ui32 DevThresholdVal, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_SetDevThreshVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort SectIdx,
            [MarshalAs(UnmanagedType.U4)] uint DevThresholdVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecmMPMon_GetDevThreshVal) (t_i32 NetID, t_i32 Axis, t_ui16 SectIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecmMPMon_GetDevThreshVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort SectIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMPMon_ReadAxisData) (t_i32 NetID, t_i32 Axis, TMPMonData *pBuffer, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_ReadAxisData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            ref TMPMonData monData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMPMon_WriteAxisData) (t_i32 NetID, t_i32 Axis, TMPMonData *pMPMonData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMPMon_WriteAxisData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            ref TMPMonData monData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);






        //*****************************************************************************************************************************
        //*
        //*							Functions for public System Users
        //*
        //*****************************************************************************************************************************




        ////====================== SLAVE I/F FUNCTIONS ============================================================//

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadEEPR) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 EeeprAddr, t_i32 DataSize, _out void  *pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadEEPR(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecSlv_ReadEEPR_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 EeeprAddr, t_i32 DataSize, _out void  *pBuf, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_ReadEEPR_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteEEPR) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 EeeprAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteEEPR(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteEEPR_FF) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 EeeprAddr, t_i32 DataSize, void *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteEEPR_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteEEPR_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 EeeprAddr, t_i32 DataSize, void  *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteEEPR_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteEEPR_A) (t_i32 NetID, t_i32 SlaveIdx, t_i32 EeeprAddr, t_i32 DataSize, void  *pData, _out t_i32 *ErrCode);
        [DllImport(dll, EntryPoint = "ecSlv_WriteEEPR_A")]
        public static extern unsafe int ecSlv_WriteEEPR_A_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I8)] ref long pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecSlv_WriteEEPR_A_FF) (t_i32 NetID, t_i32 SlaveIdx, t_i32 EeeprAddr, t_i32 DataSize, void  *pData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_WriteEEPR_A_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] int EeeprAddr,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_RenewCrcOfEEPR_A) (t_i32 NetID, t_ui16 SlaveIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_RenewCrcOfEEPR_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_RenewCrcOfEEPR) (t_i32 NetID, t_ui16 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_RenewCrcOfEEPR(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16		(CECAT_API *ecSlv_ReadReg134ID_A) (t_i32 NetID, t_i32 SlaveIdx, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecSlv_ReadReg134ID_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16		(CECAT_API *ecSlv_ReadReg134ID) (t_i32 NetID, t_ui16 SlvPhysAddr, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecSlv_ReadReg134ID(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_SetAlState_A) (t_i32 NetID, t_ui16 SlaveIdx, EEcAlState AlState, _out t_i32 *ErrCode);
        [DllImport(dll, EntryPoint = "ecSlv_SetAlState_A")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_SetAlState_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            EEcAlState AlState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        ////====================== MOTION - SINGLE AXIS FUNCTIONS ==================================================//

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetUnitDist) (t_i32 NetID, t_i32 Axis, t_f64 UnitDist, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetUnitDist(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double UnitDist,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetUnitDist) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetUnitDist(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetUnitSpeed) (t_i32 NetID, t_i32 Axis, t_f64 UnitSpeed, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetUnitSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double UnitSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetUnitSpeed) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetUnitSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetMioProp) (t_i32 NetID, t_i32 Axis, EEcmMioPropId PropId, t_i32 PropVal, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetMioProp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            EEcmMioPropId PropId,
            [MarshalAs(UnmanagedType.I4)] int PropVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_GetMioProp) (t_i32 NetID, t_i32 Axis, EEcmMioPropId PropId, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_GetMioProp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            EEcmMioPropId PropId,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMastInp) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_f64
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMastInp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] double inpositionRange,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_GetMastInp) (t_i32 NetID, t_i32 Axis, t_bool* IsEnable, t_f64*
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetMastInp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.R8)] ref double inpositionRange,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMastInp_Cnt) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 InpRangeCnt, t_i32 *ErrCode);
        [DllImport(dll, EntryPoint = "ecmSxCfg_SetMastInp_Cnt")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMastInp_Cnt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] int InpRangeCnt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_GetMastInp_Cnt) (t_i32 NetID, t_i32 Axis, t_bool* IsEnable, t_i32* InpRangeCnt, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetMastInp_Cnt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int InpRangeCnt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetSoftLimit) (t_i32 NetID, t_i32 Axis, BOOL* IsEnable, double* NegLimit, double* PosLimit, BOOL* IsDecelStop, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_GetSoftLimit(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int isEnable,
            [MarshalAs(UnmanagedType.R8)] ref double NegLimit,
            [MarshalAs(UnmanagedType.R8)] ref double PosLimit,
            [MarshalAs(UnmanagedType.I4)] ref int IsDecelStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetSoftLimit) (t_i32 NetID, t_i32 Axis, BOOL IsEnable, double NegLimit, double PosLimit, BOOL IsDecelStop, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSoftLimit(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int isEnable,
            [MarshalAs(UnmanagedType.R8)] double NegLimit,
            [MarshalAs(UnmanagedType.R8)] double PosLimit,
            [MarshalAs(UnmanagedType.I4)] int IsDecelStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetOperMode) (t_i32 NetID, t_i32 Axis, EEcmOperMode OperMode, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetOperMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            EEcmOperMode OperMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_GetOperMode) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_GetOperMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedPatt) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 Acc, t_f64 Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double Acc,
            [MarshalAs(UnmanagedType.R8)] double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedPatt_FF) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 Acc, t_f64 Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedPatt_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double Acc,
            [MarshalAs(UnmanagedType.R8)] double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_GetSpeedPatt) (t_i32 NetID, t_i32 Axis, t_i32 *SpeedMode, t_f64 *VIni, t_f64 *VEnd, t_f64 *VWork, t_f64 *Acc, t_f64 *Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double VIni,
            [MarshalAs(UnmanagedType.R8)] ref double VEnd,
            [MarshalAs(UnmanagedType.R8)] ref double VWork,
            [MarshalAs(UnmanagedType.R8)] ref double Acc,
            [MarshalAs(UnmanagedType.R8)] ref double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedPatt_Time) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 AccTime, t_f64 DecTime, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double AccTime,
            [MarshalAs(UnmanagedType.R8)] double DecTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedPatt_Time_FF) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 AccTime, t_f64 DecTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetSpeedPatt_Time_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double AccTime,
            [MarshalAs(UnmanagedType.R8)] double DecTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_GetSpeedPatt_Time) (t_i32 NetID, t_i32 Axis, t_i32 *SpeedMode, t_f64 *VIni, t_f64 *VEnd, t_f64 *VWork, t_f64 *AccTime, t_f64 *DecTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double VIni,
            [MarshalAs(UnmanagedType.R8)] ref double VEnd,
            [MarshalAs(UnmanagedType.R8)] ref double VWork,
            [MarshalAs(UnmanagedType.R8)] ref double AccTime,
            [MarshalAs(UnmanagedType.R8)] ref double DecTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSpeedRatio) (t_i32 NetID, t_i32 Axis, t_f64 SpeedRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double SpeedRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *CECAT_API *ecmSxCfg_SetSpeedRatio_FF) (t_i32 NetID, t_i32 Axis, t_f64 SpeedRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSpeedRatio_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double SpeedRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetSpeedRatio) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetSpeedRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetJerkRatio) (t_i32 NetID, t_i32 Axis, t_f64 JerkTimeRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetJerkRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double JerkTimeRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetJerkRatio_FF) (t_i32 NetID, t_i32 Axis, t_f64 JerkTimeRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetJerkRatio_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double JerkTimeRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetJerkRatio) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetJerkRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMinAccDecTime) (t_i32 NetID, t_i32 Axis, t_f64 MinAccTime, t_f64 MinDecTime, t_f64 LowLimitVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMinAccDecTime(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double MinAccTime,
            [MarshalAs(UnmanagedType.R8)] double MinDecTime,
            [MarshalAs(UnmanagedType.R8)] double LowLimitVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_GetMinAccDecTime) (t_i32 NetID, t_i32 Axis, t_f64 *MinAccTime, t_f64 *MinDecTime, t_f64 *LowLimitVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetMinAccDecTime(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] ref double MinAccTime,
            [MarshalAs(UnmanagedType.R8)] ref double MinDecTime,
            [MarshalAs(UnmanagedType.R8)] ref double LowLimitVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetVelCorrRatio) (t_i32 NetID, t_i32 Axis, t_f64 fVelCorrRatio /*0.0 ~ 1.0*/, t_i32 *ErrCode); // 삼각속도 보정 비율 설정. 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetVelCorrRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double fVelCorrRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetVelCorrRatio) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetVelCorrRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_SetRdpOffset) (t_i32 NetID, t_i32 Axis, t_f64 fRdpOffset, t_i32 *ErrCode); // 감속시작시점의 오프셋 설정(Ramping Down Position Offset(RDP Offset)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetRdpOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double fRdpOffset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetRdpOffset) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetRdpOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetTouchProbeFunc) (t_i32 NetID, t_i32 Axis, t_ui8 TouchProbeIndex, t_byte TouchProbeFuncVal, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetTouchProbeFunc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U1)] byte TouchProbeIndex,
            [MarshalAs(UnmanagedType.U1)] byte TouchProbeFuncVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_byte	(CECAT_API *ecmSxCfg_GetTouchProbeFunc) (t_i32 NetID, t_i32 Axis, t_ui8 TouchProbeIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecmSxCfg_GetTouchProbeFunc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U1)] byte TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSyncOtherEnv) (t_i32 NetID, t_i32 Axis, t_i32 SyncAxis, t_i32 SyncType, t_i32 PosSyncMethod, t_f64 SyncPosition, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSyncOtherEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SyncAxis,
            [MarshalAs(UnmanagedType.I4)] int SyncType,
            [MarshalAs(UnmanagedType.I4)] int PosSyncMethod,
            [MarshalAs(UnmanagedType.R8)] double SyncPosition,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetSyncOtherEnv) (t_i32 NetID, t_i32 Axis, t_i32 *SyncAxis, t_i32 *SyncType, t_i32 *PosSyncMethod, t_f64 *SyncPosition, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetSyncOtherEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int SyncAxis,
            [MarshalAs(UnmanagedType.I4)] ref int SyncType,
            [MarshalAs(UnmanagedType.I4)] ref int PosSyncMethod,
            [MarshalAs(UnmanagedType.R8)] ref double SyncPosition,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetSyncOtherEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_bool IsOneShot, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_SetSyncOtherEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I1)] bool IsOneShot,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_GetSyncOtherEnable) (t_i32 NetID, t_i32 Axis, t_bool* IsEnable, t_bool* IsOneShot, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetSyncOtherEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.I1)] ref bool IsOneShot,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr_SetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 TableSize, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_SetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableSize,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_PosCorr_GetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_PosCorr_GetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr_SetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_f64 RefCmdPos, t_f64 ActMotorPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_SetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.R8)] double RefCmdPos,
            [MarshalAs(UnmanagedType.R8)] double ActMotorPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr_GetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_f64* RefCmdPos, t_f64* ActMotorPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_GetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.R8)] ref double RefCmdPos,
            [MarshalAs(UnmanagedType.R8)] ref double ActMotorPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_PosCorr_SetTableFromFile) (t_i32 NetID, t_i32 Axis, char *szFilePath, t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode, BestFitMapping = false)]
        public static extern unsafe int ecmSxCfg_PosCorr_SetTableFromFile(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.LPStr)] string szFilePath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCfg_PosCorr_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr_ClearTable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr_ClearTable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_PosCorr2D_Reset) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_Reset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_PosCorr2D_SetTableHeader) (t_i32 NetID, t_i32 Axis, TEcmPC2DHeader PC2DHeader, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_SetTableHeader(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            TEcmPC2DHeader pc2Header,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_PosCorr2D_GetTableHeader) (t_i32 NetID, t_i32 Axis, TEcmPC2DHeader* pPC2DHeader, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_GetTableHeader(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            ref TEcmPC2DHeader pc2Header,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_PosCorr2D_SetTableData) (t_i32 NetID, t_i32 Axis, t_i32 Row, t_i32 Col, t_f32 CorrOfsData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_SetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Row,
            [MarshalAs(UnmanagedType.I4)] int Col,
            [MarshalAs(UnmanagedType.R4)] float CorrOfsData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f32		(CECAT_API *ecmSxCfg_PosCorr2D_GetTableData) (t_i32 NetID, t_i32 Axis, t_i32 Row, t_i32 Col, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.R4)]
        public static extern unsafe float ecmSxCfg_PosCorr2D_GetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Row,
            [MarshalAs(UnmanagedType.I4)] int Col,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr2D_SetTableFromFile) (t_i32 NetID, t_i32 Axis, char *szFilePath, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_SetTableFromFile(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int AxisCount,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szFilePath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_PosCorr2D_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCfg_PosCorr2D_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_PosCorr2D_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_SetValMode) (t_i32 NetID, t_i32 Axis, t_i32 ValMode, t_i32 NumMultiVals, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_SetValMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int ValMode,
            [MarshalAs(UnmanagedType.I4)] int NumMultiVals,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_AutoTorq_GetValMode) (t_i32 NetID, t_i32 Axis, t_i32 *NumMultiVals, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_AutoTorq_GetValMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int NumMultiVals,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_SetMultiVal) (t_i32 NetID, t_i32 Axis, t_i32 ValIndex, t_i32 TorqVal, t_i32 Duration, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_SetMultiVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int ValIndex,
            [MarshalAs(UnmanagedType.I4)] int TorqVal,
            [MarshalAs(UnmanagedType.I4)] int Duration,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_GetMultiVal) (t_i32 NetID, t_i32 Axis, t_i32 ValIndex, t_i32* TorqVal, t_i32* Duration, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_GetMultiVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int ValIndex,
            [MarshalAs(UnmanagedType.I4)] ref int TorqVal,
            [MarshalAs(UnmanagedType.I4)] ref int Duration,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_SetValue) (t_i32 NetID, t_i32 Axis, t_i32 OutTorqVal, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_SetValue(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int OutTorqVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_AutoTorq_GetValue) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_AutoTorq_GetValue(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_SetLimit) (t_i32 NetID, t_i32 Axis, t_ui32 LimitMask, t_f64 HighSpeedLimit, t_f64 LowSpeedLimit, t_i32 TimeLimit, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_SetLimit(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] uint LimitMask,
            [MarshalAs(UnmanagedType.R8)] double HighSpeedLimit,
            [MarshalAs(UnmanagedType.R8)] double LowSpeedLimit,
            [MarshalAs(UnmanagedType.I4)] int TimeLimit,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_GetLimit) (t_i32 NetID, t_i32 Axis, t_ui32 *LimitMask, t_f64 *HighSpeedLimit, t_f64 *LowSpeedLimit, t_i32 *TimeLimit, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_GetLimit(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] ref uint LimitMask,
            [MarshalAs(UnmanagedType.R8)] ref double HighSpeedLimit,
            [MarshalAs(UnmanagedType.R8)] ref double LowSpeedLimit,
            [MarshalAs(UnmanagedType.I4)] ref int TimeLimit,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_AutoTorq_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_bool IsOneShotEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I1)] bool IsOneShotEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCfg_AutoTorq_GetEnable) (t_i32 NetID, t_i32 Axis, t_bool *IsOneShotEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_AutoTorq_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsOneShotEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCfg_Ring_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_Ring_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_Ring_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_Ring_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_Ring_GetPosRange) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_Ring_GetPosRange(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_Ring_SetPosRange) (t_i32 NetID, t_i32 Axis, t_f64 RingPosRange, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_Ring_SetPosRange(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double RingPosRange,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_Ring_GetPosRangeEx) (t_i32 NetID, t_i32 Axis, t_bool *pbIsAbsMode, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_Ring_GetPosRangeEx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsAbsMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_Ring_SetPosRangeEx) (t_i32 NetID, t_i32 Axis, t_f64 RingPosRange, t_bool bIsAbsMode, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_Ring_SetPosRangeEx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double RingPosRange,
            [MarshalAs(UnmanagedType.I1)] bool IsAbsMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_Ring_GetDirMode) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_Ring_GetDirMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCfg_Ring_SetDirMode) (t_i32 NetID, t_i32 Axis, t_i32 RingDirMode, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_Ring_SetDirMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int RingDirMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_ExtStop_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_ExtStop_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool isEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCfg_ExtStop_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_ExtStop_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_ExtStop_SetEnv) (t_i32 NetID, t_i32 Axis, TEcLogicAddr SigAddr, t_i32 ActiveState, t_i32 DelayTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_ExtStop_SetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            TEcLogicAddr SigAddr,
            [MarshalAs(UnmanagedType.I4)] int ActiveState,
            [MarshalAs(UnmanagedType.I4)] int DelayTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_ExtStop_GetEnv) (t_i32 NetID, t_i32 Axis, TEcLogicAddr *pSigAddr, t_i32 *pActiveState, t_i32 *pDelayTime, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_ExtStop_GetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            ref TEcLogicAddr SigAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ActiveState,
            [MarshalAs(UnmanagedType.I4)] ref int DelayTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_ExtStop_SetEnv2) (t_i32 NetID, t_i32 Axis, TEcLogicAddr SigAddr, t_i32 ActiveLogic, t_f64 OfsDist, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_ExtStop_SetEnv2(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            TEcLogicAddr SigAddr,
            [MarshalAs(UnmanagedType.I4)] int ActiveLogic,
            [MarshalAs(UnmanagedType.R8)] double OfsDist,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_ExtStop_GetEnv2) (t_i32 NetID, t_i32 Axis, TEcLogicAddr *pSigAddr, t_i32 *ActiveLogic, t_f64 *pOfsDist, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_ExtStop_GetEnv2(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            ref TEcLogicAddr SigAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ActiveLogic,
            [MarshalAs(UnmanagedType.R8)] ref double OfsDist,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_ExtStop_SetEnv3) (t_i32 NetID, t_i32 Axis, t_ui8 TouchProbeIndex, t_i32 ActiveLogic, t_f64 OfsDist, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_ExtStop_SetEnv3(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] byte TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] int ActiveLogic,
            [MarshalAs(UnmanagedType.R8)] double OfsDist,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCfg_ExtStop_GetEnv3) (t_i32 NetID, t_i32 Axis, t_ui8 *TouchProbeIndex, t_i32 *ActiveLogic, t_f64 *pOfsDist, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCfg_ExtStop_GetEnv3(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref byte TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ActiveLogic,
            [MarshalAs(UnmanagedType.R8)] ref double OfsDist,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //t_success ecmSxCfg_ExtStop_SetOfsDistMode (t_i32 NetID, t_i32 Axis, t_bool IsHardOfsDistMode, t_i32 *ErrCode)
        [DllImport(dll, EntryPoint = "ecmSxCfg_ExtStop_SetOfsDistMode")]
        public static extern unsafe bool ecmSxCfg_ExtStop_SetOfsDistMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsHardOfsDistMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //t_bool ecmSxCfg_ExtStop_GetOfsDistMode (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode)
        [DllImport(dll, EntryPoint = "ecmSxCfg_ExtStop_GetOfsDistMode")]
        public static extern unsafe bool ecmSxCfg_ExtStop_GetOfsDistMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetZVISEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode); // KKJ : Input Shaping Function Enable
        [DllImport(dll, EntryPoint = "ecmSxCfg_SetZVISEnable")]
        public static extern unsafe int ecmSxCfg_SetZVISEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool isEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN bool	(CECAT_API *ecmSxCfg_GetZVISEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); // KKJ : Input Shaping Function Enable
        [DllImport(dll, EntryPoint = "ecmSxCfg_GetZVISEnable")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetZVISEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCfg_SetZVISParam) (t_i32 NetID, t_i32 Axis, t_f64 NaturalFrequency, t_f64 DampingRatio, t_i32 ZVISMode, t_i32 *ErrCode); // KKJ : Input Shaping Parameter
        [DllImport(dll, EntryPoint = "ecmSxCfg_SetZVISParam")]
        public static extern unsafe int ecmSxCfg_SetZVISParam(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double NaturalFrequency,
            [MarshalAs(UnmanagedType.R8)] double DampingRatio,
            [MarshalAs(UnmanagedType.I4)] int ZVISMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecmSxCfg_GetZVISParam")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_GetZVISParam(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] ref double NaturalFrequency,
            [MarshalAs(UnmanagedType.R8)] ref double DampingRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ZVISMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_SetSvon) (t_i32 NetID, t_i32 Axis, t_i32 SvonVal, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_SetSvon(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SvonVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_SetSvon_FF) (t_i32 NetID, t_i32 Axis, t_i32 SvonVal, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_SetSvon_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SvonVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCtl_GetSvon) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCtl_GetSvon(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_SetAlmRst) (t_i32 NetID, t_i32 Axis, t_i32 IsSetAlmRst, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_SetAlmRst(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int IsSetAlmRst,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_SetAlmRst_FF) (t_i32 NetID, t_i32 Axis, t_i32 IsSetAlmRst, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_SetAlmRst_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int IsSetAlmRst,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_ResetAlm) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_ResetAlm(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxCtl_ResetAlm_FF) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCtl_ResetAlm_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_SetTargTorq) (t_i32 NetID, t_i32 Axis, t_i32 TargTorq, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_SetTargTorq(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TargTorq,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxMot_GetTargTorq) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_GetTargTorq(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_SetTargVel) (t_i32 NetID, t_i32 Axis, t_f64 TargVel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_SetTargVel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double TargVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxMot_GetTargVel) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);        
        [DllImport(dll)]
        public static extern unsafe double ecmSxMot_GetTargVel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_VMoveStart) (t_i32 NetID, t_i32 Axis, t_i32 Dir, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_VMoveStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Dir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_VMoveStart_FF) (t_i32 NetID, t_i32 Axis, t_i32 Dir, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_VMoveStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Dir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_MoveStart) (t_i32 NetID, t_i32 Axis, t_f64 Distance, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_MoveStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Distance,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_MoveStart_FF) (t_i32 NetID, t_i32 Axis, t_f64 Distance, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_MoveStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Distance,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_Move) (t_i32 NetID, t_i32 Axis, t_f64 Distance, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_Move(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Distance,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_Move_NB) (t_i32 NetID, t_i32 Axis, t_f64 Distance, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_Move_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Distance,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_MoveToStart) (t_i32 NetID, t_i32 Axis, t_f64 Position, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_MoveToStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Position,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_MoveToStart_FF) (t_i32 NetID, t_i32 Axis, t_f64 Position, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_MoveToStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Position,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_MoveTo) (t_i32 NetID, t_i32 Axis, t_f64 Position, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_MoveTo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Position,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxMot_MoveTo_NB) (t_i32 NetID, t_i32 Axis, t_f64 Position, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxMot_MoveTo_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Position,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_OverrideSpeed) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_OverrideSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_OverrideMove) (t_i32 NetID, t_i32 Axis, t_f64 NewDist, _out t_bool *IsIgnored, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_OverrideMove(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double NewDist,
            [MarshalAs(UnmanagedType.I1)] ref bool IsIgnored,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_OverrideMoveTo) (t_i32 NetID, t_i32 Axis, t_f64 NewPos, _out t_bool *IsIgnored, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_OverrideMoveTo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double NewPos,
            [MarshalAs(UnmanagedType.I1)] ref bool IsIgnored,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_Stop) (t_i32 NetID, t_i32 Axis, t_i32 IsDecStop, t_i32 IsWaitCompt, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_Stop(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] int IsWaitCompt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMot_Stop_FF) (t_i32 NetID, t_i32 Axis, t_i32 IsDecStop, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMot_Stop_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxSt_IsConnected)(t_i32 NetID, t_i32 Axis, t_i32 *ConnStsDetail, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSt_IsConnected(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ConnStsDetail,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN void*		(CECAT_API *ecmSxSt_GetInPDOPtr)(t_i32 NetID, t_i32 Axis, t_ui8 *PDODataType, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcmInPDO_AxisType0 ecmSxSt_GetInPDOPtr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U1)] ref byte PDODataType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxSt_IsBusy) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecmSxSt_IsBusy(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxSt_WaitCompt) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSt_WaitCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxSt_WaitCompt_NB) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSt_WaitCompt_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxSt_SetCount) (t_i32 NetID, t_i32 Axis, t_i32 NewPosCount, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_SetCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int NewPosCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32 	(CECAT_API *ecmSxSt_GetCount) (t_i32 NetID, t_i32 Axis, t_i32 TargCntr, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_GetCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TargCntr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxSt_SetPosition) (t_i32 NetID, t_i32 Axis, t_f64 NewPosition, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_SetPosition(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double NewPosition,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxSt_GetPosition) (t_i32 NetID, t_i32 Axis, t_i32 TargCntr, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxSt_GetPosition(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TargCntr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxSt_GetCurSpeed) (t_i32 NetID, t_i32 Axis, t_i32 TargCntr, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxSt_GetCurSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TargCntr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxSt_GetCurTorque) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxSt_GetCurTorque(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxSt_GetMotState) (t_i32 NetID, t_i32 Axis, _out t_i32 *SubErrData);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_GetMotState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_word	(CECAT_API *ecmSxSt_GetFlags) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecmSxSt_GetFlags(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_word	(CECAT_API *ecmSxSt_GetDI) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecmSxSt_GetDI(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_byte	(CECAT_API *ecmSxSt_GetTouchProbeSts) (t_i32 NetID, t_i32 Axis, t_i32 TouchProbeIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecmSxSt_GetTouchProbeSts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxSt_GetTouchProbePos) (t_i32 NetID, t_i32 Axis, t_i32 TouchProbeIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxSt_GetTouchProbePos(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxSt_GetTouchProbePosN) (t_i32 NetID, t_i32 Axis, t_i32 TouchProbeIndex, t_i32 *ErrCode); // TouchProbe negative edge position 반환. TouchProbeIndex 인자는 0번부터 시작함.
        [DllImport(dll)]
        public static extern unsafe double ecmSxSt_GetTouchProbePosN(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TouchProbeIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui8		(CECAT_API *ecmSxSt_GetOpModeDisp) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecmSxSt_GetOpModeDisp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32	(CECAT_API *ecmSxSt_GetLmMapIdx) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_GetLmMapIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32	(CECAT_API *ecmSxSt_GetPtmMapIdx) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxSt_GetPtmMapIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMaxSpdOfCT) (t_i32 NetID, t_i32 Axis, t_ui32 MaxSpd, _out t_i32 *ErrCode); // Cyclic Torq Mode의 Max Speed setting. MaxSpd의 단위는 rpm값으로 설정한다. 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMaxSpdOfCT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] uint MaxSpd,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecmSxCfg_GetMaxSpdOfCT) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode); // Cyclic Torq Mode의 Max Speed 값을 반환. 반환값의 단위는 rpm이다.
        [DllImport(dll)]
        public static extern unsafe uint ecmSxCfg_GetMaxSpdOfCT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMaxProfSpdOfCT) (t_i32 NetID, t_i32 Axis, t_f64 MaxProfSpd, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMaxProfSpdOfCT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double MaxProfSpd,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmSxCfg_GetMaxProfSpdOfCT) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmSxCfg_GetMaxProfSpdOfCT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmSxCfg_SetMaxTorqOfCV) (t_i32 NetID, t_i32 Axis, t_ui16 MaxTorq, _out t_i32 *ErrCode); // Cyclic Vel. Mode의 Max Torque setting. MaxTorq의 단위는 0.1% 값으로 설정한다. 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCfg_SetMaxTorqOfCV(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U2)] ushort MaxTorq,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui16	(CECAT_API *ecmSxCfg_GetMaxTorqOfCV) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode); // Cyclic Vel. Mode의 Max Torque 값을 반환. 반환값의 단위는 0.1%이다.
        [DllImport(dll)]
        public static extern unsafe ushort ecmSxCfg_GetMaxTorqOfCV(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== MOTION - POSITION COMPARE OUTPUT FUNCTIONS =================================================//


        public delegate void CallbackFunc(IntPtr lParam);


#if true // 1.5.0.9 이후 
        //C_EXTERN t_bool	(CECAT_API *ecmSxCmpOne_SetChannel) (t_i32 NetID, t_i32 Axis, t_ui32 OutChan_LogBitAddr, t_i32 OutSigLogic, t_i32 OutSigOnTime, t_i32 *ErrCode); 
        // CMP 출력 신호로 사용할 출력 채널 설정, -OutChan_LogBitAddr: ecdiLogBitAddr_FromXXXX()함수군 참고
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] uint OutChan_LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] int OutSigLogic,
            [MarshalAs(UnmanagedType.I4)] int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpOne_GetChannel) (t_i32 NetID, t_i32 Axis, t_ui32 *OutChan_LogBitAddr, t_i32 *OutSigLogic, t_i32 *OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] ref uint OutChan_LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigLogic,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_SetCondition) (t_i32 NetID, t_i32 Axis, t_i32 CmpCntrType, t_i32 CmpMethod, t_f64 CmpRefPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetCondition(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_GetCondition) (t_i32 NetID, t_i32 Axis, t_i32 *CmpCntrType, t_i32 *CmpMethod, t_f64 *CmpRefPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetCondition(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] ref double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpOne_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_GetOutResult) (t_i32 NetID, t_i32 Axis, t_i32 *OutCount, t_f64 *LastOutPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetOutResult(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int OutCount,
            [MarshalAs(UnmanagedType.R8)] ref double LastOutPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_ClearOutResult) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_ClearOutResult(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_SetOutputState) (t_i32 NetID, t_i32 Axis, t_bool OutputState, t_i32 *ErrCode); // 트리거출력 채널 ON/OFF
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetOutputState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool OutputState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpOne_GetOutputState) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetOutputState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_ManualTrgOut) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); // 트리거 신호 수동 출력 (OutSigLogic, OutSigOnTime 반영)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_ManualTrgOut(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecmSxCmpOne_SetHandler) (t_i32 NetID, t_i32 Axis, t_i32 HandlerType, HANDLE Handler, UINT nMessage, LPARAM lParam, t_i32 *ErrCode); // HandlerType: EEcmHandlerType 형선언 참고
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetHandler_MSG(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int handlerType,
            IntPtr handler,
            [MarshalAs(UnmanagedType.U4)] uint message,
            IntPtr LPARAM,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecmSxCmpOne_SetHandler) (t_i32 NetID, t_i32 Axis, t_i32 HandlerType, HANDLE Handler, UINT nMessage, LPARAM lParam, t_i32 *ErrCode); // HandlerType: EEcmHandlerType 형선언 참고
        [DllImport(dll, EntryPoint = "ecmSxCmpOne_SetHandler")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetHandler_CLB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int handlerType,
            CallbackFunc handler,
            [MarshalAs(UnmanagedType.U4)] uint message,
            IntPtr LPARAM,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpCont_SetChannel) (t_i32 NetID, t_i32 Axis, t_ui32 OutChan_LogBitAddr, t_i32 OutSigLogic, t_i32 OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] uint OutChan_LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] int OutSigLogic,
            [MarshalAs(UnmanagedType.I4)] int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpCont_GetChannel) (t_i32 NetID, t_i32 Axis, t_ui32 *OutChan_LogBitAddr, t_i32 *OutSigLogic, t_i32 *OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] ref uint OutChan_LogBitAddr,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigLogic,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 TableSize, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableSize,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_i32		(CECAT_API *ecmSxCmpCont_GetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCmpCont_GetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32 CmpCntrType, t_i32 CmpMethod, t_f64 CmpRefPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.I4)] int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_GetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32* CmpCntrType, t_i32* CmpMethod, t_f64* CmpRefPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.I4)] ref int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] ref double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_i32		(CECAT_API *ecmSxCmpCont_GetActTblIdx) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCmpCont_GetActTblIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetActTblIdx) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetActTblIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpCont_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_GetOutResult) (t_i32 NetID, t_i32 Axis, t_i32 *OutCount, t_f64 *LastOutPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetOutResult(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int OutCount,
            [MarshalAs(UnmanagedType.R8)] ref double LastOutPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_ClearOutResult) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_ClearOutResult(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetOutputState) (t_i32 NetID, t_i32 Axis, t_bool OutputState, t_i32 *ErrCode);  // 트리거출력 채널 ON/OFF
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetOutputState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool OutputState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpCont_GetOutputState) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetOutputState(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_ManualTrgOut) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); // 트리거 신호 수동 출력 (OutSigLogic, OutSigOnTime 반영)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_ManualTrgOut(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


#else
        //EC_EXTERN t_success (CECAT_API *ecmSxCmpOne_SetHandler) (t_i32 NetID, t_i32 Axis, t_i32 HandlerType, HANDLE Handler, UINT nMessage, LPARAM lParam, t_i32 *ErrCode); // HandlerType: EEcmHandlerType 선언 참고
        [DllImport(dll, EntryPoint = "ecmSxCmpOne_SetHandler")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetHandler_MSG(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int handlerType,
            IntPtr handler,
            [MarshalAs(UnmanagedType.U4)] uint message,
            IntPtr LPARAM,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll, EntryPoint = "ecmSxCmpOne_SetHandler")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetHandler_CLB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int handlerType,
            CallbackFunc handler,
            [MarshalAs(UnmanagedType.U4)] uint message,
            IntPtr LPARAM,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 GdoChanIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I1)] bool isEnable, 
            [MarshalAs(UnmanagedType.I4)] int GdoChanIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpOne_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32* GdoChanIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int GdoChanIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_SetEnv) (t_i32 NetID, t_i32 Axis, t_i32 CmpCntrType, t_i32 CmpMethod, t_f64 CmpRefPos, t_i32 OutSigLogic, t_i32 OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_SetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] double CmpRefPos, 
            [MarshalAs(UnmanagedType.I4)] int OutSigLogic,
            [MarshalAs(UnmanagedType.I4)] int OutSigOnTime, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_GetEnv) (t_i32 NetID, t_i32 Axis, t_i32* CmpCntrType, t_i32* CmpMethod, t_f64* CmpRefPos, t_i32* OutSigLogic, t_i32* OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpOne_GetEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] ref int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] ref double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigLogic, 
            [MarshalAs(UnmanagedType.I4)] ref int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_GetOutSts) (t_i32 NetID, t_i32 Axis, t_i32 *OutCount, t_f64 *LastOutPos, t_i32 *ErrCode);
        //[DllImport("ComiEcatSdk.dll", EntryPoint = "ecmSxCmpOne_GetOutSts")]
        //[return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecmSxCmpOne_GetOutSts(
        //    [MarshalAs(UnmanagedType.I4)] int NetID,
        //    [MarshalAs(UnmanagedType.I4)] int Axis,
        //    [MarshalAs(UnmanagedType.I4)] ref int OutCount, 
        //    [MarshalAs(UnmanagedType.R8)] ref double LastOutPos, 
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        // DLL에 없음

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpOne_ResetOutSts) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        //[DllImport("ComiEcatSdk.dll", EntryPoint = "ecmSxCmpOne_ResetOutSts")]
        //[return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecmSxCmpOne_ResetOutSts(
        //    [MarshalAs(UnmanagedType.I4)] int NetID,
        //    [MarshalAs(UnmanagedType.I4)] int Axis,
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        // DLL에 없음

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 GdoChanIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool isEnable, 
            [MarshalAs(UnmanagedType.I4)] int GdoChanIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxCmpCont_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32* GdoChanIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int GdoChanIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 TableSize, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] int TableSize, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCmpCont_GetTableSize) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCmpCont_GetTableSize(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32 CmpCntrType, t_i32 CmpMethod, t_f64 CmpRefPos, t_i32 OutSigLogic, t_i32 OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex, 
            [MarshalAs(UnmanagedType.I4)] int CmpCntrType,
            [MarshalAs(UnmanagedType.I4)] int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] int OutSigLogic, 
            [MarshalAs(UnmanagedType.I4)] int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_GetTableData) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32* CmpCntrType, t_i32* CmpMethod, t_f64* CmpRefPos, t_i32* OutSigLogic, t_i32* OutSigOnTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetTableData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] int TableIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int CmpCntrType, 
            [MarshalAs(UnmanagedType.I4)] ref int CmpMethod,
            [MarshalAs(UnmanagedType.R8)] ref double CmpRefPos,
            [MarshalAs(UnmanagedType.I4)] ref int OutSigLogic, 
            [MarshalAs(UnmanagedType.I4)] ref int OutSigOnTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmSxCmpCont_GetActTblIdx) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxCmpCont_GetActTblIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_SetActTblIdx) (t_i32 NetID, t_i32 Axis, t_i32 TableIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_SetActTblIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int TableIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_GetOutSts) (t_i32 NetID, t_i32 Axis, t_i32 *OutCount, t_f64 *LastOutPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_GetOutSts(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int Axis, 
            [MarshalAs(UnmanagedType.I4)] ref int OutCount, 
            [MarshalAs(UnmanagedType.R8)] ref double LastOutPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmSxCmpCont_ResetOutSts) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxCmpCont_ResetOutSts(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
#endif

        //====================== SLAVE ABCMP FUNCTIONS ==========================================================//

        //EC_EXTERN t_success (CECAT_API *ecTrg_InitCmp) 		(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_InitCmp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecTrg_SetEnc) 		(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 Mode, t_i32 EncInitCnt, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetEnc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int Mode,
            [MarshalAs(UnmanagedType.I4)] int EncInitCnt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecTrg_SetCmp) 		(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 Mode, t_i32 Width, t_i32 Margin, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetCmp(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int Mode,
            [MarshalAs(UnmanagedType.I4)] int Width,
            [MarshalAs(UnmanagedType.I4)] int Margin,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_SetEncEnable) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetEncEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_SetCmpEnable) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 IsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetCmpEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_GetCmpStatus) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 *CmpPos, t_i32 *CmpCnt, t_i32 *FaultCnt, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_GetCmpStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] ref int CmpPos,
            [MarshalAs(UnmanagedType.I4)] ref int CmpCnt,
            [MarshalAs(UnmanagedType.I4)] ref int FaultCnt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_GetEncStatus) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 *EncCntX, t_i32 *EncCntY, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_GetEncStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] ref int EncCntX,
            [MarshalAs(UnmanagedType.I4)] ref int EncCntY,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_SetCmpData) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 Addr, t_i32 CntX, t_i32 CntY, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetCmpData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int Addr,
            [MarshalAs(UnmanagedType.I4)] int CntX,
            [MarshalAs(UnmanagedType.I4)] int CntY,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_SetCmpNum) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 cmpNum, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetCmpNum(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] int cmpNum,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_GetCmpFaultData) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 Addr, t_i32 *CntX, t_i32 *CntY, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_GetCmpFaultData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.I4)] ref int CntX,
            [MarshalAs(UnmanagedType.I4)] ref int CntY,
            [MarshalAs(UnmanagedType.I4)] int Addr,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecTrg_SetCmpTable) 	(t_i32 NetID, t_i32 SlvPhysAddr, t_i32 Channel, t_i32 *pTable, t_i32 dataNum, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecTrg_SetCmpTable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int Channel,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] pTable,
            [MarshalAs(UnmanagedType.I4)] int dataNum,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== MOTION - MultiTorq FUNCTIONS =================================================//

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMulTorq1_Start) (t_i32 NetID, t_i32 Axis, t_i16 InitialTorq, TEcmMTQ1Item NextTorqList[], t_ui16 NumNextTorqItems, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMulTorq1_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I2)] short InitialTorq,
            TEcmMTQ1Item[] NextTorqList,
            [MarshalAs(UnmanagedType.U2)] ushort NumNextTorqItems,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmSxMulTorq1_Stop) (t_i32 NetID, t_i32 Axis, t_i16 StopTorq, t_bool IsApplyStopTorq, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmSxMulTorq1_Stop(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I2)] short StopTorq,
            [MarshalAs(UnmanagedType.I1)] bool IsApplyStopTorq,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== MOTION - Slow Down(SD) for Single Axis FUNCTIONS =================================================//

        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_SetInputEnv) (t_i32 NetID, t_i32 Axis, t_dword LogBitAddr, t_bool IsInvertLogic, t_i32 FilterCount, t_i32 *ErrCode); 
        // SD 입력 신호에 대한 환경을 설정한다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_SetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_GetInputEnv) (t_i32 NetID, t_i32 Axis, t_dword *LogBitAddr, t_bool *IsInvertLogic, t_i32 *FilterCount, t_i32 *ErrCode); 
        // SD 입력 신호에 대한 환경 설정값을 읽는다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.U4)] ref uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] ref bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] ref int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_SetSpeed) (t_i32 NetID, t_i32 Axis, t_f64 SdSpeed, t_i32 *ErrCode); 
        // SD 속도를 설정한다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_SetSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double SdSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_f64		(CECAT_API *ecmSxSD_GetSpeed) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode);
        // SD 속도 설정값을 반환한다.
        [DllImport(dll)]
        public static extern unsafe double ecmSxSD_GetSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_SetOffset) (t_i32 NetID, t_i32 Axis, t_i32 OffsetMode, t_f64 Offset, t_i32 *ErrCode); 
        // SD OFFSET을 설정한다. OffsetMode에 설정가능한 값은 EEcmSdOfsMode 형선언 참조.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_SetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int OffsetMode,
            [MarshalAs(UnmanagedType.R8)] double Offset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_GetOffset) (t_i32 NetID, t_i32 Axis, t_i32 *OffsetMode, t_f64 *Offset, t_i32 *ErrCode); 
        // SD OFFSET 설정을 읽는다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int OffsetMode,
            [MarshalAs(UnmanagedType.R8)] ref double Offset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_SetLatchMode) (t_i32 NetID, t_i32 Axis, t_bool IsLatchMode, t_i32 *ErrCode); 
        // SD 신호의 Latch 모드를 설정한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_SetLatchMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsLatchMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxSD_GetLatchMode) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); 
        // SD 신호의 Latch 모드를 설정값을 반환한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetLatchMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_SetEnable) (t_i32 NetID, t_i32 Axis, t_bool IsEnable, t_i32 *ErrCode); 
        // SD 기능을 활성화 또는 비활성화한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxSD_GetEnable) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); 
        // SD 기능의 활성화 여부를 반환한다. 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmSxSD_RestoreSpeed) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); 
        // Slow Down 속도로 진행 중인 경우에 원래의 속도로 복귀하도록 한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_RestoreSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmSxSD_GetInputStatus) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); 
        // SD 신호의 상태를 반환다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetInputStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmSxSD_GetActStatus) (t_i32 NetID, t_i32 Axis, t_i32 *ErrCode); 
        // SD 속도로 진행중인지에 대한 상태를 반환한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxSD_GetActStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //====================== MOTION - Digital Output for Single Axis FUNCTIONS =================================================//

        //EC_EXTERN t_bool(CECAT_API* ecmSxDO_GetOne) (t_i32 NetID, t_i32 Axis, t_i32 BitIndex, t_i32* ErrCode); // 각 축에서 제공하는 Digital Output 중에서 지정한 한 채널의 값을 반환한다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxDO_GetOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int BitIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecmSxDO_PutOne) (t_i32 NetID, t_i32 Axis, t_i32 BitIndex, t_bool OutState, t_i32* ErrCode); // 각 축에서 제공하는 Digital Output 중에서 지정한 한 채널의 값을 출력한다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxDO_PutOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int BitIndex,
            [MarshalAs(UnmanagedType.I1)] bool OutState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword(CECAT_API* ecmSxDO_GetMulti) (t_i32 NetID, t_i32 Axis, t_i32* ErrCode); // 각 축에서 제공하는 Digital Output 32채널에 대한 값을 32비트 값으로 반환한다.
        [DllImport(dll)]
        public static extern unsafe int ecmSxDO_GetMulti(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecmSxDO_PutMulti) (t_i32 NetID, t_i32 Axis, t_dword OutStates, t_i32* ErrCode); // 각 축에서 제공하는 Digital Output 32채널에 대한 값을 32비트 값으로 출력한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxDO_PutMulti(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int OutState,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success   (CECAT_API *ecmSxDO_SetBitMask) (t_i32 NetID, t_i32 Axis, t_bool IsBitmaskEnabled, t_dword BitmaskValue, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmSxDO_SetBitMask(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] bool IsBitmaskEnabled,
            [MarshalAs(UnmanagedType.I4)] int BitmaskValue,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //void      (CECAT_API *ecmSxDO_GetBitMask) (t_i32 NetID, t_i32 Axis, t_bool *IsBitmaskEnabled, t_dword *BitmaskValue, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe void ecmSxDO_GetBitMask(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsBitmaskEnabled,
            [MarshalAs(UnmanagedType.I4)] ref int BitmaskValue,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        ////====================== MOTION - MULTIPLE AXES FUNCTIONS =================================================//

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_VMoveStart) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 DirList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_VMoveStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_VMoveStart_FF) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 DirList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_VMoveStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_MoveStart) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_MoveStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_MoveStart_FF) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_MoveStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_MoveToStart) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_MoveToStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_MoveToStart_FF) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_MoveToStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_Stop) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 IsDecStop, t_i32 IsWaitCompt, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_Stop(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] int IsWaitCompt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmMxMot_Stop_FF) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 IsDecStop, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmMxMot_Stop_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmMxSt_IsBusy) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMxSt_IsBusy(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMxSt_WaitCompt) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMxSt_WaitCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmMxSt_WaitCompt_NB) (t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMxSt_WaitCompt_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        ////====================== MOTION - INTERPOLATION FUNCTIONS ================================================//
        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_MapAxes) (t_i32 NetID, t_i32 MapIndex, t_i32 NumAxes, t_i32 AxisList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_MapAxes(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_MapAxes_FF) (t_i32 NetID, t_i32 MapIndex, t_i32 NumAxes, t_i32 AxisList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_MapAxes_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_UnmapAxes) (t_i32 NetID, t_i32 MapIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_UnmapAxes(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_UnmapAxes_FF) (t_i32 NetID, t_i32 MapIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_UnmapAxes_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetSpeedPatt) (t_i32 NetID, t_i32 MapIndex, t_i32 IsVectSpeed, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 Acc, t_f64 Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int IsVectSpeed,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double Acc,
            [MarshalAs(UnmanagedType.R8)] double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetSpeedPatt_FF) (t_i32 NetID, t_i32 MapIndex, t_i32 IsVectSpeed, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 Acc, t_f64 Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetSpeedPatt_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int IsVectSpeed,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double Acc,
            [MarshalAs(UnmanagedType.R8)] double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_GetSpeedPatt) (t_i32 NetID, t_i32 MapIndex, t_i32 *IsVectSpeed, t_i32 *SpeedMode, t_f64 *VIni, t_f64 *VEnd, t_f64 *VWork, t_f64 *Acc, t_f64 *Dec, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_GetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int IsVectSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double VIni,
            [MarshalAs(UnmanagedType.R8)] ref double VEnd,
            [MarshalAs(UnmanagedType.R8)] ref double VWork,
            [MarshalAs(UnmanagedType.R8)] ref double Acc,
            [MarshalAs(UnmanagedType.R8)] ref double Dec,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetSpeedPatt_Time) (t_i32 NetID, t_i32 MapIndex, t_i32 IxSpdPattType, t_i32 SpeedMode, t_f64 VIni, t_f64 VEnd, t_f64 VWork, t_f64 AccTime, t_f64 DecTime, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int IsVectSpeed,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double VIni,
            [MarshalAs(UnmanagedType.R8)] double VEnd,
            [MarshalAs(UnmanagedType.R8)] double VWork,
            [MarshalAs(UnmanagedType.R8)] double AccTime,
            [MarshalAs(UnmanagedType.R8)] double DecTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_GetSpeedPatt_Time) (t_i32 NetID, t_i32 MapIndex, t_i32 *IxSpdPattType, t_i32 *SpeedMode, t_f64 *VIni, t_f64 *VEnd, t_f64 *VWork, t_f64 *AccTime, t_f64 *DecTime, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_GetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int IsVectSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double VIni,
            [MarshalAs(UnmanagedType.R8)] ref double VEnd,
            [MarshalAs(UnmanagedType.R8)] ref double VWork,
            [MarshalAs(UnmanagedType.R8)] ref double AccTime,
            [MarshalAs(UnmanagedType.R8)] ref double DecTime,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetJerkRatio) (t_i32 NetID, t_i32 MapIndex, t_f64 JerkTimeRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetJerkRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double JerkTimeRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetJerkRatio_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 JerkTimeRatio, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetJerkRatio_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double JerkTimeRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmIxCfg_GetJerkRatio) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmIxCfg_GetJerkRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetSpeedPatt_MR) (t_i32 NetID, t_i32 MapIndex, t_i32 SpeedMode, t_f64 MasterRatio, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetSpeedPatt_MR(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double MasterRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetSpeedPatt_MR_FF) (t_i32 NetID, t_i32 MapIndex, t_i32 SpeedMode, t_f64 MasterRatio, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetSpeedPatt_MR_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double MasterRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_GetSpeedPatt_MR) (t_i32 NetID, t_i32 MapIndex, t_i32 *SpeedMode, t_f64 *MasterRatio, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_GetSpeedPatt_MR(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double MasterRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxCfg_SetMastWeight) (t_i32 NetID, t_i32 MapIndex, t_f64 *MastWeightList, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_SetMastWeight(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] ref double MastWeightList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_GetMastWeight) (t_i32 NetID, t_i32 MapIndex, t_f64 *MastWeightList, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_GetMastWeight(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] ref double MastWeightList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_ClearPool) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_ClearPool(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_handle32 (CECAT_API *ecmIxCfg_Spline_AddNewObj) (t_i32 NetID, t_i32 MapIndex,  t_ui32 NumRefPoints, t_bool IsAbsPosMode, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr ecmIxCfg_Spline_AddNewObj(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.U4)] uint NumRefPoints,
            [MarshalAs(UnmanagedType.I1)] bool IsAbsPosMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_Spline_SetRefPoint) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 PointIndex, t_f64 Point[], t_i32 NumPointData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_SetRefPoint(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int PointIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] Point,
            [MarshalAs(UnmanagedType.I4)] int NumPointData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_Spline_BuildObj) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_BuildObj(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_SetTrgOutEnv) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_bool IsEnableTrgOut, t_i16 TrgOfsTime_ms, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_SetTrgOutEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I1)] bool IsEnableTrgOut,
            [MarshalAs(UnmanagedType.I2)] short TrgOfsTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_GetTrgOutEnv) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_bool* IsEnableTrgOut, t_i16* TrgOfsTime_ms, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_GetTrgOutEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnableTrgOut,
            [MarshalAs(UnmanagedType.I2)] ref short TrgOfsTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmIxCfg_Spline_GetTrgOutPos) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 Axis, t_i32 PointIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmIxCfg_Spline_GetTrgOutPos(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int PointIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_SetJsType) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 JerkSmoothType, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_SetJsType(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int JerkSmoothType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmIxCfg_Spline_GetJsType) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxCfg_Spline_GetJsType(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_SetJsProp_F) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 PropId, t_f64 PropVal, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_SetJsProp_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int PropId,
            [MarshalAs(UnmanagedType.R8)] double PropVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmIxCfg_Spline_GetJsProp_F) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 PropId, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmIxCfg_Spline_GetJsProp_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int PropId,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxCfg_Spline_SetJerkThresh) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 Axis, t_f64 JerkThreshold, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_Spline_SetJerkThresh(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double JerkThreshold,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64		(CECAT_API *ecmIxCfg_Spline_GetJerkThresh) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_i32 Axis, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmIxCfg_Spline_GetJerkThresh(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_MPRLin2X_ClearPool) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_MPRLin2X_ClearPool(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_handle32 (CECAT_API *ecmIxCfg_MPRLin2X_AddNewObj) (t_i32 NetID, t_i32 MapIndex,  t_ui32 NumRefPoints, t_bool IsAbsPosMode, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr ecmIxCfg_MPRLin2X_AddNewObj(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.U4)] uint NumRefPoints,
            [MarshalAs(UnmanagedType.I1)] bool IsAbsPosMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_MPRLin2X_SetRefPoint) (t_i32 NetID, t_i32 MapIndex, t_handle32 hMPRLin2XObj, t_i32 PointIndex, t_f64 Point[], t_f64 RoundData, t_i32 RoundDataType, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_MPRLin2X_SetRefPoint(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hMPRLin2XObj,
            [MarshalAs(UnmanagedType.I4)] int PointIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] Point,
            [MarshalAs(UnmanagedType.R8)] double RoundData,
            [MarshalAs(UnmanagedType.I4)] int RoundDataType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxCfg_MPRLin2X_BuildObj) (t_i32 NetID, t_i32 MapIndex, t_handle32 hMPRLin2XObj, _out t_f64 LastPointBuf[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxCfg_MPRLin2X_BuildObj(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hMPRLin2XObj,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] LastPointBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_LineStart) (t_i32 NetID, t_i32 MapIndex, t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_LineStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_LineStart_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_LineStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_Line) (t_i32 NetID, t_i32 MapIndex, t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Line(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_Line_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 DistList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Line_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] DistList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success(CECAT_API* ecmIxMot_Line_OverrideSpeed) (t_i32 NetID, t_i32 MapIndex, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Line_OverrideSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_LineToStart) (t_i32 NetID, t_i32 MapIndex, t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_LineToStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_LineToStart_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_LineToStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_LineTo) (t_i32 NetID, t_i32 MapIndex, t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_LineTo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_LineTo_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 PosList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_LineTo_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success(CECAT_API* ecmIxMot_LineTo_OverrideSpeed) (t_i32 NetID, t_i32 MapIndex, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_LineTo_OverrideSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_OverrideSpeed) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_OverrideSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_ArcAng_R_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcAng_R_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_ArcAng_R_Start_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcAng_R_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_ArcAng_R) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcAng_R(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxMot_ArcAng_R_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcAng_R_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_ArcAng_A_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcAng_A_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_ArcAng_A_Start_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcAng_A_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcAng_A) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcAng_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcAng_A_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcAng_A_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_ArcPos_R_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 XEndOffset, t_f64 YEndOffset, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcPos_R_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double yCentOffset,
            [MarshalAs(UnmanagedType.R8)] double xEndOffset,
            [MarshalAs(UnmanagedType.R8)] double yEndOffset,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_ArcPos_R_Start_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 XEndOffset, t_f64 YEndOffset, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcPos_R_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double XEndOffset,
            [MarshalAs(UnmanagedType.R8)] double YEndOffset,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcPos_R) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 XEndOffset, t_f64 YEndOffset, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcPos_R(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double XEndOffset,
            [MarshalAs(UnmanagedType.R8)] double YEndOffset,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcPos_R_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentOffset, t_f64 YCentOffset, t_f64 XEndOffset, t_f64 YEndOffset, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcPos_R_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentOffset,
            [MarshalAs(UnmanagedType.R8)] double YCentOffset,
            [MarshalAs(UnmanagedType.R8)] double XEndOffset,
            [MarshalAs(UnmanagedType.R8)] double YEndOffset,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_ArcPos_A_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 XEndPos, t_f64 YEndPos, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcPos_A_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_ArcPos_A_Start_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 XEndPos, t_f64 YEndPos, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_ArcPos_A_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcPos_A) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 XEndPos, t_f64 YEndPos, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcPos_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_ArcPos_A_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 XCentPos, t_f64 YCentPos, t_f64 XEndPos, t_f64 YEndPos, t_i32 Direction, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_ArcPos_A_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double XCentPos,
            [MarshalAs(UnmanagedType.R8)] double YCentPos,
            [MarshalAs(UnmanagedType.R8)] double XEndPos,
            [MarshalAs(UnmanagedType.R8)] double YEndPos,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_Arc3P_R_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XOffset, t_f64 P2_YOffset, t_f64 P3_XOffset, t_f64 P3_YOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Arc3P_R_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P2_YOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_YOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_Arc3P_R_Start_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XOffset, t_f64 P2_YOffset, t_f64 P3_XOffset, t_f64 P3_YOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Arc3P_R_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P2_YOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_YOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_Arc3P_R) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XOffset, t_f64 P2_YOffset, t_f64 P3_XOffset, t_f64 P3_YOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Arc3P_R(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P2_YOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_YOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_Arc3P_R_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XOffset, t_f64 P2_YOffset, t_f64 P3_XOffset, t_f64 P3_YOffset, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Arc3P_R_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P2_YOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_XOffset,
            [MarshalAs(UnmanagedType.R8)] double P3_YOffset,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_Arc3P_A_Start) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XPos, t_f64 P2_YPos, t_f64 P3_XPos, t_f64 P3_YPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Arc3P_A_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XPos,
            [MarshalAs(UnmanagedType.R8)] double P2_YPos,
            [MarshalAs(UnmanagedType.R8)] double P3_XPos,
            [MarshalAs(UnmanagedType.R8)] double P3_YPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx 	(CECAT_API *ecmIxMot_Arc3P_A_FF) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XPos, t_f64 P2_YPos, t_f64 P3_XPos, t_f64 P3_YPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Arc3P_A_Start_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XPos,
            [MarshalAs(UnmanagedType.R8)] double P2_YPos,
            [MarshalAs(UnmanagedType.R8)] double P3_XPos,
            [MarshalAs(UnmanagedType.R8)] double P3_YPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_Arc3P_A) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XPos, t_f64 P2_YPos, t_f64 P3_XPos, t_f64 P3_YPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Arc3P_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XPos,
            [MarshalAs(UnmanagedType.R8)] double P2_YPos,
            [MarshalAs(UnmanagedType.R8)] double P3_XPos,
            [MarshalAs(UnmanagedType.R8)] double P3_YPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmIxMot_Arc3P_A_NB) (t_i32 NetID, t_i32 MapIndex, t_f64 P2_XPos, t_f64 P2_YPos, t_f64 P3_XPos, t_f64 P3_YPos, t_f64 EndAngle, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxMot_Arc3P_A_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double P2_XPos,
            [MarshalAs(UnmanagedType.R8)] double P2_YPos,
            [MarshalAs(UnmanagedType.R8)] double P3_XPos,
            [MarshalAs(UnmanagedType.R8)] double P3_YPos,
            [MarshalAs(UnmanagedType.R8)] double EndAngle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_Spline_Start) (t_i32 NetID, t_i32 MapIndex, t_handle32 hSplineObj, t_bool IsReverseDir, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Spline_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hSplineObj,
            [MarshalAs(UnmanagedType.I1)] bool IsReverseDir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_MPRLin2X_Start) (t_i32 NetID, t_i32 MapIndex, t_handle32 hMPRLin2XObj, t_bool IsReverseDir, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_MPRLin2X_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            IntPtr hMPRLin2XObj,
            [MarshalAs(UnmanagedType.I1)] bool IsReverseDir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_MoveVia2X_Start) (t_i32 NetID, t_ui16 MapIndex, t_f64 P2[], t_f64 P3[], t_bool IsAbsPosMode, t_i32 RoundPosType, t_f64 NormRadius, t_f64 MinRadius, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_MoveVia2X_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] P2,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] P3,
            [MarshalAs(UnmanagedType.I1)] bool IsAbsPosMode,
            [MarshalAs(UnmanagedType.I4)] int RoundPosType,
            [MarshalAs(UnmanagedType.R8)] double NormRadius,
            [MarshalAs(UnmanagedType.R8)] double MinRadius,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_MoveVia2X_OverrideTP) (t_i32 NetID, t_ui16 MapIndex, t_f64 TP_new[], t_f64 RoundRadius, _out t_bool *IsIgnored, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_MoveVia2X_OverrideTP(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort MapIndex,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] TP_new,
            [MarshalAs(UnmanagedType.R8)] double RoundRadius,
            [MarshalAs(UnmanagedType.I1)] ref bool IsIgnored,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_Stop) (t_i32 NetID, t_i32 MapIndex, t_i32 IsDecStop, t_i32 IsWaitCompt, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Stop(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] int IsWaitCompt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmIxMot_Stop_FF) (t_i32 NetID, t_i32 MapIndex, t_i32 IsDecStop, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmIxMot_Stop_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int IsDecStop,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmIxSt_IsBusy) (t_i32 NetID, t_i32 MapIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSt_IsBusy(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxSt_WaitCompt) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSt_WaitCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmIxSt_WaitCompt_NB) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSt_WaitCompt_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== MOTION - Slow Down(SD) for IX(Interpolation) FUNCTIONS =================================================//
        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_SetInputEnv) (t_i32 NetID, t_i32 MapIndex, t_dword LogBitAddr, t_bool IsInvertLogic, t_i32 FilterCount, t_i32 *ErrCode); 
        // SD 입력 신호에 대한 환경을 설정한다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_SetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.U4)] uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_GetInputEnv) (t_i32 NetID, t_i32 MapIndex, t_dword *LogBitAddr, t_bool *IsInvertLogic, t_i32 *FilterCount, t_i32 *ErrCode); 
        // SD 입력 신호에 대한 환경 설정값을 읽는다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetInputEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.U4)] ref uint LogBitAddr,
            [MarshalAs(UnmanagedType.I1)] ref bool IsInvertLogic,
            [MarshalAs(UnmanagedType.I4)] ref int FilterCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_SetSpeed) (t_i32 NetID, t_i32 MapIndex, t_f64 SdSpeed, t_i32 *ErrCode); 
        // SD 속도를 설정한다 (이때 속도의 단위는 ecmIxCfg_SetSpeedPatt() 함수의 IxSpdPattType 인자에 따른다)
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_SetSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.R8)] double SdSpeed,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_f64		(CECAT_API *ecmIxSD_GetSpeed) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); // SD 속도 설정값을 반환한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.R8)]
        public static extern unsafe double ecmIxSD_GetSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_SetOffset) (t_i32 NetID, t_i32 MapIndex, t_i32 OffsetMode, t_f64 Offset, t_i32 OfsAxis, t_i32 *ErrCode); 
        // SD OFFSET을 설정한다. OffsetMode에 설정가능한 값은 EEcmSdOfsMode 형선언 참조. OfsAixs는 OffsetMode가 ecmSD_OFS_CMDPOS나 ecmSD_OFS_FEEDPOS인 경우에 거리의 주체가 되는 축을 지정한다. 이 값을 -1로 지정하면 보간 거리를 적용한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_SetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] int OffsetMode,
            [MarshalAs(UnmanagedType.R8)] double Offset,
            [MarshalAs(UnmanagedType.I4)] int OfsAxis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_GetOffset) (t_i32 NetID, t_i32 MapIndex, t_i32 *OffsetMode, t_f64 *Offset, t_i32 *OfsAxis, t_i32 *ErrCode); 
        // SD OFFSET 설정을 읽는다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int OffsetMode,
            [MarshalAs(UnmanagedType.R8)] ref double Offset,
            [MarshalAs(UnmanagedType.I4)] ref int OfsAxis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_SetLatchMode) (t_i32 NetID, t_i32 MapIndex, t_bool IsLatchMode, t_i32 *ErrCode); 
        // SD 신호의 Latch 모드를 설정한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_SetLatchMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I1)] bool IsLatchMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmIxSD_GetLatchMode) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); 
        // SD 신호의 Latch 모드를 설정값을 반환한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetLatchMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_SetEnable) (t_i32 NetID, t_i32 MapIndex, t_bool IsEnable, t_i32 *ErrCode); 
        // SD 기능을 활성화 또는 비활성화한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_SetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmIxSD_GetEnable) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); 
        // SD 기능의 활성화 여부를 반환한다. 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetEnable(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmIxSD_RestoreSpeed) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); 
        // Slow Down 속도로 진행 중인 경우에 원래의 속도로 복귀하도록 한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_RestoreSpeed(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmIxSD_GetInputStatus) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); 
        // SD 신호의 상태를 반환다
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetInputStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_bool	(CECAT_API *ecmIxSD_GetActStatus) (t_i32 NetID, t_i32 MapIndex, t_i32 *ErrCode); 
        // SD 속도로 진행중인지에 대한 상태를 반환한다.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmIxSD_GetActStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int MapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);




        ////====================== MOTION - HOMING OPERATION FUNCTIONS ============================================//

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeCfg_SetMode) (t_i32 NetID, t_i32 Axis, t_i32 HomeOpMode, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int HomeOpMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmHomeCfg_GetMode) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_GetMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeCfg_SetOffset) (t_i32 NetID, t_i32 Axis, t_f64 Offset, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Offset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_f64		(CECAT_API *ecmHomeCfg_GetOffset) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe double ecmHomeCfg_GetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetOffsetEx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double Offset,
            [MarshalAs(UnmanagedType.I1)] bool IsMoveToZero,
            [MarshalAs(UnmanagedType.I4)] int OffsetDir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_GetOffsetEx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] ref double Offset,
            [MarshalAs(UnmanagedType.I1)] ref bool IsMoveToZero,
            [MarshalAs(UnmanagedType.I4)] ref int OffsetDir,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeCfg_SetSpeedPatt) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 Vel, t_f64 Acc, t_f64 Dec, t_f64 HomeSpecVel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double Vel,
            [MarshalAs(UnmanagedType.R8)] double Acc,
            [MarshalAs(UnmanagedType.R8)] double Dec,
            [MarshalAs(UnmanagedType.R8)] double HomeSpecVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHomeCfg_GetSpeedPatt) (t_i32 NetID, t_i32 Axis, t_i32* SpeedMode, t_f64* Vel, t_f64* Acc, t_f64* Dec, t_f64* HomeSpecVel, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeCfg_GetSpeedPatt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double Vel,
            [MarshalAs(UnmanagedType.R8)] ref double Acc,
            [MarshalAs(UnmanagedType.R8)] ref double Dec,
            [MarshalAs(UnmanagedType.R8)] ref double HomeSpecVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeCfg_SetSpeedPatt_Time) (t_i32 NetID, t_i32 Axis, t_i32 SpeedMode, t_f64 Vel, t_f64 AccTime, t_f64 DecTime, t_f64 HomeSpecVel, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] double Vel,
            [MarshalAs(UnmanagedType.R8)] double AccTime,
            [MarshalAs(UnmanagedType.R8)] double DecTime,
            [MarshalAs(UnmanagedType.R8)] double HomeSpecVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHomeCfg_GetSpeedPatt_Time) (t_i32 NetID, t_i32 Axis, t_i32* SpeedMode, t_f64* Vel, t_f64* AccTime, t_f64* DecTime, t_f64* HomeSpecVel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeCfg_GetSpeedPatt_Time(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int SpeedMode,
            [MarshalAs(UnmanagedType.R8)] ref double Vel,
            [MarshalAs(UnmanagedType.R8)] ref double AccTime,
            [MarshalAs(UnmanagedType.R8)] ref double DecTime,
            [MarshalAs(UnmanagedType.R8)] ref double HomeSpecVel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeCfg_SetOption) (t_i32 NetID, t_i32 Axis, EEcmHomeOptID OptionID, t_i32 OptionVal, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_SetOption(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            EEcmHomeOptID OptionID,
            [MarshalAs(UnmanagedType.I4)] int OptionVal,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmHomeCfg_GetOption) (t_i32 NetID, t_i32 Axis, EEcmHomeOptID OptionID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeCfg_GetOption(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            EEcmHomeOptID OptionID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeMot_MoveStart) (t_i32 NetID, t_i32 Axis, t_i32 Direction, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeMot_MoveStart(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeMot_MoveStart_FF) (t_i32 NetID, t_i32 Axis, t_i32 Direction, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeMot_MoveStart_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHomeMot_Move) (t_i32 NetID, t_i32 Axis, t_i32 Direction, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeMot_Move(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int Direction,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeMot_MoveStartAll)(t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 DirList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeMot_MoveStartAll(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_cmdidx	(CECAT_API *ecmHomeMot_MoveStartAll_FF)(t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 DirList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmHomeMot_MoveStartAll_FF(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHomeMot_MoveAll)(t_i32 NetID, t_i32 NumAxes, t_i32 AxisList[], t_i32 DirList[], _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeMot_MoveAll(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int NumAxes,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] AxisList,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] int[] DirList,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmHomeSt_IsBusy) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeSt_IsBusy(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmHomeSt_WaitCompt) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmHomeSt_WaitCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_word	(CECAT_API *ecmHomeSt_GetFlags) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecmHomeSt_GetFlags(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== MOTION - Master/Slave FUNCTIONS ============================================//

        //EC_EXTERN t_success (CECAT_API *ecmMsCfg_SetSlvEnv) (t_i32 NetID, t_i32 Axis, t_i32 MasterAxis, t_f64 PosRatio, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCfg_SetSlvEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] int MasterAxis,
            [MarshalAs(UnmanagedType.R8)] double PosRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCfg_GetSlvEnv) (t_i32 NetID, t_i32 Axis, t_i32* MasterAxis, t_f64* PosRatio, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCfg_GetSlvEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int MasterAxis,
            [MarshalAs(UnmanagedType.R8)] ref double PosRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCfg_SetTorqFollowMode) (t_i32 NetID, t_i32 SalveAxis, t_bool IsEnable, t_ui32 FilterDepth, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCfg_SetTorqFollowMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SalveAxis,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] uint FilterDepth,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCfg_GetTorqFollowMode) (t_i32 NetID, t_i32 SalveAxis, t_bool* IsEnable, t_ui32* FilterDepth, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCfg_GetTorqFollowMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SalveAxis,
            [MarshalAs(UnmanagedType.I1)] ref bool IsEnable,
            [MarshalAs(UnmanagedType.U4)] ref uint FilterDepth,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCtl_StartSlv) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCtl_StartSlv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCtl_StopSlv) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCtl_StopSlv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmMsCtl_SynchSlv) (t_i32 NetID, t_i32 Axis, t_f64 MastPosOfs, t_bool IsWaitMoveCompt, _out t_i32 *ErrCode); 
        //< 지정된 슬레이브 축의 위치가 마스터축의 위치와 일치하는 위치가 되도록 슬레이브축을 이송.
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsCtl_SynchSlv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.R8)] double MastPosOfs,
            [MarshalAs(UnmanagedType.I1)] bool IsWaitMoveCompt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecmMsSt_IsSlvStarted) (t_i32 NetID, t_i32 Axis, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmMsSt_IsSlvStarted(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int Axis,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //====================== MOTION - ListMotion FUNCTIONS ============================================//

        //EC_EXTERN t_success	(CECAT_API *ecmLmCtl_Begin) (t_i32 NetID, t_i32 LmMapIndex, t_ui32 AxisMask1, t_ui32 AxisMask2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_Begin(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.U4)] uint AxisMask1,
            [MarshalAs(UnmanagedType.U4)] uint AxisMask2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCtl_End) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_End(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCtl_Run) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_Run(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCtl_Stop) (t_i32 NetID, t_i32 LmMapIndex, t_bool IsComptCurStep, t_i32 DecelTime_ms, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_Stop(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I1)] bool IsComptCurStep,
            [MarshalAs(UnmanagedType.I4)] int DecelTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCtl_ClearQue) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_ClearQue(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCfg_SetQueFullMode) (t_i32 NetID, t_i32 LmMapIndex, t_i32 QueFullMode, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCfg_SetQueFullMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int QueFullMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmLmCfg_GetQueFullMode) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmLmCfg_GetQueFullMode(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int QueFullMode,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCfg_SetQueDepth) (t_i32 NetID, t_i32 LmMapIndex, t_i32 QueDepth, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCfg_SetQueDepth(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int QueDepth,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmLmCfg_GetQueDepth) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmLmCfg_GetQueDepth(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCfg_SetStepId) (t_i32 NetID, t_i32 LmMapIndex, t_i32 StepId, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCfg_SetStepId(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int StepId,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmLmCfg_GetStepId) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmLmCfg_GetStepId(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCfg_SetStepParam) (t_i32 NetID, t_i32 LmMapIndex, t_i32 StepParam1, t_i32 StepParam2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCfg_SetStepParam(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int StepParam1,
            [MarshalAs(UnmanagedType.I4)] int StepParam2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCfg_GetStepParam) (t_i32 NetID, t_i32 LmMapIndex, t_dword *StepParam1, t_dword *StepParam2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCfg_GetStepParam(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int StepParam1,
            [MarshalAs(UnmanagedType.I4)] ref int StepParam2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmLmSt_GetRunSts) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmLmSt_GetRunSts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecmLmSt_GetRemStepCount) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecmLmSt_GetRemStepCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmSt_GetRunStepInfo) (t_i32 NetID, t_i32 LmMapIndex, t_i32 *RunStepCount, t_i32 *RunStepId, t_i32 *RunStepSts, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmSt_GetRunStepInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int RunStepCount,
            [MarshalAs(UnmanagedType.I4)] ref int RunStepId,
            [MarshalAs(UnmanagedType.I4)] ref int RunStepSts,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmSt_GetRunStepParam) (t_i32 NetID, t_i32 LmMapIndex, t_dword *StepParam1, t_dword *StepParam2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmSt_GetRunStepParam(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.U4)] ref uint StepParam1,
            [MarshalAs(UnmanagedType.U4)] ref uint StepParam2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmCmd_Delay) (t_i32 NetID, t_i32 LmMapIndex, t_i32 DelayTime_ms, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCmd_Delay(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.I4)] int DelayTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecmLmCtl_SetSpeedRatio) (t_i32 NetID, t_i32 LmMapIndex, t_f32 LmSpeedRatio, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmLmCtl_SetSpeedRatio(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.R4)] float LmSpeedRatio,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecdoLmPutOne) (t_i32 NetID, t_i32 LmMapIndex, t_ui32 DoChannel, t_bool OutState, t_ui32 DurTime_ms, t_i32* ErrCode);							// 리스트모션용 전역채널 DO 출력
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoLmPutOne(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.U4)] uint DoChannel,
            [MarshalAs(UnmanagedType.U1)] bool OutState,
            [MarshalAs(UnmanagedType.U4)] uint DurTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecdoLmPutOne_L) (t_i32 NetID, t_i32 LmMapIndex, t_ui16 SlvPhysAddr, t_i32 LocalChannel, t_bool OutState, t_ui32 DurTime_ms, t_i32 *ErrCode);	// 리스트모션용 로컬채널 DO 출력
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecdoLmPutOne_L(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int LmMapIndex,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.I4)] int LocalChannel,
            [MarshalAs(UnmanagedType.I4)] bool OutState,
            [MarshalAs(UnmanagedType.U4)] uint DurTime_ms,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmLmResetAll);
        [DllImport(dll)]
        public static extern unsafe void ecmLmResetAll(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== MOTION - PT-Motion FUNCTIONS ============================================//

        //EC_EXTERN t_success	(CECAT_API *ecmPtmResetAll) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmResetAll(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_success	(CECAT_API *ecmPtmCtl_Begin) (t_i32 NetID, t_i32 PtmMapIndex, t_ui32 AxisMask1, t_ui32 AxisMask2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmCtl_Begin(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.U4)] uint AxisMask1,
            [MarshalAs(UnmanagedType.U4)] uint AxisMask2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmPtmCtl_End) (t_i32 NetID, t_i32 PtmMapIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmCtl_End(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmPtmCtl_SetHold) (t_i32 NetID, t_i32 PtmMapIndex, t_bool IsHold, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmCtl_SetHold(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.I1)] bool IsHold,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecmPtmCmd_AddItem_PT) (t_i32 NetID, t_i32 PtmMapIndex, t_f64 DurTime, t_f64 PosDataList[], t_i32 NumPosData, t_bool IsAbsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmCmd_AddItem_PT(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.R8)] double DurTime,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R8)] double[] PosDataList,
            [MarshalAs(UnmanagedType.I4)] int NumPosData,
            [MarshalAs(UnmanagedType.I1)] bool IsAbsPos,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN EEcmPtmSts (CECAT_API *ecmPtmSt_GetRunSts) (t_i32 NetID, t_i32 PtmMapIndex, t_i32 *RunStepCount, t_i32 *RemStepCount, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe EEcmPtmSts ecmPtmSt_GetRunSts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int runStepCount,
            [MarshalAs(UnmanagedType.I4)] ref int RemStepCount,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecmPtmSt_GetRunStepInfo) (t_i32 NetID, t_i32 PtmMapIndex, t_i32 *RunStepCount, t_i32 *RunStepSts, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecmPtmSt_GetRunStepInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PtmMapIndex,
            [MarshalAs(UnmanagedType.I4)] ref int RunStepCount,
            [MarshalAs(UnmanagedType.I4)] ref int RunStepSts,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

    }
}
