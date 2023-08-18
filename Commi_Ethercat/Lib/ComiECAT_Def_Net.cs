using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

// Date : 2020.08.20
// Ver : 1.0.0
namespace ComiLib.EtherCAT
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public partial class SafeNativeMethods
    {
        public enum EEcLocNetSel
        {
            ecNETSEL_NET0 = 0,
            ecNETSEL_NET1 = 1,
            ecNETSEL_ALL = 2
        }


        public enum EEcLogSectType
        {
            ecLOGSECT_DI,
            ecLOGSECT_DO,
            ecLOGSECT_AI,
            ecLOGSECT_AO,
            ecLOGSECT_SERVO,
            ecLOGSECT_ETC = 15,
            ecLOGSECT_INVALID = 16
        }


        public enum EEcAxisType
        {
            ecAXIS_TYPE_NONE, // Slave가 모션제어 기능이 없는 경우
            ecAXIS_TYPE_SERVO, // Slave가 servo driver인 경우
            ecAXIS_TYPE_STEP, // Slave가 step driver인 경우
            ecAXIS_TYPE_PULSEMOTION // Slave가 Pulse Motion Controller인 경우.
        }


        public enum EEcPdoSyncMode
        {
            FreeRUN, 
            SM2_Event,
            DC, 
            NONE = 0xf, // PDO Sync Mode 셋팅을 지원하지 않는 경우.
        }


        public enum EEcChanMapType
        {
            ecCH_MAP_TYPE_DI, // Digital Input Channel Mapping
            ecCH_MAP_TYPE_DO, // Digital Output Channel Mapping
            ecCH_MAP_TYPE_AI, // Analog Input Channel Mapping
            ecCH_MAP_TYPE_AO, // Analog Output Channel Mapping
            ecCH_MAP_TYPE_INVALID
        }


        public enum EEcAiScaleType
        {
            ecAI_ST_MINMAX, // Scale데이터가 Min/Max 값을 나타내는 경우.
            ecAI_ST_MULTIPLY, // Scale 데이터가 곱하기값으로 사용되는 경우. 이 때는 ScaleMin 값이 계수값으로 사용된다.  
            ecAI_ST_INVALID
        }


        public enum EEcAoScaleType
        {
            ecAO_ST_MINMAX, // Scale데이터가 Min/Max 값을 나타내는 경우.
            ecAO_ST_MULTIPLY, // Scale 데이터가 곱하기값으로 사용되는 경우. 이 때는 ScaleMin 값이 계수값으로 사용된다.  
            ecAO_ST_INVALID
        }


        public enum LogMemSectType
        {
            DI,
            DO,
            AI,
            AO,
            SERVO,
            ETC = 15
        }
        

        public enum EEcAliasILMode
        {
            NONE,
            IdentificationADO,
            IdentificationREG134,
            INVALID
        };

        // FPRD/FPWR 시에 사용되는 Address 모드 //
        public enum EEcFPRWAddrMode
        {
	        ALIAS, 	// Alias 레지스터(0x12) 사용
	        CSA, 	// Configured Station Address(CSA) 레지스터(0x10) 사용
            INVALID
        };

        public enum RegisterType
        {
            ESC10_ESC20 = 0x2,
            IPcore = 0x4,
            ET1100 = 0x11,
            ET1200 = 0x12,
            //Other FPGA-based BackhoffESCs
            First_teminlas = 0x1,
            FirstEK1100 = 0x3,
            CurrentTerminals = 0x5,
        }


        //============================================================================================
        //  Structures
        //============================================================================================

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TCfgFileHeader
        {
            [Serializable]
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Version
            {
                public ushort Major;
                public ushort Minor;
            };

            [Serializable]
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct Info
            {
                public ushort DevIdx;
                public ushort LcaNetIdx;
                public uint Resv1;
                public uint Resv2;
            };

            public Version version;
            public Info info;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public uint[] dwReserved;
        }


        // 각 슬레이브의 Device Type 정보 : ecSlv_GetDevTypeInfo() 함수의 Argument로 사용됨. //
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcDevTypeInfo
        {
            public ushort DeviceType; // CoE SDO 1000h 값 참조.
            public ushort NumDI;
            public ushort NumDO;
            public ushort NumAI;
            public ushort NumAO;
            public ushort NumServo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Resv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSmCfg
        {
            public ushort StaAddr;
            public ushort Len;
            public byte CtlByte;
            public byte ActByte;
            public ushort Resv; // Dummy 변수 4바이트 정렬을 맞추기 위해서 있는 것임.
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcFmmuCfg
        {
            public uint StaLAddr;
            public ushort Len;
            public byte LStaBit;
            public byte LStopBit;
            public ushort StaPAddr;
            public byte PStaBit;
            public byte Type;
            public byte Activate;
            public byte IoType;
            public byte Resv2;
            public byte Resv3;
        }
        

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TFmmu
        {
            public ushort num;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)] // MAX_NUM_FMMU = 3;
            public TFmmuAttr[] Attr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] Resv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TFmmuAttr
        {
            public short Sm;
            public ushort Type;
            public bool OpOnly;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public uint[] Resv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TSm
        {
            public ushort num;
            public byte numOut;
            public byte numIn;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_SM)] 
            public TSmAttr[] Attr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] Resv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TSmAttr
        {
            public ushort Type;
            public ushort MinSize;
            public ushort MaxSize;
            public ushort DefaultSize;
            public uint StartAddress;
            public uint ControlByte;
            public bool Enable;
            public bool OpOnly;
            public int IoType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public uint[] Resv;
        }


        // ESI의 "DeviceType:Mailbox:CoE" 항목 //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcMbxCoE
        {
#if BitFieldUsing
            [BitFieldInfo(0, 1)]    public uint IsSupport { get; set; }
            [BitFieldInfo(1, 1)]    public uint IsPdoAssign { get; set; }
            [BitFieldInfo(2, 1)]    public uint IsPdoConfig { get; set; }
            [BitFieldInfo(3, 1)]    public uint IsPdoUpload { get; set; }
            [BitFieldInfo(4, 1)]    public uint IsComptAccess { get; set; }
            [BitFieldInfo(5, 1)]    public uint IsSegmSdoSupp { get; set; }
            [BitFieldInfo(6, 26)]   public uint Resv { get; set; } 

#else
            uint data;
            public uint IsSupport
            {
                get { return data & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 0);
                    else
                        data &= ~((value & 1) << 0);
                }
            }

            public uint IsPdoAssign
            {
                get { return (data >> 1) & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 1);
                    else
                        data &= ~((value & 1) << 1);
                }
            }

            public uint IsPdoConfig
            {
                get { return (data >> 2) & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 2);
                    else
                        data &= ~((value & 1) << 2);
                }
            }

            public uint IsPdoUpload
            {
                get { return (data >> 3) & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 3);
                    else
                        data &= ~((value & 1) << 3);
                }
            }

            public uint IsComptAccess
            {
                get { return (data >> 4) & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 4);
                    else
                        data &= ~((value & 1) << 4);
                }
            }

            public uint IsSegmSdoSupp
            {
                get { return (data >> 5) & 1; }
                set
                {
                    if ((value & 1) == 1)
                        data |= ((value & 1) << 5);
                    else
                        data &= ~((value & 1) << 5);
                }
            }
