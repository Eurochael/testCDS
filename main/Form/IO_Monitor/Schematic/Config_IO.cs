using System;

namespace cds
{
    public class Config_IO
    {
        public enum enum_io_type
        {
            DI = 0, DO = 10, AI = 20, AO = 30, SERIAL = 40
        }
        public enum enum_unit
        {
            NONE = 0,
            THERMOSTAT_HE_3320C = 10,
            FLOWMETER_USF500 = 30, PUMP_CONTROLLER_PB12 = 40,
            CONCENTRATION_CM210A_DC = 60, CONCENTRATION_CS600F = 61, CONCENTRATION_HF_700 = 62, CONCENTRATION_CS_150C = 63,
            TEMP_CONTROLLER_M74 = 70,TEMP_CONTROLLER_M74R = 71,
            SCR_DPU31A_025A = 80, FLOWMETER_SONOTEC = 90
        }
        //Index Map
        //--databit 0:7bit / 1:8bit
        //--Stopbit 0:1bit / 1:2bit
        //--paritybit 0:none / 1:odd / 2:even
        //--baudrate 0:2400 / 1:4800 / 2:9600 / 3:14400 / 4:19200 / 5:38400 / 6:57600 / 7:115200
        //serialtype 0:232/422 / 1:485

        public enum enum_aes_cbc_serial_databit
        {
            bit7 = 0, bit8 = 1
        }
        public enum enum_aes_cbc_serial_stopbit
        {
            bit1 = 0, bit2 = 1
        }
        public enum enum_aes_cbc_serial_parity
        {
            none = 0, odd = 1, even = 2
        }
        public enum enum_aes_cbc_serial_baudrate
        {
            _2400 = 0, _4800 = 1, _9600 = 2, _14400 = 3, _19200 = 4, _38400 = 5, _57600 = 6, _115200 = 7
        }
        public enum enum_aes_cbc_serial_type
        {
            rs232_422 = 0, rs485 = 1
        }
        public enum enum_aes_cbc_analog_range
        {
            _n10_24_to_p10_24_v = 0,
            _n5_12_to_p5_12_v = 1,
            _n2_56_to_p2_56_v = 2,
            _0_to_10_24_v = 3,
            _0_to_5_12_v = 4,
            _4_to_20_ma = 5,
            _0_to_20_ma = 6,
            _0_to_24_ma = 7,
        }

        public Config_DI DI = new Config_DI();
        public Config_DO DO = new Config_DO();
        public Config_AI AI = new Config_AI();
        public Config_AO AO = new Config_AO();
        public Config_SERIAL SERIAL = new Config_SERIAL();

        //public Digial_Value[] DI = new Digial_Value[80]; //DHF : AES-CBC(48) //APM : AES-CBC(48) //DSP : AES-CBC(48)
        //public Digial_Value[] DO = new Digial_Value[80]; //DHF : AES-CBC-DO(16) + Solblock1(32) + Solblock2(16) //APM : AES-CBC-DO(16) + ETS-DO16N-E(16) + Solblock1(32) + Solblock2(16) //DSP : AES-CBC-DO(16) + Solblock1(32) + Solblock2(16)
        //public Analog_Value[] AI = new Analog_Value[50]; //DHF : AES-CBC-AI(8) + ETS-Ai16AH-E1(16) + ETS-Ai16AH-E2(16) //APM : AES-CBC-AI(8) + ETS-Ai16AH-E2(16) //DSP : AES-CBC-AI(8) + ETS-Ai16AH-E1(16) + ETS-Ai16AH-E2(8)
        //public Analog_Value[] AO = new Analog_Value[10]; // DHF : AES-CBC-AO(4)
        //public Serial_Value[] serial = new Serial_Value[10]; // DHF : AES-CBC-Serial(8)

        public class Config_DI
        {
            public int use_cnt = 48;
            public Config_Digial_Tag[] Tag = new Config_Digial_Tag[80];
        }
        public class Config_DO
        {
            public int use_cnt = 100;
            public Config_Digial_Tag[] Tag = new Config_Digial_Tag[80];
        }
        public class Config_AI
        {
            public int use_cnt = 64;
            public Config_Analog_Tag[] Tag = new Config_Analog_Tag[80];
        }
        public class Config_AO
        {
            public int use_cnt = 4;
            public Config_Analog_Tag[] Tag = new Config_Analog_Tag[80];
        }
        public class Config_SERIAL
        {
            public int use_cnt = 8;
            public Config_Serial_Tag[] Tag = new Config_Serial_Tag[8];
        }
        public class Config_Digial_Tag
        {
            public int no;
            public int address;
            public Boolean use = false;
            public Boolean gui_display = false;
            public string name;
            public string description;
            public string etc;
            public string unit;
            public Boolean value;
            public Boolean value_raw;
            public DateTime dt_off;
            public DateTime dt_on;
        }

