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
        //const string dll = "ComiEcatSdk.dll";
        //====================== DLL LOAD/UNLOAD FUNCTIONS ======================================================//
        //EC_EXTERN t_success ecDll_Load (void);        
        //EC_EXTERN t_success ecDll_Unload (void);      
        //[DllImport("ComiEcatSdk.DLL", EntryPoint = "ecDll_Unload")]
        //EC_EXTERN BOOL		ecDll_IsLoaded(void);

        ////====================== GENERAL FUNCTIONS ==============================================================//

        //EC_EXTERN t_success (CECAT_API *ecGn_SetBootWaitMode) (t_bool IsWaitBootCompt, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_SetBootWaitMode(
            [MarshalAs(UnmanagedType.I1)] bool IsWaitBootCompt, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecGn_GetBootWaitMode) (t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_GetBootWaitMode(
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== NET INTERFACE FUNCTIONS =========================================================//


        //EC_EXTERN void*		(CECAT_API *ecNet_InPDO_GetBufPtr) (t_i32 NetID, t_i32 OfsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr ecNet_InPDO_GetBufPtr(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_InPDO_GetData) (t_i32 NetID, t_i32 OfsPos, t_ui16 Size, void *pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InPDO_GetData(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.U2)] ushort Size, 
            ref IntPtr pBuf, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_byte	(CECAT_API *ecNet_InPDO_GetData_B) (t_i32 NetID, t_i32 OfsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecNet_InPDO_GetData_B(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_word	(CECAT_API *ecNet_InPDO_GetData_W) (t_i32 NetID, t_i32 OfsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecNet_InPDO_GetData_W(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword	(CECAT_API *ecNet_InPDO_GetData_D) (t_i32 NetID, t_i32 OfsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_InPDO_GetData_D(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN void*		(CECAT_API *ecNet_OutPDO_GetBufPtr) (t_i32 NetID, t_i32 OfsPos, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr ecNet_OutPDO_GetBufPtr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int OfsPos, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_OutPDO_SetData) (t_i32 NetID, t_i32 OfsPos, t_ui16 Size, void *pBuf, t_i32 *ErrCode);
        //[DllImport(dll, EntryPoint = "ecNet_OutPDO_SetData")]
        //public static extern unsafe int ecNet_OutPDO_SetData(
        //    [MarshalAs(UnmanagedType.I4)] int NetID, 
        //    [MarshalAs(UnmanagedType.I4)] int OfsPos, 
        //    [MarshalAs(UnmanagedType.U2)] ushort Size, 
        //    ref IntPtr pBuf, 
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        //DLL에 없음

        //EC_EXTERN t_success	(CECAT_API *ecNet_OutPDO_SetData_B) (t_i32 NetID, t_i32 OfsPos, t_byte Data, t_i32 *ErrCode);
        //[DllImport(dll, EntryPoint = "ecNet_OutPDO_SetData_B")]
        //[return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecNet_OutPDO_SetData_B(
        //    [MarshalAs(UnmanagedType.I4)] int NetID, 
        //    [MarshalAs(UnmanagedType.I4)] int OfsPos,
        //    [MarshalAs(UnmanagedType.U1)] byte Data, 
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        //DLL에 없음

        //EC_EXTERN t_success	(CECAT_API *ecNet_OutPDO_SetData_W) (t_i32 NetID, t_i32 OfsPos, t_word Data, t_i32 *ErrCode);
        //[DllImport(dll)]
        //[return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecNet_OutPDO_SetData_W(
        //    [MarshalAs(UnmanagedType.I4)] int NetID, 
        //    [MarshalAs(UnmanagedType.I4)] int OfsPos,
        //    [MarshalAs(UnmanagedType.U2)] ushort Data, 
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        //DLL에 없음

        //EC_EXTERN t_success	(CECAT_API *ecNet_OutPDO_SetData_D) (t_i32 NetID, t_i32 OfsPos, t_dword Data, t_i32 *ErrCode);
        //[DllImport(dll)]
        //[return: MarshalAs(UnmanagedType.I1)]
        //public static extern unsafe bool ecNet_OutPDO_SetData_D(
        //    [MarshalAs(UnmanagedType.I4)] int NetID,
        //    [MarshalAs(UnmanagedType.I4)] int OfsPos, 
        //    [MarshalAs(UnmanagedType.U4)] uint Data, 
        //    [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        //DLL에 없음



        //====================== SLAVE I/F FUNCTIONS ============================================================//

        //EC_EXTERN void*		(CECAT_API *ecSlv_InPDO_GetBufPtr) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 PdoIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr  ecSlv_InPDO_GetBufPtr(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr, 
            [MarshalAs(UnmanagedType.U1)] byte PdoIndex,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
        //EC_EXTERN t_i32			(CECAT_API *ecSlv_InPDO_GetOffset) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 InPdoUnitIdx, int *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_InPDO_GetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte InPdoUnitIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecSlv_InPDO_GetBufLen) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 InPdoUnitIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_InPDO_GetBufLen(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr, 
            [MarshalAs(UnmanagedType.U1)] byte PdoIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN void*		(CECAT_API *ecSlv_OutPDO_GetBufPtr) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 PdoIndex, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe IntPtr ecSlv_OutPDO_GetBufPtr(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte PdoIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
        //EC_EXTERN t_i32			(CECAT_API *ecSlv_OutPDO_GetOffset) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, int *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_OutPDO_GetOffset(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte OutPdoUnitIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecSlv_OutPDO_GetBufLen) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_OutPDO_GetBufLen(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte PdoIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_OutPDO_SetInitValMode) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, t_i32 OutPDOInitMode, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_OutPDO_SetInitValMode(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte OutPdoUnitIdx,
            [MarshalAs(UnmanagedType.I4)] int OutPDOInitMode, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32			(CECAT_API *ecSlv_OutPDO_GetInitValMode) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_OutPDO_GetInitValMode(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte OutPdoUnitIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success		(CECAT_API *ecSlv_OutPDO_SetInitVal) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, t_i32 OfsPos, t_i32 DataSize, void *pData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_OutPDO_SetInitVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte OutPdoUnitIdx,
            [MarshalAs(UnmanagedType.I4)] int OfsPos,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        ////EC_EXTERN t_i32			(CECAT_API *ecSlv_OutPDO_GetInitVal) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui8 OutPdoUnitIdx, t_i32 OfsPos, t_i32 BufSize, void *pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecSlv_OutPDO_GetInitVal(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U1)] byte OutPdoUnitIdx,
            [MarshalAs(UnmanagedType.I4)] int OfsPos,
            [MarshalAs(UnmanagedType.I4)] int DataSize,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
        
        //*****************************************************************************************************************************
        //*
        //*							Functions for Advanced Users
        //*
        //*****************************************************************************************************************************

        //====================== NET INTERFACE FUNCTIONS =========================================================//

        //EC_EXTERN t_success (CECAT_API *ecNet_SetFastFuncType) (t_i32 NetID, t_ui8 FastCmdType, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SetFastFuncType(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int FastCmdType, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui8		(CECAT_API *ecNet_GetFastFuncType) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_GetFastFuncType(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_InPDO_GetSectPos) (t_i32 NetID, t_ui16 SectionID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InPDO_GetSectPos(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SectionID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_OutPDO_GetSectPos) (t_i32 NetID, t_ui16 SectionID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_OutPDO_GetSectPos(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SectionID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_InQue_ClearChanList) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_ClearChanList(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN HANDLE 	(CECAT_API *ecNet_InQue_AddChannel) (t_i32 NetID, TEcInQueDataDesc *pDataDescList, int NumDataDescLisItems, t_i32 QueBufDepth, t_bool IsCircular, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_AddChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            ref TEcInQueDataDesc pDataDescList, 
            [MarshalAs(UnmanagedType.I4)] int NumDataDescLisItems, 
            [MarshalAs(UnmanagedType.I4)] int QueBufDepth, 
            [MarshalAs(UnmanagedType.I1)] bool IsCircular,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_DelChannel) (t_i32 NetID, HANDLE QueChanHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_DelChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecNet_InQue_GetNumDataDescItems) (t_i32 NetID, HANDLE QueChanHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_GetNumDataDescItems(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_InQue_GetDataDesc) (t_i32 NetID, HANDLE QueChanHandle, t_i32 ItemIndex, TEcInQueDataDesc *pDataDesc, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_GetDataDesc(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            [MarshalAs(UnmanagedType.I4)] int ItemIndex, 
            ref TEcInQueDataDesc pDataDesc,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_InQue_GetQueSizeInfo)	(t_i32 NetID, HANDLE QueChanHandle, t_i32 *QueItemSize, t_i32 *QueBufDepth, t_bool *IsCircular, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_GetQueSizeInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle,
            [MarshalAs(UnmanagedType.I4)] ref int QueItemSize, 
            [MarshalAs(UnmanagedType.I4)] ref int QueBufDepth,
            [MarshalAs(UnmanagedType.I1)] ref bool IsCircular,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_SetStaTrgEnv) (t_i32 NetID, HANDLE QueChanHandle, TEcInQueTrgCfg *pTrgCfgData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_SetStaTrgEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle,
            ref TEcInQueTrgCfg pTrgCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_GetStaTrgEnv) (t_i32 NetID, HANDLE QueChanHandle, TEcInQueTrgCfg *pTrgCfgBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_GetStaTrgEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            ref TEcInQueTrgCfg pTrgCfgBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_SetStopTrgEnv) (t_i32 NetID, HANDLE QueChanHandle, TEcInQueTrgCfg *pTrgCfgData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_SetStopTrgEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            ref TEcInQueTrgCfg pTrgCfgData, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_GetStopTrgEnv) (t_i32 NetID, HANDLE QueChanHandle, TEcInQueTrgCfg *pTrgCfgBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_GetStopTrgEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            ref TEcInQueTrgCfg pTrgCfgBuf, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_StartChannel) (t_i32 NetID, HANDLE QueChanHandle, t_i32 SamplingInterval, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_GetStopTrgEnv(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle,
            [MarshalAs(UnmanagedType.I4)] int SamplingInterval, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_StopChannel) (t_i32 NetID, HANDLE QueChanHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_StopChannel(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32 	(CECAT_API *ecNet_InQue_GetQueuedCount) (t_i32 NetID, HANDLE QueChanHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_GetQueuedCount(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32 	(CECAT_API *ecNet_InQue_Deque) (t_i32 NetID, HANDLE QueChanHandle, void *pBuf, int nBufSize, t_i32 ReqDequeCnt, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_Deque(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            ref IntPtr pBuf, 
            [MarshalAs(UnmanagedType.I4)] int nBufSize,
            [MarshalAs(UnmanagedType.I4)] int ReqDequeCnt,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32 	(CECAT_API *ecNet_InQue_PeekData) (t_i32 NetID, HANDLE QueChanHandle, void *pBuf, int nBufSize, int nStartIndex, t_i32 ReqPeekCnt, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_PeekData(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int QueChanHandle, 
            ref IntPtr pBuf, 
            [MarshalAs(UnmanagedType.I4)] int nBufSize, 
            [MarshalAs(UnmanagedType.I4)] int nStartIndex,
            [MarshalAs(UnmanagedType.I4)] int ReqPeekCnt, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN HANDLE	(CECAT_API *ecNet_InQue_AddCommonTrg) (t_i32 NetID, TEcInQueCommonTrg *pCommonTrgObj, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecNet_InQue_AddCommonTrg(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            ref TEcInQueCommonTrg pCommonTrgObj, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_DelCommonTrg) (t_i32 NetID, HANDLE CommTrgHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_DelCommonTrg(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CommTrgHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_EnableCommonTrg) (t_i32 NetID, HANDLE CommTrgHandle, t_bool IsEanble, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_EnableCommonTrg(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int CommTrgHandle, 
            [MarshalAs(UnmanagedType.I1)] bool IsEanble,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_InQue_SetCommonTrgState) (t_i32 NetID, HANDLE CommTrgHandle, t_bool State, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_InQue_SetCommonTrgState(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int CommTrgHandle, 
            [MarshalAs(UnmanagedType.I1)] bool State, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_pdoidx	(CECAT_API *ecNet_GetLastPDOIdx) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_GetLastPDOIdx(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecNet_IsPDOIdxCompt) (t_i32 NetID, t_pdoidx PDOIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_IsPDOIdxCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int PDOIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_WaitPDOIdxCompt) (t_i32 NetID, t_pdoidx PDOIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_WaitPDOIdxCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int PDOIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_WaitPDOIdxCompt_NB) (t_i32 NetID, t_pdoidx PDOIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_WaitPDOIdxCompt_NB
            ([MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int PDOIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecNet_IsCmdIdxCompt) (t_i32 NetID, t_cmdidx CmdIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_IsCmdIdxCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CmdIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_WaitCmdIdxCompt) (t_i32 NetID, t_cmdidx CmdIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_WaitCmdIdxCompt(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int CmdIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecNet_WaitCmdIdxCompt_NB) (t_i32 NetID, t_cmdidx CmdIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_WaitCmdIdxCompt_NB(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int CmdIdx,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_ui32	(CECAT_API *ecNet_GetCycleProcessTime) (t_i32 NetID, t_ui8 ProcTimeType, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_GetCycleProcessTime(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] byte ProcTimeType, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        //====================== ECAT CONFIG FUNCTIONS ==========================================================//

        //EC_EXTERN t_success (CECAT_API *ecCfgFile_Download) (t_i32 DevIdx, char *szFilePath, EEcLocNetSel LocNetSel, t_i32 *ErrCode);
        [DllImport(dll, ThrowOnUnmappableChar = true, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfgFile_Download(
            //[MarshalAs(UnmanagedType.I1)] ref char[] szFilePath,
            [MarshalAs(UnmanagedType.LPStr)] string szFilePath,
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int LocNetSel,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfgFile_Verify) (t_i32 DevIdx, char *szFilePath, EEcLocNetSel LocNetSel, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfgFile_Verify(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szFilePath,
            EEcLocNetSel LocNetSel, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);





        //*****************************************************************************************************************************
        //*
        //*							Functions for public System Users
        //*
        //*****************************************************************************************************************************


        //====================== GENERAL FUNCTIONS ==============================================================//


        //EC_EXTERN ULONG		(CECAT_API *ecGn_GetDebugData) (t_i32 DevIdx, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecGn_GetDebugData(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpB) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_byte Data, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpB(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.U1)] byte Data, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpsB) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_byte* pData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpsB(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS,
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U1)] ref byte pData, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpW) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_word Data, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpW(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset,
            [MarshalAs(UnmanagedType.U2)] ushort Data, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpsW) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_word* pData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpsW(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U2)] ref ushort pData, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpD) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_dword Data, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpD(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset,
            [MarshalAs(UnmanagedType.U4)] uint Data, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_OutpsD) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_dword* pData, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_OutpsD(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS,
            [MarshalAs(UnmanagedType.I4)] int intOffset,
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U4)] ref uint pData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_byte	(CECAT_API *ecGn_InpB) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe byte ecGn_InpB(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_InpsB) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_byte* pBuffer, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_InpsB(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U1)] ref byte pBuffer, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_word	(CECAT_API *ecGn_InpW) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe ushort ecGn_InpW(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_InpsW) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_word* pBuffer, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_InpsW(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS,
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U2)] ref ushort pBuffer, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_dword	(CECAT_API *ecGn_InpD) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecGn_InpD(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int CS, 
            [MarshalAs(UnmanagedType.I4)] int intOffset,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_InpsD) (t_i32 DevIdx, t_i32 CS, t_i32 intOffset, t_i32 Count, t_dword* pBuffer, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_InpsD(
            [MarshalAs(UnmanagedType.I4)] int DevIdx, 
            [MarshalAs(UnmanagedType.I4)] int CS,
            [MarshalAs(UnmanagedType.I4)] int intOffset, 
            [MarshalAs(UnmanagedType.I4)] int Count, 
            [MarshalAs(UnmanagedType.U4)] ref uint pBuffer, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_DbgTempCmd) (t_i32 DevIdx, t_i32 Data1, t_i32 Data2, t_i32 Data3, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_DbgTempCmd(
            [MarshalAs(UnmanagedType.I4)] int DevIdx,
            [MarshalAs(UnmanagedType.I4)] int Data1,
            [MarshalAs(UnmanagedType.I4)] int Data2,
            [MarshalAs(UnmanagedType.I4)] int Data3,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecGn_ReadInPDO) (t_i32 NetID, t_ui32 StartAddr, t_ui32 Size, void* pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecGn_ReadInPDO(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U4)] uint StartAddr,
            [MarshalAs(UnmanagedType.U4)] uint Size, 
            ref IntPtr pBuf, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_ReadDspDbgData_I) (t_i32 NetID, t_ui8 DspCoreIdx, t_ui16 IniArrayIdx, t_ui16 NumData, t_i32* pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern unsafe int ecGn_ReadDspDbgData_I(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] byte DspCoreIdx,
            [MarshalAs(UnmanagedType.U2)] ushort IniArrayIdx,
            [MarshalAs(UnmanagedType.U2)] ushort NumData,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_WriteDspDbgData_I) (t_i32 NetID, t_ui8 DspCoreIdx, t_ui16 IniArrayIdx, t_ui16 NumData, t_i32* pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern unsafe int ecGn_WriteDspDbgData_I(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] byte DspCoreIdx,
            [MarshalAs(UnmanagedType.U2)] ushort IniArrayIdx,
            [MarshalAs(UnmanagedType.U2)] ushort NumData,
            [MarshalAs(UnmanagedType.I4)] ref int pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_ReadDspDbgData_F) (t_i32 NetID, t_ui8 DspCoreIdx, t_ui16 IniArrayIdx, t_ui16 NumData, t_f64* pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern unsafe int ecGn_ReadDspDbgData_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] byte DspCoreIdx,
            [MarshalAs(UnmanagedType.U2)] ushort IniArrayIdx,
            [MarshalAs(UnmanagedType.U2)] ushort NumData,
            [MarshalAs(UnmanagedType.R8)] ref double pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecGn_WriteDspDbgData_F) (t_i32 NetID, t_ui8 DspCoreIdx, t_ui16 IniArrayIdx, t_ui16 NumData, t_f64* pBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern unsafe int ecGn_WriteDspDbgData_F(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] byte DspCoreIdx,
            [MarshalAs(UnmanagedType.U2)] ushort IniArrayIdx,
            [MarshalAs(UnmanagedType.U2)] ushort NumData,
            [MarshalAs(UnmanagedType.R8)] ref double pBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //public static bool ecGn_InitFromFile(int netID, int axisCount, string filePath, ref int errorCode)
        //{
        //    try
        //    {
        //        XmlDocument xdoc = new XmlDocument();
        //        xdoc.Load(filePath);

        //        byte[] axisList = new byte[axisCount];
        //        ecmGn_GetAxisList(netID, axisList, (byte)axisCount, ref errorCode);

        //        int tempInt = 0;
        //        int tempInt2 = 0;
        //        double tempDouble = 0;
        //        double tempDouble2 = 0;
        //        bool tempBool = false;
        //        //bool tempBool2 = false;

        //        int speedMode = 0;
        //        double workSpeed = 0;
        //        double accel = 0;
        //        double decel = 0;
        //        double init = 0;
        //        double end = 0;
        //        double reverseSpeed = 0;
        //        double jerk = 0;

        //        XmlNode deviceNode = xdoc.SelectSingleNode(string.Format("descendant::Devices/Device[@Type='{0}' and @ID='{1}']", "EtherCAT", netID));
        //        if (deviceNode == null)
        //        {
        //            errorCode = -30000;
        //            return false;
        //        }

        //        XmlNode devNode = deviceNode.ChildNodes[0].SelectSingleNode(string.Format("descendant::Motion[@ID='{0}']", 0));

        //        for (int axis = 0; axis < axisCount; axis++)
        //        {
        //            XmlNode axisNode = devNode.ChildNodes[1].SelectSingleNode(string.Format("descendant::Axis[@ID='{0}']", axisList[axis]));
        //            if (axisNode == null)
        //                continue;

        //            if (axisNode.SelectSingleNode("UnitDistance") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("UnitDistance").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31001;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetUnitDist(netID, axisList[axis], tempDouble, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = - 31101;

        //            if (axisNode.SelectSingleNode("UnitSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("UnitSpeed").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31002;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetUnitSpeed(netID, axisList[axis], tempDouble, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31102;

        //            if (axisNode.SelectSingleNode("SpeedMode") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("SpeedMode").InnerText, out speedMode))
        //                {
        //                    errorCode = -31000;
        //                    return false;
        //                }
        //            }

        //            if (axisNode.SelectSingleNode("WorkSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("WorkSpeed").InnerText, out workSpeed))
        //                {
        //                    errorCode = -31003;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31103;

        //            if (axisNode.SelectSingleNode("Accel") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("Accel").InnerText, out accel))
        //                {
        //                    errorCode = -31004;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31104;

        //            if (axisNode.SelectSingleNode("Decel") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("Decel").InnerText, out decel))
        //                {
        //                    errorCode = -31005;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31105;

        //            if (axisNode.SelectSingleNode("Init") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("Init").InnerText, out init))
        //                {
        //                    errorCode = -31006;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31106;

        //            if (axisNode.SelectSingleNode("EndSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("EndSpeed").InnerText, out end))
        //                {
        //                    errorCode = -31007;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31107;
                                        
        //            ecmSxCfg_SetSpeedPatt(netID, axisList[axis], speedMode, init, end, workSpeed, accel, decel, ref errorCode);
        //            if(errorCode != 0) 
        //                return false;

        //            if (axisNode.SelectSingleNode("Jerk") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("Jerk").InnerText, out jerk))
        //                {
        //                    errorCode = -31008;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetJerkRatio(netID, axisList[axis], jerk, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31108;

        //            if (axisNode.SelectSingleNode("HomeMode") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("HomeMode").InnerText, out tempInt))
        //                {
        //                    errorCode = -31009;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmHomeCfg_SetMode(netID, axisList[axis], tempInt, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31109;

        //            if (axisNode.SelectSingleNode("HomeSpeedMode") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("HomeSpeedMode").InnerText, out speedMode))
        //                {
        //                    errorCode = -31010;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31111;

        //            if (axisNode.SelectSingleNode("HomeWorkSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("HomeWorkSpeed").InnerText, out workSpeed))
        //                {
        //                    errorCode = -31012;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31112;

        //            if (axisNode.SelectSingleNode("HomeAccSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("HomeAccSpeed").InnerText, out accel))
        //                {
        //                    errorCode = -31000;
        //                    return false;
        //                }
        //            }

        //            if (axisNode.SelectSingleNode("HomeDecSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("HomeDecSpeed").InnerText, out decel))
        //                {
        //                    errorCode = -31013;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31113;

        //            if (axisNode.SelectSingleNode("HomeReverseSpeed") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("HomeReverseSpeed").InnerText, out reverseSpeed))
        //                {
        //                    errorCode = -31014;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31114;

        //            ecmHomeCfg_SetSpeedPatt(netID, axisList[axis], speedMode, workSpeed, accel, decel, reverseSpeed, ref errorCode);
        //            if(errorCode != 0)
        //                return false;

        //            if (axisNode.SelectSingleNode("HomeOffset") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("HomeOffset").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31015;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmHomeCfg_SetOffset(netID, axisList[axis], tempDouble, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31115;

        //            if (axisNode.SelectSingleNode("TouchProbeEdge") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("TouchProbeEdge").InnerText, out tempInt))
        //                {
        //                    errorCode = -31016;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmHomeCfg_SetOption(netID, axisList[axis], EEcmHomeOptID.ecmHOID_TPROB_EDGE_SEL, tempInt, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31116;

        //            if (axisNode.SelectSingleNode("RingCounterDirection") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("RingCounterDirection").InnerText, out tempInt))
        //                {
        //                    errorCode = -31017;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_Ring_SetDirMode(netID, axisList[axis], tempInt, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31117;

        //            if (axisNode.SelectSingleNode("RingCounterMaxValue") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("RingCounterMaxValue").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31018;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_Ring_SetPosRange(netID, axisList[axis], tempDouble, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31118;

        //            if (axisNode.SelectSingleNode("RingCounterEnable") != null)
        //                tempBool = axisNode.SelectSingleNode("RingCounterEnable").InnerText.Equals("True"));
        //            else
        //                errorCode = -31119;

        //            ecmSxCfg_Ring_SetEnable(netID, axisList[axis], tempBool, ref errorCode);
        //            if (errorCode != 0)
        //                return false;

        //            if (axisNode.SelectSingleNode("ControlMode") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("ControlMode").InnerText, out tempInt))
        //                {
        //                    errorCode = -31020;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetOperMode(netID, axisList[axis], (EEcmOperMode)(tempInt + 8), ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31120;

        //            if (axisNode.SelectSingleNode("SoftLimitEnable") != null)
        //                tempInt = axisNode.SelectSingleNode("SoftLimitEnable").InnerText.Equals("True") ? 1: 0;
        //            else
        //                errorCode = -31121;

        //            if (axisNode.SelectSingleNode("SoftLimitStopMode") != null)
        //                tempInt2 = axisNode.SelectSingleNode("SoftLimitStopMode").InnerText.Equals("True") ? 1 : 0;
        //            else
        //                errorCode = -31122;

        //            if (axisNode.SelectSingleNode("SoftLimitNValue") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("SoftLimitNValue").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31023;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31123;

        //            if (axisNode.SelectSingleNode("SoftLimitPValue") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("SoftLimitPValue").InnerText, out tempDouble2))
        //                {
        //                    errorCode = -31024;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31124;
                                        
        //            ecmSxCfg_SetSoftLimit(netID, axisList[axis], tempInt, tempDouble, tempDouble2, tempInt2, ref errorCode);
        //            if (errorCode != 0)
        //                return false;

        //            if (axisNode.SelectSingleNode("NotIgnore") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("NotIgnore").InnerText.Equals("True"));
        //                ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_IGNORE_ELN, tempBool ? 1 : 0, ref errorCode);
        //            }
        //            else
        //                errorCode = -31125;

        //            if (axisNode.SelectSingleNode("PotIgnore") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("PotIgnore").InnerText.Equals("True"));
        //                ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_IGNORE_ELP, tempBool ? 1 : 0, ref errorCode);
        //            }
        //            else
        //                errorCode = -31126;

        //            if (axisNode.SelectSingleNode("NotSignalType") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("NotSignalType").InnerText, out tempInt))
        //                {
        //                    errorCode = -31027;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_ELN_INPUT_SEL, tempInt, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31127;

        //            if (axisNode.SelectSingleNode("PotSignalType") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("PotSignalType").InnerText, out tempInt))
        //                {
        //                    errorCode = -31028;
        //                    return false;
        //                }
        //                else
        //                {
        //                    ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_ELP_INPUT_SEL, tempInt, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31128;

        //            if (axisNode.SelectSingleNode("AlarmStopMode") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("AlarmStopMode").InnerText.Equals("True"));
        //                ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_ALM_STOP_MODE, tempBool ? 1 : 0, ref errorCode);
        //                if (errorCode != 0)
        //                    return false;
        //            }
        //            else
        //                errorCode = -31129;

        //            if (axisNode.SelectSingleNode("ELStopMode") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("ELStopMode").InnerText.Equals("True"));
        //                ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_EL_STOP_MODE, tempBool ? 1 : 0, ref errorCode);
        //                if (errorCode != 0)
        //                    return false;
        //            }
        //            else
        //                errorCode = -31130;

        //            if (axisNode.SelectSingleNode("InpositionEnalbe") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("InpositionEnalbe").InnerText.Equals("True"));
        //                ecmSxCfg_SetMioProp(netID, axisList[axis], EEcmMioPropId.ecmMPID_INP_ENABLE, tempBool ? 1 : 0, ref errorCode);
        //                if (errorCode != 0)
        //                    return false;
        //            }
        //            else
        //                errorCode = -31131;

        //            if (axisNode.SelectSingleNode("MasterInpositionRange") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("MasterInpositionRange").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31032;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31132;

        //            if (axisNode.SelectSingleNode("MasterInpositionEnable") != null)
        //            {
        //                tempBool = axisNode.SelectSingleNode("MasterInpositionEnable").InnerText.Equals("True"));
        //                ecmSxCfg_SetMastInp(netID, axisList[axis], tempBool, tempDouble, ref errorCode);
        //                if (errorCode != 0)
        //                    return false;
        //            }
        //            else
        //                errorCode = -31133;

        //            if (axisNode.SelectSingleNode("MasterAxis") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("MasterAxis").InnerText, out tempInt))
        //                {
        //                    errorCode = -31033;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31133;

        //            if (axisNode.SelectSingleNode("MasterSlaveRatio") != null)
        //            {
        //                if (!double.TryParse(axisNode.SelectSingleNode("MasterSlaveRatio").InnerText, out tempDouble))
        //                {
        //                    errorCode = -31034;
        //                    return false;
        //                }
        //            }
        //            else
        //                errorCode = -31134;

        //            if (axisNode.SelectSingleNode("MasterSlaveEnable") != null)
        //            {
        //                if (!int.TryParse(axisNode.SelectSingleNode("MasterSlaveEnable").InnerText, out tempInt2))
        //                {
        //                    errorCode = -31035;
        //                    return false;
        //                }

        //                if(tempInt2 == 1)
        //                {
        //                    ecmMsCfg_SetSlvEnv(netID, axisList[axis], tempInt, tempDouble, ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;

        //                    ecmMsCtl_StartSlv(netID, axisList[axis], ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //                else
        //                {
        //                    ecmMsCtl_StopSlv(netID, axisList[axis], ref errorCode);
        //                    if (errorCode != 0)
        //                        return false;
        //                }
        //            }
        //            else
        //                errorCode = -31135;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return true;
        //}


        //====================== NET INTERFACE FUNCTIONS =========================================================//


        //EC_EXTERN t_success (CECAT_API *ecNet_SetEnableSTM) (t_i32 NetID, t_bool IsEnable, t_i32 *ErrCode);
        [DllImport(dll, EntryPoint = "ecNet_SetEnableSTM")]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SetEnableSTM(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool IsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //EC_EXTERN t_cmdidx	(CECAT_API *ecNet_SendRcvEcatCmd) (t_i32 NetID, EEcEcatCmd EcatCmd, t_ui16 ADP, t_ui16 ADO, t_ui16 DataSize, t_byte *pDataBuf, t_ui32 Timeout, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe uint ecNet_SendRcvEcatCmd(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            EEcEcatCmd EcatCmd,
            [MarshalAs(UnmanagedType.U2)] ushort ADP, 
            [MarshalAs(UnmanagedType.U2)] ushort ADO, 
            [MarshalAs(UnmanagedType.U2)] ushort DataSize, 
            [MarshalAs(UnmanagedType.U1)] ref byte pDataBuf,
            [MarshalAs(UnmanagedType.U4)] uint Timeout,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWDnld_Process) (t_i32 NetID, t_char szFilePath[], t_bool IsVerify, t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWDnld_Process(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.LPStr)] string szfilePath,
            [MarshalAs(UnmanagedType.I1)] bool IsVerify,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWDnld_Cancel) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWDnld_Cancel(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWDnld_GetSts) (t_i32 NetID, TEcFwuStatus *pStsBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWDnld_GetSts(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            ref TEcFwuStatus pStsBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWDnld_ResetSts) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWDnld_ResetSts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWUpld_GetFileInfo) (t_i32 NetID, TEcFwuFileInfo *pFileInfo, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWUpld_GetFileInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            ref TEcFwuFileInfo pFileInfo, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWUpld_Process) (t_i32 NetID, t_char szFilePath[], t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode, BestFitMapping = false)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWUpld_Process(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.LPStr)] string szfilePath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWUpld_Cancel) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWUpld_Cancel(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWUpld_GetSts) (t_i32 NetID, TEcFwuStatus *pStsBuf, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWUpld_GetSts(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            ref TEcFwuStatus pStsBuf,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_FWUpld_ResetSts) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_FWUpld_ResetSts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_CloseTwoPorts) (t_i32 NetID, t_i32 SlaveIdx1, t_i32 PortIdx1, t_i32 SlaveIdx2, t_i32 PortIdx2, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_CloseTwoPorts(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx1,
            [MarshalAs(UnmanagedType.I4)] int PortIdx1,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx2,
            [MarshalAs(UnmanagedType.I4)] int PortIdx2,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecNet_GetEnableRedundSupport) (t_i32 NetID, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_GetEnableRedundSupport(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecNet_SetEnableRedundSupport) (t_i32 NetID, t_bool bIsEnable, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_SetEnableRedundSupport(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I1)] bool bIsEnable,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        //====================== DC Delay Measure =========================================================//
        //EC_EXTERN t_success(CECAT_API* ecNet_DCMeas_Start) (t_i32 NetID, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_DCMeas_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,            
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecNet_DCMeas_Abort) (t_i32 NetID, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_DCMeas_Abort(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_f64(CECAT_API* ecNet_DCMeas_GetProgress) (t_i32 NetID, t_i32* ErrCode); // return: progress value as %
        [DllImport(dll)]        
        public static extern unsafe double ecNet_DCMeas_GetProgress(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool(CECAT_API* ecNet_DCMeas_IsBusy) (t_i32 NetID, t_i32* ErrCode); // return: 
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_DCMeas_IsBusy(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32(CECAT_API* ecNet_DCMeas_GetStatus) (t_i32 NetID, t_i32* ErrCode); // return: 1=>success, 0=>undone, 음수:에러코드
        [DllImport(dll)]
        public static extern unsafe int ecNet_DCMeas_GetStatus(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecNet_DCMeas_GetSlaveInfo_A) (t_i32 NetID, t_i32 SlaveIdx, TEcDCMeasInfoOfSlv* pDCSetupInfo, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_DCMeas_GetSlaveInfo_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SlaveIdx,
            //ref TEcDCMeasInfoOfSlv pDCSetupInfo,
            ref TEcDCMeasInfoOfSlv pDCSetupInfo,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success(CECAT_API* ecNet_DCMeas_GetSlaveInfo) (t_i32 NetID, t_ui16 SlvPhysAddr, TEcDCMeasInfoOfSlv* pDCSetupInfo, t_i32* ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecNet_DCMeas_GetSlaveInfo(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            //ref TEcDCMeasInfoOfSlv pDCSetupInfo,
            ref TEcDCMeasInfoOfSlv pDCSetupInfo,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);


        ////====================== ECAT CONFIG FUNCTIONS ==========================================================//

        //EC_EXTERN t_success (CECAT_API *ecCfg_Start) (t_i32 NetID, BOOL IsApplyTimeout, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_Start(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.Bool)] bool IsApplyTimeout,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_End) (t_i32 NetID, BOOL IsSave, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_End(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I1)] bool IsSave, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecCfg_SetNetConfig) (t_i32 NetID, _in TEcNetConfig *pNetCfgHeader, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_SetNetConfig(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            ref TEcNetConfig pNetCfgHeader, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
        //EC_EXTERN t_success	(CECAT_API *ecCfg_GetNetConfig) (t_i32 NetID, _out TEcNetConfig *pNetCfgHeader, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_GetNetConfig(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            ref TEcNetConfig pNetCfgHeader,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_SetOutPDOLogicSect) (t_i32 NetID, t_i32 SectIdx, _in TEcLogMemSectCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_SetOutPDOLogicSect(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int SectIdx,
            ref TEcNetConfig pCfgData, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_GetOutPDOLogicSect) (t_i32 NetID, t_i32 SectIdx, _out TEcLogMemSectCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_GetOutPDOLogicSect(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SectIdx, 
            ref TEcNetConfig pCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_SetInPDOLogicSect) (t_i32 NetID, t_i32 SectIdx, _in TEcLogMemSectCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_SetInPDOLogicSect(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int SectIdx, 
            ref TEcNetConfig pCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_GetInPDOLogicSect) (t_i32 NetID, t_i32 SectIdx, _out TEcLogMemSectCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_GetInPDOLogicSect
            ([MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SectIdx, 
            ref TEcNetConfig pCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecCfg_SetSlaveConfig) (t_i32 NetID, t_i32 SlaveIdx, _in TEcSlaveCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_SetSlaveConfig(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.I4)] int SectIdx,
            ref TEcSlaveCfg pCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecCfg_GetSlaveConfig) (t_i32 NetID, t_i32 SlaveIdx, _out TEcSlaveCfg *pCfgData, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_GetSlaveConfig(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.I4)] int SectIdx,
            ref TEcSlaveCfg pCfgData,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_SetChanMap) (t_i32 NetID, t_ui8 MapType, t_i32 ChanMapIdx, _in void* pChanMapData, t_i32 ChanMapDataSize, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_SetChanMap(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] int MapType, 
            [MarshalAs(UnmanagedType.I4)] int ChanMapIdx,
            ref IntPtr pChanMapData, 
            [MarshalAs(UnmanagedType.I4)] int ChanMapDataSize, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecCfg_GetChanMap) (t_i32 NetID, t_ui8 MapType, t_i32 ChanMapIdx, _in void* pChanMapBuf, t_i32 ChanMapBufSize, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecCfg_GetChanMap(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U1)] int MapType,
            [MarshalAs(UnmanagedType.I4)] int ChanMapIdx,
            ref IntPtr pChanMapData, 
            [MarshalAs(UnmanagedType.I4)] int ChanMapBufSize, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        
        ////====================== XML FILE READING FUNCTIONS ==========================================================//

        //EC_EXTERN HANDLE	(CECAT_API *ecXml_LoadByProdInfo) (char *szXmlDirectory, TEcSlvProdInfo *pProdInfo, _out char *szFileNameBuf, t_i32 FileNameBufLen, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecXml_LoadByProdInfo(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szXmlDirectory, 
            ref TEcSlvProdInfo pProdInfo,
            [Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szFileNameBuf, 
            [MarshalAs(UnmanagedType.I4)] int FileNameBufLen,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN HANDLE	(CECAT_API *ecXml_LoadByFileName) (char *szXmlDirectory, char *szFileName, t_i32 *ErrCode);
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern unsafe int ecXml_LoadByFileName(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szXmlDirectory,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szFileName, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN void		(CECAT_API *ecXml_Unload) (HANDLE XmlHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe void ecXml_Unload(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_GetTagName) (HANDLE XmlHandle, char *szTagNameBuf, t_i32 TagNameBufLen, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_GetTagName(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szTagNameBuf, 
            [MarshalAs(UnmanagedType.I4)] int TagNameBufLen, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);
        
        //EC_EXTERN t_i32		(CECAT_API *ecXml_GetNumElem) (HANDLE XmlHandle, t_bool IsResetPos, const char *szElemPath, t_i32 *ErrCode); -ysy const 처리안함
        [DllImport(dll)]
        public static extern unsafe int ecXml_GetNumElem(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle, 
            [MarshalAs(UnmanagedType.I1)] bool IsResetPos,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szElemPath, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success	(CECAT_API *ecXml_FindElem) (HANDLE XmlHandle, t_bool IsResetPos, const char *szElemPath, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_FindElem(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle, 
            [MarshalAs(UnmanagedType.I1)] bool IsResetPos,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szElemPath,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_IntoElem) (HANDLE XmlHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_IntoElem(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_OutOfElem) (HANDLE XmlHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_OutOfElem(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_ResetPos) (HANDLE XmlHandle, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_ResetPos(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_SavePos) (HANDLE XmlHandle, char *szPosName, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_SavePos(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szPosName,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_RestorePos) (HANDLE XmlHandle, char *szPosName, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_RestorePos(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szPosName,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_ReadElemData) (HANDLE XmlHandle, char *szDataBuf, t_i32 DataBufLen, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_ReadElemData(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szDataBuf,
            [MarshalAs(UnmanagedType.I4)] int DataBufLen,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_ReadElemAttr) (HANDLE XmlHandle, const char *szAttrName, char *szDataBuf, t_i32 DataBufLen, t_i32 *ErrCode); //ysy const 처리안함
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_ReadElemAttr(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szAttrName,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szDataBuf,
            [MarshalAs(UnmanagedType.I4)] int DataBufLen,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_FindReadElemData) (HANDLE XmlHandle, t_bool IsResetPos, const char *szElemPath, char *szDataBuf, t_i32 DataBufLen, t_i32 *ErrCode); //ysy const 처리안함
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_FindReadElemData(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I1)] bool IsResetPos,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szElemPath,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szDataBuf,
            [MarshalAs(UnmanagedType.I4)] int DataBufLen, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_FindReadElemAttr) (HANDLE XmlHandle, t_bool IsResetPos, const char *szElemPath, const char *szElemAttrName, char *szDataBuf, t_i32 DataBufLen, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_FindReadElemAttr(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.I1)] bool IsResetPos,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szElemPath,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szElemAttrName,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szDataBuf, 
            [MarshalAs(UnmanagedType.I4)] int DataBufLen, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_FindDeviceByProdInfo) (HANDLE XmlHandle, TEcSlvProdInfo *pProdInfo, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_FindDeviceByProdInfo(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            ref TEcSlvProdInfo pProdInfo, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_bool	(CECAT_API *ecXml_Str2Bool) (char *szString, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_Str2Bool(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szString,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_i32		(CECAT_API *ecXml_Str2HexDec) (char *szString, t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe int ecXml_Str2HexDec(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szString, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success (CECAT_API *ecXml_GetProdDesc) (HANDLE XmlHandle, char *szLcId, TEcSlvProdInfo *pProdInfo, TEcSlvProdDesc *pProdDesc, t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecXml_GetProdDesc(
            [MarshalAs(UnmanagedType.I4)] int XmlHandle,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] char[] szLcId,
            ref TEcSlvProdInfo pProdInfo, ref TEcSlvProdDesc pProdDesc, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);



        ////====================== SLAVE I/F FUNCTIONS ============================================================//

        //EC_EXTERN TEcSdoiODList*	(CECAT_API *ecSlv_SdoInfo_GetODList) (t_i32 NetID, t_ui16 SlvPhysAddr, EEcSdoiODListType ODListType, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiODList ecSlv_SdoInfo_GetODList(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            EEcSdoiODListType ODListType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcSdoiODList*	(CECAT_API *ecSlv_SdoInfo_GetODList_A) (t_i32 NetID, t_ui16 SlaveIdx, EEcSdoiODListType ODListType, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiODList ecSlv_SdoInfo_GetODList_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIdx,
            EEcSdoiODListType ODListType,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcSdoiObjDesc*	(CECAT_API *ecSlv_SdoInfo_GetObjDesc) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui16 ObjIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiObjDesc ecSlv_SdoInfo_GetObjDesc(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr, 
            [MarshalAs(UnmanagedType.U2)] ushort ObjIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcSdoiObjDesc*	(CECAT_API *ecSlv_SdoInfo_GetObjDesc_A) (t_i32 NetID, t_ui16 SlaveIdx, t_ui16 ObjIndex, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiObjDesc ecSlv_SdoInfo_GetObjDesc_A(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIdx, 
            [MarshalAs(UnmanagedType.U2)] ushort ObjIndex, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcSdoiEntryDesc* (CECAT_API *ecSlv_SdoInfo_GetEntryDesc) (t_i32 NetID, t_ui16 SlvPhysAddr, t_ui16 ObjIndex, t_ui8 SubIndex, t_ui8 ValueInfo, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiEntryDesc ecSlv_SdoInfo_GetEntryDesc(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlvPhysAddr,
            [MarshalAs(UnmanagedType.U2)] ushort ObjIndex, 
            [MarshalAs(UnmanagedType.U1)] int SubIndex,
            [MarshalAs(UnmanagedType.U1)] int ValueInfo, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN TEcSdoiEntryDesc* (CECAT_API *ecSlv_SdoInfo_GetEntryDesc_A) (t_i32 NetID, t_ui16 SlaveIdx, t_ui16 ObjIndex, t_ui8 SubIndex, t_ui8 ValueInfo, _out t_i32 *ErrCode);
        [DllImport(dll)]
        public static extern unsafe TEcSdoiEntryDesc ecSlv_SdoInfo_GetEntryDesc_A(
            [MarshalAs(UnmanagedType.I4)] int NetID, 
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIdx, 
            [MarshalAs(UnmanagedType.U2)] ushort ObjIndex, 
            [MarshalAs(UnmanagedType.U1)] int SubIndex,
            [MarshalAs(UnmanagedType.U1)] int ValueInfo, 
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

        //EC_EXTERN t_success			(CECAT_API *ecSlv_SdoInfo_ReleaseData) (t_i32 NetID, t_ui16 SlaveIdx, void *pSdoiStruct, _out t_i32 *ErrCode);
        [DllImport(dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern unsafe bool ecSlv_SdoInfo_ReleaseData(
            [MarshalAs(UnmanagedType.I4)] int NetID,
            [MarshalAs(UnmanagedType.U2)] ushort SlaveIdx, 
            ref IntPtr pSdoiStruct,
            [MarshalAs(UnmanagedType.I4)] ref int ErrCode);

    }
}