#endif
        }



        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcPdoAssObj
        {
            public byte SmIdx;
            public byte NumAssVals;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public ushort[] AssVal;	    // 이멤버의 배열 크기는 255로 설정되어 있으나 이것은 구조체의 틀을 잡기 위한 것이고 실제 사용되는 AssVal 배열의 크기는 NumAssVals 가 결정한다.
        }


        // Slave configuration //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcSlaveCfg
        {
            public uint VendId;
            public uint ProdCode;
            public short AutoAddr; // 자동주소. 연결순서대로 매겨지는 주소로서 첫번째 슬레이브는 0 두번째는 -1, 세번째는 -2,....의 순으로 값이 설정된다.
            public ushort PhysAddr; // 물리주소

#if BitFieldUsing
            [BitFieldInfo(0, 4)]   public uint SmIdx_OutMbx { get; set; }
            [BitFieldInfo(4, 4)]   public uint SmIdx_InMbx { get; set; }
            [BitFieldInfo(8, 4)]   public uint AxisType { get; set; }
            [BitFieldInfo(12, 4)]  public uint NumSM { get; set; }
            [BitFieldInfo(16, 4)]  public uint SmSyncMode { get; set; }
            [BitFieldInfo(20, 2)]  public uint AliasILMode { get; set; }
            [BitFieldInfo(22, 2)]  public uint AliasILSize { get; set; }
            [BitFieldInfo(24, 1)]  public uint FPRWAddrMode { get; set; }
            [BitFieldInfo(25, 7)]  public uint Resv1 { get; set; }

#else
            private uint data;

            public uint SmIdx_OutMbx
            {
                get { return (data & 0xF); }
                set { data = ((data & 0xFFFFFFF0) | (value & 0xF)); }
            }

            public uint SmIdx_InMbx
            {
                get { return ((data >> 4) & 0xF); }
                set { data = ((data & 0xFFFFFF0F) | ((value & 0xF) << 4)); }
            }

            public uint AxisType    // 0-Axis가 아님, 1-servo driver, 2-step driver, 3-pusle motion, 4-other
            {
                get { return ((data >> 8) & 0xF); }
                set { data = ((data & 0xFFFFF0FF) | ((value & 0xF) << 8)); }
            }

            public uint NumSM
            {
                get { return ((data >> 12) & 0xF); }
                set { data = ((data & 0xFFFF0FFF) | ((value & 0xF) << 12)); }
            }

            public uint SmSyncMode
            {
                get { return ((data >> 16) & 0xF); }
                set { data = ((data & 0xFFF0FFFF) | (value & 0xF) << 16); }
            }

            public uint AliasILMode
            {
                get { return ((data >> 20) & 0x3); }
                set { data = ((data & 0xFFCFFFFF) | (value & 0x3) << 20); }                
            }

            public uint AliasILSize
            {
                get { return ((data >> 22) & 0x3); }
                set { data = ((data & 0xFF3FFFFF) | (value & 0x3) << 22); }
            }

            public uint FPRWAddrMode
            {
                get { return ((data >> 24) & 0x1); }
                set { data = ((data & 0xFEFFFFFF) | (value & 0x1) << 24); }
            }

            public uint IsVirtual
            {
                get { return ((data >> 25) & 0x1); }
                set { data = ((data & 0xFDFFFFFF) | (value & 0x1) << 25); }
            }

            public uint UsingDI
            {
                get { return ((data >> 26) & 0x1); }
                set { data = ((data & 0xFBFFFFFF) | (value & 0x1) << 26); }
            }

            public uint UsingDO
            {
                get { return ((data >> 27) & 0x1); }
                set { data = ((data & 0xF7FFFFFF) | (value & 0x1) << 27); }
            }

            //public uint Resv1
            //{
            //    get { return ((data >> 25) & 0x7F); }
            //    set { data = ((data & 0x1FFFFFFF) | (value & 0x7F) << 25); }
            //}
#endif
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ecMAX_NUM_SMMU)]  // SizeConst = ecMAX_NUM_FMMU
            public TEcSmCfg[] SM;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ecMAX_NUM_FMMU)]  // SizeConst = ecMAX_NUM_FMMU
            public TEcFmmuCfg[] FMMU;
            public TEcMbxCoE MbxCoE;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = ecPDO_ASSGN_SECT_SIZE)] // SizeConst = ecPDO_ASSGN_SECT_SIZE
            public byte[] PdoAssData;

            public ushort AliasILReg;
            public ushort AliasILValue;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPdo
        {
            public ushort RxNum;	// RxPDO 수
            public ushort TxNum;	// RxPDO 수
            public ushort TxLength;
            public ushort RxLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]//#define MAX_NUM_PDO_ASSIGN	32
            public TPdoAttr[] Rx;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] //#define MAX_NUM_PDO_ASSIGN	32
            public TPdoAttr[] Tx;
            public ushort usingEntryType;            
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] uiResv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPdoAttr
        {
            public ushort ExcludeNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public ushort[] Excludes;
            public bool Fixed;
            public bool Mandatory;
            public bool IsSm;
            public bool Resv0;
            public ushort SmIdx;
            public ushort Index;
            public ushort NumEntry;
            public ushort Size;
            public ushort LstIdx;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)] //#define CSR_DEV_ELEM		40        
            public string Name;
            public int usingIoType;
            public int IoType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public uint[] uiResv;
            
            public TPdoEntryAttr[] Entry;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPdoEntryAttr
        {
            public ushort Index;		// M
            public ushort SubIndex;
            public ushort BitLen;		// M
            public byte DScaleNo;
            public byte DataTypeNo;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string Name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string DataType;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TAlias
        {
            public byte ILMode;
            public ushort ILReg;
            public byte ILSize;
            public ushort Value;
        }
        

        // a 'Logical Memory Section' configuration //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcLogMemSectCfg
        {
            //char szName[ecMAX_LMEM_SECT_NAME_LEN];
            public byte Type; // 0-DI, 1-DO, 2-AI, 3-AO, 4-SERVO, 5-ETC

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] Resv;
            public ushort StartAddr; // Section의 시작주소 (Logical Memory 내에서의 주소), 2048보다 작아야 한다.
            public ushort Length; // Section 바이트 크기. 2048보다 작아야 한다. 
        }



        // Network configuration header //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcNetConfig
        {
            public uint Version;  // <= t_ui32	NetID;
            public uint CycleTime;
            public byte NumOutpdoLogSects; // Number of logical memory sections which has been defined
            public byte NumInpdoLogSects; // Number of logical memory sections which has been defined
            public ushort NumSlaves; // Number of cofgured slaves
            public ushort NumDiChanMaps;
            public ushort NumDoChanMaps;
            public ushort NumAiChanMaps;
            public ushort NumAoChanMaps;

            uint data;
            public uint IsAllFPRWAlias
            {
                get { return (data & 0x1); }
                set { data = ((data & 0xFFFFFFFE) | value); }
            }

            public uint IsRedundSupport
            {
                get { return ((data >> 1) & 1); }
                set { data = ((data & 0xFFFFFFFD) | value << 1); }
            }

            public uint Resv1
            {
                get { return (data & 0xFFFFFFFC); }
                set { data = ((data & 0x3) | (value & 0xFFFFFFFC)); }
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
            public byte[] Resv;
        }


        

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TNetworkInfo
        {
            public TEcNetConfig EcNetCfg;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            TEcLogMemSectCfg[]	EcLogMemOut;	// DO, AO, SERVO, ETC
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        TEcLogMemSectCfg[]	EcLogMemIn;	// DI, AI, SERVO, ETC

	        ushort				LenRxPdo;
	        ushort				LenTxPdo;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_DEV_TYPE)]
	        short[]				DevNum;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        uint[]				uiResv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_MAX_PATH)]
	        char[]				szResv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TChanInfo
        {
            public ushort DiChanMapCnt;
            public ushort DoChanMapCnt;
            public ushort AiChanMapCnt;
            public ushort AoChanMapCnt;

            //[Serializable]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            public TEcDiChanMap[] DiChanMap;

            //[Serializable]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            public TEcDoChanMap[] DoChanMap;

            //[Serializable]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            public TEcAioChanMap[] AiChanMap;

            //[Serializable]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
            public TEcAioChanMap[] AoChanMap;
        };

        
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TDScaleInfo
        {
            public byte	Type;
	        public float	ScaleMin;
	        public float	ScaleMax;
	        public string  Name;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TDScale
        {
            public byte         No;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_DSCALE)]
	        public TDScaleInfo[]	Info;
        }



        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TDc
        {
            public ushort NumOpMode;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)] //MAX_NUM_DC_OPMODE = 8
            public TDcInfo[] Info;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TDcInfo
        {
            public string Name;
            public string Desc;
            public ushort AssignActivate;
            public ushort CylTmSync0;
            public ushort CylTmSync0Factor;
            public ushort ShftTmSync0;
            public ushort CylTmSync1;
            public ushort CylTmSync1Factor;
            public ushort ShftTmSync1;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public uint[] uiResv;
        }


        


        // Digital Input Global Channel 맵핑 정보 //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcDiChanMap
        {
            public ushort SlvIdx;
            public ushort StaGlobChan;
            public ushort StaLogAddr; // start logical address
            public byte StaLogBit;	// start bit of logical data
            public byte NumChans;	// number of channels in this channel group
        }


        // Digital Output Global Channel 맵핑 정보 //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcDoChanMap
        {
            public ushort SlvIdx;
            public ushort StaGlobChan;
            public ushort StaLogAddr; // start logical address
            public byte StaLogBit;	// start bit of logical data
            public byte NumChans;	// number of channels in this channel group
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcAioChanMap
        {
            public ushort SlvIdx;
            public ushort StaLogAddr;
            public uint Data;
            public uint StaLogBit // :4
            {
                get { return (uint)(Data & 0xf); }
                set { Data = (Data & 0xfffffff0) | (uint)(value & 0xf); }
            }

            public uint BitLength // :6
            {
                get { return (uint)((Data >> 4) & 0x3f); }
                set { Data = (uint)((Data & 0xFFFFFC0F) | ((value & 0x3F) << 4)); }
            }

            public uint DataType // :3 // 0-signed int, 1-unsigned int, 2-float; EEcAiDataType 타입 참조 
            {
                get { return (uint)((Data >> 10) & 0x7); }
                set { Data = (uint)((Data & 0xFFFFE3FF) | ((value & 0x7) << 10)); }
            }

            public uint ScaleType  // :3 //0-MinMax, 1-Multiply; EEcAiScaleType 타입 참조 
            {
                get { return (uint)((Data >> 13) & 0x7); }
                set { Data = (uint)((Data & 0xFFFF1FFF) | ((value & 0x7) << 13)); }
            }

            public float ScaleMin;	// Scale Min. Value, 단 ScaleType=ecAI_ST_MULTIPLY인 경우에는 이 값은 Multiplication 값을 셋팅한다.  
            public float ScaleMax;	// Scale Max. Value, 단 ScaleType=ecAI_ST_MULTIPLY인 경우에는 이 값은 무시된다.
        };


        //// Analog Output Global Channel 맵핑 정보 //
        //[Serializable]
        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        //public struct TEcAoChanMap
        //{
        //    public ushort SlvIdx;
        //    public ushort StaLogAddr;
        //    public uint Data;
        //    public uint StaLogBit // :4
        //    {
        //        get { return (uint)(Data & 0xf); }
        //        set { Data = (Data & 0xfffffff0) | (uint)(value & 0xf); }
        //    }

        //    public uint BitLength // :6
        //    {
        //        get { return (uint)((Data >> 4) & 0x3f); }
        //        set { Data = (uint)((Data & 0xFFFFFC0F) | ((value & 0x3F) << 4)); }
        //    }

        //    public uint DataType // :3 // 0-signed int, 1-unsigned int, 2-float; EEcAiDataType 타입 참조 
        //    {
        //        get { return (uint)((Data >> 10) & 0x7); }
        //        set { Data = (uint)((Data & 0xFFFFE3FF) | ((value & 0x7) << 10)); }
        //    }

        //    public uint ScaleType  // :3 //0-MinMax, 1-Multiply; EEcAiScaleType 타입 참조 
        //    {
        //        get { return (uint)((Data >> 13) & 0x7); }
        //        set { Data = (uint)((Data & 0xFFFF1FFF) | ((value & 0x7) << 13)); }
        //    }

        //    public float ScaleMin;	// Scale Min. Value, 단 ScaleType=ecAI_ST_MULTIPLY인 경우에는 이 값은 Multiplication 값을 셋팅한다.  
        //    public float ScaleMax;	// Scale Max. Value, 단 ScaleType=ecAI_ST_MULTIPLY인 경우에는 이 값은 무시된다.
        //}

        
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdHeader
        {
	        public int	ESCDSectSignature;	    // 0x50321234
            public int	ESCDSectTotSize;        // 단 ESCDSectSignature 항목은 크기 계산에서 제외
            public int	NumSlaves;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] Reserved;
        };

        
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdSlvHeader
        {
            public short SlaveSectSize;
            public short NumSlvCfgItems;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdSlvData
        {
            public short MapIndex;
            public sbyte NumMapItems;
            public sbyte IsFixedMap;
            public int[] MapData;
        }


        // ESCD 데이터 아이템 중에서 PDO 맵핑 데이터용 구조체 (ESCD Type 0 & 1)  //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdSlvItem
        {
            public short DataType;
            public short DataSize;
            public TEcEscdSlvData Data;		// Entry 수
        }


        // ESCD 데이터 아이템 중에서 SlaveAxisInfo 데이터용 구조체 (ESCD Type 0 & 1)  //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdSlvAxisInfo
        {
            public short DataType;
            public short DataSize;
            public TSlvAxisInfo SlvAxisInfo;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscdSlv
        {
            public TEcEscdSlvHeader Header;
            public ushort numMappingItems;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public TEcEscdSlvItem[] Item;		// PDO 수
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public TEcEscdSlvAxisInfo SlvAxisInfo;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TEcEscd
        {
            public TEcEscdHeader Header;
            //public IntPtr Slave;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public TEcEscdSlv[] Slave;		// ESCD 슬레이브 수
        }


        // 슬레이브가 Axis 슬레이브인 경우에 ComiEcatDevInfo.xml 파일에서 설정해주는 정보 //
        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TSlvAxisInfo
        {
            public byte IsMultiAxes;    // 다축을 지원하는 슬레이브인지...
            public byte Resv;

            //< 각 축의 SDO 인덱스의 Offset 값 
            public ushort SdoIdxOfs;            

            // PDO맵인덱스 값을 로컬축번호로 변환하는데 필요한 정보 (IsMultiAxes=1인 경우에만 사용됨) //
            [Serializable]
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct PDOM2Axis
            {
                ///////////////////////////////////////////////////////////////////////////////
                // (BaseMapIdx <= PDOMap <= MaxMapIdx)조건을 만족하는 맵번호에 대해서
                // LocalAxis = ((PDOMap & Mask) >> Shift) 의 식으로 축번호를 계산한다.
                // 예를들어서 Mask=0xF0, Shift=4, PDOMap=0x1610이면, LocalAxis=((0x1610 & 0xF0)>>4)=1이 된다.
                // 그리고 OutputPDO에 대해서는 BaseMapIdx = 0x1600 + BaseMapIdxOfs 가 되고, InputPDO에 대해서는 BaseMapIdx = 0x1A00 + BaseMapIdxOfs 가 된다.
                // 마찬가지로 OutputPDO에 대해서는 MaxMapIdx = 0x1600 + MaxMapIdxOfs 가 되고, InputPDO에 대해서는 BaseMapIdx = 0x1A00 + MaxMapIdxOfs 가 된다.

                public ushort BaseMapIdxOfs; // 로컬축번호를 구분하기 위해서 사용되는 PDO맵번호의 최소값 (0x1600 또는 0x1A00 으로부터의 Offset값으로 정의함)
                public ushort MaxMapIdxOfs; // 로컬축번호를 구분하기 위해서 사용되는 PDO맵번호의 최대값 (0x1600 또는 0x1A00 으로부터의 Offset값으로 정의함)
                public ushort Mask; // 맵번호를 마스킹하는 값
                public ushort Shift; // 맵번호를 쉬프트하는 값
            }

            public PDOM2Axis pdoM2Axis;
            public ushort SlotScanObject;
        }

        

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPrevDev
        {
            public ushort DevType;
            public uint VendorId;
            public uint ProductCode;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TBackupNetInfo
        {
            public bool IsChange;
            public bool IsFile;
            public string Path;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TXmlVen
        {
            uint 	Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_DEV_NAME)]
	        char[] 	Name;
	        IntPtr 	Icon;
	        uint	IconNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        uint[]	uiResv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TXmlGrp
        {
            uint	VenIdx;
	        byte	PhysisType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
	        char[] 	Type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_DEV_NAME)]
	        char[] 	Name;
	        IntPtr 	Icon;
	        uint	IconNo;
	        short	GrpDevType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	        uint[]	uiResv;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TXmlDev
        {
            byte	PhysisType;	// 0 : Slave("YY"), 1 : Coupler Slave("KK"), 2 : Coupler("YKY")
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	        char[] 	Physis;
	        uint	ProductCode;
	        uint	RevisionNo;
	        ushort	ProfileNo;
	        ushort	Alias;
	        ushort  ModProfNo;

            bool	bIsInfoFile;
	        short	NumRxCh, NumTxCh;
            short	NumRxPdo, NumTxPdo;
            short	NumRxSm, NumTxSm;
            TPdoEntryInfo[]	RxPEI;
            TPdoEntryInfo[]	TxPEI;

	        TInfo		Info;
	        TProfile	Profile;
	        bool	PhysAddrChk;	// Physical address duplication check
   	        bool	PhysAddrChange;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_DEV_NAME)]
            char[]  Type;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_DEV_NAME)]
	        char[] 	Name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CSR_FILE_NAME)]
	        char[]	URL;
	        IntPtr 	Icon;
	        uint	IconNo;
            TSlvAxisInfo SlvAxisInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 29)]
	        uint[]	uiResv;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TInfo
        {
            byte	NumCh;
	        TVendorSpecific DfltPdoAssin;
        }


        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TVendorSpecific
        {
            byte	NumSm;
	        byte	NumPdo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_SM)]
	        byte[]	DfltSmNo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_NUM_PDO_ENTRY)]
	        ushort[]	DfltPdoIdx;
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TProfile
        {
            ushort	ProfileNo;
	        ushort	AddInfo;
	        ushort	ChannelCount;	// <ChannelInfo> 속성이 있으면 수가 채널 수 이다.
        }

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TPdoEntryInfo
        {
            ushort	Index;		// M
	        ushort	SmNo;
            ushort	NumEtry;
	        ushort	NumCh;
	        ushort	BitLen;
	        ushort	ChStartBit;
	        ushort	ChBitLen;
            short	DataType;
	        short	DScale;
        }


        [Serializable]
        public class SlotInfos
        {
            public bool isReadInfo = false;
            public int SlotPdoIncrement;
            public bool isSlotPdoIncrementExist = false;
            public int SlotIndexIncrement;
            public bool IsSlotIndexIncrementExist;
            //public int XmlSlotCount;
            public List<SlotInfo> slotList = new List<SlotInfo>();
            public bool isMoudleClassExist;
        }


        [Serializable]
        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class SlotInfo
        {
            public int minInstances;
            public int maxInstances;
            public string name;
            public List<int> moduleIdent = new List<int>();
            public int defaultVal;
            public int moduleCount;
            public List<string> moduleClass = new List<string>();
            //public bool isMoudleClassExist;
            //public ModuleInfo[] moduleInfo;
        }

        [Serializable]
        public class ModuleInfos
        {
            public int moduleCount = 0;
            public List<ModuleInfo> moduleList;

            public ModuleInfos()
            {
                moduleList = new System.Collections.Generic.List<ModuleInfo>();
            }
        }

        [Serializable]
        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class ModuleInfo
        {
            public string type;
            public int moduleIdent;
            public string name;
            public TPdo pdoList;
            public string moduleClass;
        }

        public const int ecMAX_NUM_LMEM_SECTIONS = 16;	// Maximum number of logical memory sections
        public const int ecMAX_LMEM_SECT_NAME_LEN = 63;	// Maximum string length of a logical memory section

        // 여기서 정의하는 논리메모리의 Base주소는 내부적으로만 사용되는 것이며 사용자가 사용할 때는 
        // InputPDO 영역과 OutputPDO 영역 모두 주소가 0부터 시작한다.
        public const int ecINPDO_BASE_LOGIC_ADDR = 10000;
        public const int ecOUTPDO_BASE_LOGIC_ADDR = 0;

        public const int ecSLAVE_ID_MASTER = 0xffff;   // Master 스스로가 처리해야 하는 Command일 때 SlaveIdx 값 대신에 셋팅해야 하는 ID값 

        public const int ecMAX_NUM_SMMU = 8;
        public const int ecMAX_NUM_FMMU = 8;
        public const int ecMAX_NUM_PDO_UNIT = 4; // OutPDO / InPDO 각각으로 할당되는 SyncManager의 최대갯수 (In / Out 각각에 할당될 수 있는 최대 갯 수임)
        public const int ecMAX_NUM_PDO_ASSIGN = 8;
        public const int NUM_DEV_TYPE = 13;
        public const int CSR_MAX_PATH = 260;
        public const int CSR_FILE_NAME	= 60;
        public const int CSR_USR_NAME	= 100;
        public const int CSR_DEV_NAME	= 100;
        public const int CSR_DEV_ELEM	= 40;
        public const int MAX_NUM_DSCALE = 19;

        public const int MAX_NUM_SM		= 4;
        public const int MAX_NUM_PDO_ENTRY = 16;
        public const int ecPDO_ASSGN_SECT_SIZE = 136;

        public enum EEcEscdTypeId
        {
	        ESCD_TYPE_OutPdoMap,  	// OutPDO Mapping Data
	        ESCD_TYPE_InPdoMap,		// InPDO Mapping Data
	        ESCD_TYPE_AxisInfo		// Axis Information
        };

        [Serializable]
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TEcDCMeasInfoOfSlv
        {
            public ushort PhysAddr; // Slave Address
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public int[] PortDelay; // delay of each ESC port to next slave port 
	        public int DelayToSlave; // delay time of the slave
        }
    }
}