        public class Config_Analog_Tag
        {
            public int no;
            public int address;
            public Boolean use = false;
            public Boolean gui_display = false;
            public string type; //N.O OR N.C
            public string name;
            public string description;
            public string etc;
            public int value_raw;
            public double value;
            public double min;
            public double max;
            public int range_row;
            public int range_high;
            public double gain;
            public double offset;
            public string unit;
            public enum_aes_cbc_analog_range range = enum_aes_cbc_analog_range._4_to_20_ma;
        }
        public class Config_Serial_Tag
        {
            public int address;
            public Boolean use = false;
            public Boolean gui_display = false;
            public int comport;
            public string name;
            public string description;
            public string etc;
            public int ch_total_cnt; //USF500의 경우 2채널, Temp controller의 경우 최대 4채널
            public enum_unit unit = enum_unit.NONE;
            public enum_aes_cbc_serial_baudrate buadrate = enum_aes_cbc_serial_baudrate._9600;
            public enum_aes_cbc_serial_parity parity = enum_aes_cbc_serial_parity.even;
            public enum_aes_cbc_serial_databit databit = enum_aes_cbc_serial_databit.bit8;
            public enum_aes_cbc_serial_stopbit stopbit = enum_aes_cbc_serial_stopbit.bit1;
            public enum_aes_cbc_serial_type type = enum_aes_cbc_serial_type.rs232_422; //RS-232 OR RS-485
        }
        public enum enum_di
        {
            EMS = 0,
            E_RIGHT_DOOR = 1,
            ELEC_DOOR_OVERRIDE = 2,
            GPS_UPS_TRIP = 3,
            HDIW_POWER_ON = 4, //APM
            HDIW_REMOTE_MODE = 5, ///APM
			HDIW_TOTAL_ALARM = 6, ///APM
			MAIN_UNIT_MIX_TRIP = 7, //DHF
            LEFT_FAN_ALARM = 8,
            RIGHT_FAN_ALARM = 9,
            SUPPLY_A_HEATER_DRAIN_V_V = 10,
            SUPPLY_B_HEATER_DRAIN_V_V = 11,
            Drain_Pump_On_Level = 12,
            Interlock_trip = 13,
            EX_TOTAL_AL = 14,
            SPARE_15 = 15,
            CIRCULATION_PUMP_L_LEAK = 16, //20230-04-06 DSP Dino Pump Changed
            CIRCULATION_PUMP_R_LEAK = 17, //20230-04-06 DSP Dino Pump Changed
            CIRCULATION_PUMP_STROKE_1 = 18, //20230-04-06 DSP Dino Pump Changed
            CIRCULATION_PUMP_STROKE_2 = 19, //20230-04-06 DSP Dino Pump Changed
            TANKA_LEVEL_HH = 20,
            TANKA_OVERFLOW_CHECK = 21,
            TANKB_LEVEL_HH = 22,
            TANKB_OVERFLOW_CHECK = 23,
            BOTTOM_VAT_LEAK1 = 24,
            Drain_Tank_H = 25,
            TANK_VAT_LEAK = 26,
            DIKE_LEAK = 27,
            C_DOOR_ALARM = 28,
            EXH_LL = 29,
            HEAT_EXCHANGER_EMO = 30,
            MAIN_EQ_TRIP = 31,
            TANKA_LEVEL_H = 32,
            TANKA_LEVEL_M = 33,
            TANKA_LEVEL_L = 34,
            TANKA_LEVEL_LL = 35,
            TANKA_EMPTY_CHECK = 36,
            TANKB_LEVEL_H = 37,
            TANKB_LEVEL_M = 38,
            TANKB_LEVEL_L = 39,
            TANKB_LEVEL_LL = 40,
            TANKB_EMPTY_CHECK = 41,
            SPARE_42 = 42,
            RETURN_DRAIN_V_V = 43,
            SPARE_44 = 44,
            CIRCULATION_PUMP_LEFT_ON = 45,
            CIRCULATION_PUMP_RIGHT_ON = 46,
            CIRCULATION_PUMP_LEAK = 47,
            SUPPLY_A_PUMP_LEAK = 48,
            SUPPLY_B_PUMP_LEAK = 49,
            SPARE_50 = 50,
            Gas_Detec_Alarm = 51,
            Purge_Unit_Alarm = 52,
            LEAK_FAULT = 53,
            Purge_Unit_OK = 54,
            CM_DRAIN_PUMP_ON_LEVEL = 55,
            CM_LEAK = 56,
            CM_DIKE_LEAK = 57,
            CM_VAT_LEAK = 58,
            CM_DRAIN_TANK_H = 59,
            SUPPLY_A_PUMP_L_LEAK = 60, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_A_PUMP_R_LEAK = 61, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_A_PUMP_STROKE_1 = 62, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_A_PUMP_STROKE_2 = 63, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_B_PUMP_L_LEAK = 64, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_B_PUMP_R_LEAK = 65, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_B_PUMP_STROKE_1 = 66, //20230-04-06 DSP Dino Pump Changed
            SUPPLY_B_PUMP_STROKE_2 = 67, //20230-04-06 DSP Dino Pump Changed
            SPARE_68 = 68,
            IPA_CCSS_Ready_Signal = 69, //2023-06-07 AK 연구소 IPA 관련 DI 추가
        }
        public enum enum_do
        {
            BUZZER = 0,
            TOWER_LAMP_RED = 1,
            TOWER_LAMP_YEL = 2,
            TOWER_LAMP_GRN = 3,
            TOWER_LAMP_BLU = 4,
            SUPPLY_A_HEATER_PWR_ON = 5,
            SUPPLY_B_HEATER_PWR_ON = 6,
            SUPPLY_A_THERMOSTAT_PWR_ON = 7,
            SUPPLY_B_THERMOSTAT_PWR_ON = 8,
            CIRCULATION1_HEATER_PWR_ON = 9,
            CIRCULATION2_HEATER_PWR_ON = 10,
            CIRCULATION_THERMOSTAT_PWR_ON = 11,
            CIRCULATION_PUMP_LEFT_ON = 12,
            CIRCULATION_PUMP_RIGHT_ON = 13,
            Drain_Pump_On = 14,
            Drain_Tank_V_V_On = 15,
            Vet_V_V_On = 16,
            CCSS1 = 17,
            CCSS2 = 18,
            CCSS3 = 19,
            SCR1_RUN = 20,
            SCR2_RUN = 21,
            SCR3_RUN = 22,
            SCR4_RUN = 23,
            HDIW_REMOTE_START = 24,
            SPARE_25 = 25,
            SPARE_26 = 26,
            SPARE_27 = 27,
            SPARE_28 = 28,
            SPARE_20 = 29,
            SPARE_30 = 30,
            DIW_SUPPLY_TANK_A = 31,
            DIW_SUPPLY_TANK_B = 32,
            IPA_SUPPLY_TANK = 33,
            H2O2_SUPPLY_TANK_A = 34,
            H2O2_SUPPLY_TANK_B = 35,
            HF_SUPPLY_TANK_A = 36,
            HF_SUPPLY_TANK_B = 37,
            LAL_SUPPLY_TANK_A = 38,
            LAL_SUPPLY_TANK_B = 39,
            NH4OH_SUPPLY_TANK_A = 40,
            NH4OH_SUPPLY_TANK_B = 41,
            H2SO4_SUPPLY_TANK_A = 42,
            H2SO4_SUPPLY_TANK_B = 43,
            SPARE_44 = 44,
            SPARE_45 = 45,
            DSP_SUPPLY_TANK_A = 46,
            DSP_SUPPLY_TANK_B = 47,
            SPARE_48 = 48,
            SPARE_49 = 49,
            SUPPLY_FROM_TANK_A = 50,
            SUPPLY_FROM_TANK_B = 51,
            SUPPLY_TO_MAIN_A = 52,
            SUPPLY_TO_MAIN_B = 53,
            CIR_FROM_TANK_A = 54,
            CIR_FROM_TANK_B = 55,
            CIR_TO_TANK_A = 56,
            CIR_TO_TANK_B = 57,
            RETURN_TO_TANK_A = 58,
            RETURN_TO_TANK_B = 59,
            RETURN_SAMPLE_TO_TANK_A = 60,
            RETURN_SAMPLE_TO_TANK_B = 61,
            MAIN_RETURN_DRAIN = 62,
            MAIN_RETURN_SAMPLE_1 = 63,
            MAIN_RETURN_SAMPLE_2 = 64,
            CIR_DRAIN = 65,
            TANK_A_DRAIN = 66,
            TANK_B_DRAIN = 67,
            SAMPLING = 68,
            RECLAIM_DRAIN = 69,
            RETURN_RECLAIM_TO_TANK_A = 70,
            RETURN_RECLAIM_TO_TANK_B = 71,
            SPARE_72 = 72,
            SPARE_73 = 73,
            SPARE_74 = 74,
            SPARE_75 = 75,
            SPARE_76 = 76,
            SPARE_77 = 77,
            PCW_HEAT_CONTROLLER_A = 78, //DSP
            PCW_HEAT_CONTROLLER_B = 79, //DSP
            PCW_HEAT_CONTROLLER_CIR = 80, //DSP
            CIR_HEAT_CONTROLLER_DRAIN = 81, //DSP
            CM_FLUSHING_DIW = 82, //LAL
            HOT_DIW_BY_PASS = 83,
            SPARE_84 = 84,
            CIRCULATION_PUMP_START = 85,
            SUPPLY_PUMP_A_START = 86,
            SUPPLY_PUMP_B_START = 87,
            CM_Drain_Pump_On = 88,
            CM_Drain_Tank_V_V_On = 89,
            CM_VAT_V_V_On = 90,
            CM_FLW_V_V = 91,
            CIR_TO_HE_UNIT = 92,
            DIW_TO_CM = 93,
            CM_VAT_LEAK = 94,
            SPARE_95 = 95,
            SPARE_96 = 96,
            SPARE_97 = 97,
            SPARE_98 = 98,
            SPARE_99 = 99,


        }
        public enum enum_ai
        {
            EXHAUST = 0,
            SUPPLY_A_FILTER_IN_PRESS = 1,
            SUPPLY_A_FILTER_OUT_PRESS = 2,
            SUPPLY_B_FILTER_IN_PRESS = 3,
            SUPPLY_B_FILTER_OUT_PRESS = 4,
            SUPPLY_A_FLOW = 5,
            SUPPLY_B_FLOW = 6,
            CIRCULATION_FLOW = 7,
            CHEMICAL_RETURN_A = 8,
            CHEMICAL_RETURN_B = 9,
            SPARE_10 = 10,
            DRAIN_FLOW = 11, //APM
            DSP_SUPPLY_FLOW = 12, //DSP
            DIW_SUPPLY_FLOW = 13, //LAL
            SUPPLY_A_THERMOSTAT_PCW_FLOW = 14,
            SUPPLY_B_THERMOSTAT_PCW_FLOW = 15,
            CIRCULATION_THERMOSTAT_PCW_FLOW = 16,
            TANKA_PN2_FLOW = 17,
            TANKB_PN2_FLOW = 18,
            SPARE_19 = 19,
            SPARE_20 = 20,
            H2O2_SUPPLY_FILTER_IN_PRESS = 21, //APM
            H2O2_SUPPLY_FILTER_OUT_PRESS = 22, //APM
            HF_SUPPLY_FILTER_IN_PRESS = 23, //DHF
            HF_SUPPLY_FILTER_OUT_PRESS = 24, //DHF
            DSP_SUPPLY_FILTER_IN_PRESS = 25, //DSP
            DSP_SUPPLY_FILTER_OUT_PRESS = 26, //DSP
            LAL_SUPPLY_FILTER_IN_PRESS = 27, //LAL
            LAL_SUPPLY_FILTER_OUT_PRESS = 28, //LAL
            NH4OH_SUPPLY_FILTER_IN_PRESS = 29,//APM
            NH4OH_SUPPLY_FILTER_OUT_PRESS = 30, //APM
            RECLAIM_FILTER_IN_PRESS = 31, //LAL
            RECLAIM_FILTER_OUT_PRESS = 32, //LAL
            IPA_SUPPLY_FILTER_IN_PRESS = 33,
            IPA_SUPPLY_FILTER_OUT_PRESS = 34,
            H2SO4_SUPPLY_FILTER_IN_PRESS = 35,
            H2SO4_SUPPLY_FILTER_OUT_PRESS = 36,
            HEATER_N2_PRESS = 37,
            SPARE_38 = 38,
            SPARE_39 = 39,
            MAIN_PN2_PRESS = 40,
            MAIN_PCW_PRESS = 41,
            MAIN_CDA1_PRESS_PUMP = 42,
            MAIN_CDA2_PRESS_SOL = 43,
            MAIN_CDA3_PRESS_DRAIN = 44,
            CIRCULATION_PRESS = 45, //DHF
            DIW_SUPPLY_PRESS = 46,
            HOT_DIW_SUPPLY_PRESS = 47, //APM
            HDIW_FLOW_MONITORING = 48, //APM
            HDIW_TEMP_MONITORING = 49, //APM
            SPARE_50 = 50,
            CONCENTRATION_PRESS = 51,//DHF
            TANK_PN2_FLOW = 52,
            HEATER_PN2_FLOW = 53,
            CM_DIW_PRESS = 54,
            CM_SAMPLING_PRESS = 55,
            CM_PUMP_PRESS = 56,
            CM_SOL_PRESS = 57,
            CM_SAMPLING_FLOW = 58,
            CM_DIW_FLOW = 59,
            SPARE_60 = 60,
            SPARE_61 = 61,
            SPARE_62 = 62,
            SPARE_63 = 63,
            SPARE_64 = 64,
            SPARE_65 = 65,
            SPARE_66 = 66,
            SPARE_67 = 67,
            SPARE_68 = 68,
            SPARE_69 = 69,
            SPARE_70 = 70,
            SPARE_71 = 71,
            SPARE_72 = 72,
            SPARE_73 = 73,
            SPARE_74 = 74,
            SPARE_75 = 75,
            SPARE_76 = 76,
            SPARE_77 = 77,
            SPARE_78 = 78,
            SPARE_79 = 79,

        }
        public enum enum_ao
        {

        }
        public enum enum_dhf_serial_index
        {
            SUPPLY_A_THERMOSTAT = 0,
            SUPPLY_B_THERMOSTAT = 1,
            CIRCULATION_THERMOSTAT = 2,
            USF500_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            SUPPLY_B_PUMP_CONTROLLER = 5,
            CONCENTRATION = 6,
            TEMP_CONTROLLER_M74 = 7
        }
        public enum enum_apm_serial_index
        {
            SCR = 0,
            TEMP_CONTROLLER_M74R = 1,

