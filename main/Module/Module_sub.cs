using System;
using System.Data;
using static cds.Config_IO;

namespace cds
{
    class Module_sub
    {
        public enum Config_type
        {
            cg_main = 1,
            cg_app_info = 2,
            cg_sync = 3,
            cg_trend = 4,
            cg_socket = 5,
            cg_trace = 6,
            cg_mixing_step = 7,
            cg_di = 10,
            cg_do = 11,
            cg_ai = 12,
            cg_ao = 13,
            cg_serial = 14,
            cg_unit_io = 20,
            cg_parameter = 21,
            cg_mixing_step_custom1 = 30,
        }
        public string Config_Class_To_Yaml(Config_type config_type, string filename)
        {
            // 수정 시 Config_Yaml_To_Class Config_Class_To_Yaml 두개의 Function 동시 수정 확인
            string result = "";
            try
            {
                if (config_type == Config_type.cg_main)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_main.yaml", Program.cg_main); }
                }
                else if (config_type == Config_type.cg_app_info)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_app_info.yaml", Program.cg_app_info); }
                }
                else if (config_type == Config_type.cg_trend)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_trend.yaml", Program.cg_trend_datalist); }
                }
                else if (config_type == Config_type.cg_socket)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_socket.yaml", Program.cg_socket); }
                }
                else if (config_type == Config_type.cg_trace)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_trace.yaml", Program.cg_trace); }
                }
                else if (config_type == Config_type.cg_mixing_step)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_mixing_step.yaml", Program.cg_mixing_step); }
                    }
                    else
                    {
                        if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_mixing_step.yaml", Program.cg_mixing_step); }
                    }
                }
                else if (config_type == Config_type.cg_di)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_di.yaml", Program.IO.DI); }
                }
                else if (config_type == Config_type.cg_do)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_do.yaml", Program.IO.DO); }
                }
                else if (config_type == Config_type.cg_ai)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_ai.yaml", Program.IO.AI); }
                }
                else if (config_type == Config_type.cg_ao)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_ao.yaml", Program.IO.AO); }
                }
                else if (config_type == Config_type.cg_serial)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_serial.yaml", Program.IO.SERIAL); }
                }
                else if (config_type == Config_type.cg_unit_io)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_unit_io.yaml", Program.cg_unit_no); }
                }
                else if (config_type == Config_type.cg_mixing_step_custom1)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml + Program.cg_main.path.mixing_step_save_folder_name, filename, Program.cg_mixing_step); }
                }
                else if (config_type == Config_type.cg_parameter)
                {
                    if (result == "") { result = Program.yaml_md.Serialize(Program.cg_main.path.yaml, "config_offset.yaml", Program.cg_offset); }
                }
                else
                {
                    //if (result == "") { result = Program.yaml_md.Serialize("config_main.yaml", Program.cg_main); }
                    //if (result == "") { result = Program.yaml_md.Serialize("config_sync.yaml", Program.cg_sync); }
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("Module_main.Config_Class_To_Yaml." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            return result;
        }
        public string Config_Yaml_To_Class(Config_type config_type, string filename)
        {
            //filename이 있을 경우 해당 파일로 참조. Mixing Type 기능 추가에 따라 함수 변경
            // 수정 시 Config_Yaml_To_Class Config_Class_To_Yaml 두개의 Function 동시 수정 확인
            string result = "";
            try
            {
                if (config_type == Config_type.cg_main)
                {
                    if (result == "") { Program.cg_main = Program.yaml_md.DeSerialize<Config_Main>(Program.cg_main.path.yaml, "config_main.yaml"); }
                }
                else if (config_type == Config_type.cg_app_info)
                {
                    if (result == "") { Program.cg_app_info = Program.yaml_md.DeSerialize<Config_App_Info>(Program.cg_main.path.yaml, "config_app_info.yaml"); }
                }
                else if (config_type == Config_type.cg_trend)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.cg_trend_datalist = Program.yaml_md.DeSerialize<Config_Trend_DataList>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_trend.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.cg_trend_datalist = Program.yaml_md.DeSerialize<Config_Trend_DataList>(Program.cg_main.path.yaml, "config_trend.yaml"); }
                    }
                }
                else if (config_type == Config_type.cg_socket)
                {
                    if (result == "") { Program.cg_socket = Program.yaml_md.DeSerialize<Config_Socket>(Program.cg_main.path.yaml, "config_socket.yaml"); }
                }
                else if (config_type == Config_type.cg_trace)
                {
                    if (result == "") { Program.cg_trace = Program.yaml_md.DeSerialize<Config_Trace>(Program.cg_main.path.yaml, "config_trace.yaml"); }
                }
                else if (config_type == Config_type.cg_mixing_step)
                {
                    if(Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.cg_mixing_step = Program.yaml_md.DeSerialize<Config_Mixing_Step>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_mixing_step.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.cg_mixing_step = Program.yaml_md.DeSerialize<Config_Mixing_Step>(Program.cg_main.path.yaml, "config_mixing_step.yaml"); }
                    }
                }
                else if (config_type == Config_type.cg_di)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.IO.DI = Program.yaml_md.DeSerialize<Config_IO.Config_DI>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_di.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.IO.DI = Program.yaml_md.DeSerialize<Config_IO.Config_DI>(Program.cg_main.path.yaml, "config_di.yaml"); }
                    }
                }
                else if (config_type == Config_type.cg_do)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.IO.DO = Program.yaml_md.DeSerialize<Config_IO.Config_DO>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_do.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.IO.DO = Program.yaml_md.DeSerialize<Config_IO.Config_DO>(Program.cg_main.path.yaml, "config_do.yaml"); }

                    }
                }
                else if (config_type == Config_type.cg_ai)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.IO.AI = Program.yaml_md.DeSerialize<Config_IO.Config_AI>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_ai.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.IO.AI = Program.yaml_md.DeSerialize<Config_IO.Config_AI>(Program.cg_main.path.yaml, "config_ai.yaml"); }
                    }
                   
                }
                else if (config_type == Config_type.cg_ao)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.IO.AO = Program.yaml_md.DeSerialize<Config_IO.Config_AO>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_ao.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.IO.AO = Program.yaml_md.DeSerialize<Config_IO.Config_AO>(Program.cg_main.path.yaml, "config_ao.yaml"); }
                    }
                }
                else if (config_type == Config_type.cg_serial)
                {
                    if (Program.cg_app_info.mode_simulation.use == true)
                    {
                        if (result == "") { Program.IO.SERIAL = Program.yaml_md.DeSerialize<Config_IO.Config_SERIAL>(Program.cg_main.path.yaml + @"\" + Program.cg_app_info.eq_type.ToString().ToUpper() + @"\", "config_serial.yaml"); }
                    }
                    else
                    {
                        if (result == "") { Program.IO.SERIAL = Program.yaml_md.DeSerialize<Config_IO.Config_SERIAL>(Program.cg_main.path.yaml, "config_serial.yaml"); }
                    }
                }
                else if (config_type == Config_type.cg_unit_io)
                {
                    if (result == "") { Program.cg_unit_no = Program.yaml_md.DeSerialize<Config_Unit_Io>(Program.cg_main.path.yaml, "config_unit_io.yaml"); }
                }
                else if (config_type == Config_type.cg_mixing_step_custom1)
                {
                    if (result == "") { Program.cg_mixing_step = Program.yaml_md.DeSerialize<Config_Mixing_Step>(Program.cg_main.path.yaml + Program.cg_main.path.mixing_step_save_folder_name, filename); }
                }
                else if (config_type == Config_type.cg_parameter)
                {
                    if (result == "") { Program.cg_offset = Program.yaml_md.DeSerialize<Config_Offset_Parameter>(Program.cg_main.path.yaml, "config_offset.yaml"); }
                }
                else
                {
                    //if (result == "") { Program.cg_main = Program.yaml_md.DeSerialize<Config_Main>("config_main.yaml"); }
                    //if (result == "") { Program.cg_sync = Program.yaml_md.DeSerialize<Config_Sync>("config_sync.yaml"); }
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("Module_main.Config_Yaml_To_Class. File:" + config_type.ToString() + " : " + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            return result;
        }
        public string Config_Yaml_To_DB(Config_type config_type)
        {
            string result = "", err = "", query = "";
            Byte[] binary_ymal;
            DataSet dataset = new DataSet();
            try
            {
                if (config_type == Config_type.cg_sync)
                {
                    binary_ymal = Program.yaml_md.DeSerialize(Program.cg_main.path.yaml, "config_sync.yaml");

                    query = "SELECT unit_id FROM configurations WHERE unit_id = " + Program.cg_socket.ctc_network_no + "";
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_server.connection, query, dataset, ref err);

                    if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                    {
                        query = "UPDATE configurations SET configuration_data = ? WHERE unit_id = " + Program.cg_socket.ctc_network_no + "";
                    }
                    else
                    {
                        query = "INSERT INTO configurations(unit_id, configuration_data) VALUES (" + Program.cg_socket.ctc_network_no + ", ?);";
                    }
                    Program.database_md.MariaDB_MainQuery(Program.cg_main.db_server.connection, query, "@bytedata", binary_ymal, ref err);
                    if (err == "1" || err == "0")
                    {
                        //동일 데이타 업데이트 시 return 0
                        //Insert 또는 변동 데이터 업데이트 시 return 1
                        result = "";
                    }
                    else
                    {
                        result = err;
                        Program.log_md.LogWrite("Module_main.Config_Yaml_To_DB." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("Module_main.Config_Yaml_To_DB." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }
        public string Config_DB_To_Class_To_Yaml(Config_type config_type)
        {
            string result = "", err = "", query = "";
            DataSet dataset = new DataSet();
            try
            {
                if (config_type == Config_type.cg_app_info)
                {
                    query = "SELECT unit_id, configuration_data FROM configurations WHERE unit_id = " + Program.cg_socket.ctc_network_no + "";
                    Program.database_md.MariaDB_MainDataSet(Program.cg_main.db_server.connection, query, dataset, ref err);

                    if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                    {
                        Byte[] value = (Byte[])dataset.Tables[0].Rows[0][1];
                        Program.cg_app_info = Program.yaml_md.DeSerialize<Config_App_Info>(value);
                    }
                    else
                    {
                        result = "ServerDatabase is not exist Unit ID : " + Program.cg_socket.ctc_network_no;
                        Program.log_md.LogWrite("Module_main.Config_DB_To_Class_To_Yaml." + result, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
                    }

                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
                Program.log_md.LogWrite("Module_main.Config_Yaml_To_DB." + ex.Message, Module_Log.enumLog.Error, "", Module_Log.enumLevel.ALWAYS);
            }
            finally { if (dataset != null) { dataset.Clear(); dataset.Dispose(); dataset = null; } }
            return result;
        }

    }
}