            USF500_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            SUPPLY_B_PUMP_CONTROLLER = 5,

            TEMP_CONTROLLER_M74 = 7
        }
        public enum enum_ipa_serial_index
        {
            SCR = 0,
            //TEMP_CONTROLLER_M74R = 1,
            SONOTEC_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            TEMP_CONTROLLER_M74 = 7
        }
        public enum enum_dsp_serial_index
        {
            SUPPLY_A_THERMOSTAT = 0,
            SUPPLY_B_THERMOSTAT = 1,
            CIRCULATION_THERMOSTAT = 2,
            SONOTEC_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            SUPPLY_B_PUMP_CONTROLLER = 5,
            CONCENTRATION = 6,
            TEMP_CONTROLLER_M74 = 7
        }
        public enum enum_lal_serial_index
        {
            SUPPLY_A_THERMOSTAT = 0,
            SUPPLY_B_THERMOSTAT = 1,
            CIRCULATION_THERMOSTAT = 2,
            SONOTEC_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            SUPPLY_B_PUMP_CONTROLLER = 5,
            CONCENTRATION = 6,
            TEMP_CONTROLLER_M74 = 7
        }
        public enum enum_dsp_mix_serial_index
        {
            SUPPLY_A_THERMOSTAT = 0,
            SUPPLY_B_THERMOSTAT = 1,
            CIRCULATION_THERMOSTAT = 2,
            USF500_FLOWMETER = 3,
            SUPPLY_A_PUMP_CONTROLLER = 4,
            SUPPLY_B_PUMP_CONTROLLER = 5,

            TEMP_CONTROLLER_M74 = 7,
            CM_HF_700 = 8,
            CM_CS_150C = 9,
        }
    }

}
