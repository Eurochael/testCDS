using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DB_MARIA
{
    public class MainModule
    {
        public string GetDataSet (string ConnectionString, string str_Query, DataSet dataset_ReturenDataSet, string str_SrcTable = "GetDataSet")
        {
            OdbcConnection SQL_Connection = new OdbcConnection(ConnectionString);
            OdbcDataAdapter SQL_DataAdapter = new OdbcDataAdapter(str_Query, ConnectionString);
            try
            {
                SQL_DataAdapter.SelectCommand.CommandTimeout = 5;
                SQL_DataAdapter.Fill(dataset_ReturenDataSet, str_SrcTable);
                return "";
            }
            catch(Exception ex)
            {
                return "SQLlib.ModuleMain.GetDataSet=" + ex.Message + str_Query;
            }
            finally
            {
                if (SQL_DataAdapter != null){SQL_DataAdapter.Dispose();}
                if (SQL_Connection != null){SQL_Connection.Dispose();}
            }
        }
        public string ExecuteQuery(string ConnectionString, string str_Query, ref int int_Result)
        {
            OdbcConnection SQL_Connection = new OdbcConnection(ConnectionString);
            OdbcCommand SQL_Command = new OdbcCommand(str_Query, SQL_Connection);
            try
            {
                SQL_Command.Connection.Open();
                int_Result = SQL_Command.ExecuteNonQuery();
                return "";
            }
            catch (Exception ex)
            {
                return "SQLlib.ModuleMain.ExecuteQuery=" + ex.Message + str_Query;
            }
            finally
            {
                if (SQL_Command != null){SQL_Command.Dispose();}
                if (SQL_Connection != null){SQL_Connection.Dispose();}
            }
        }
        public string ExecuteQuery(string ConnectionString, string str_Query, string str_Key, Byte[] byte_data,ref int int_Result)
        {
            OdbcParameter SQL_Parameter = new OdbcParameter();
            OdbcConnection SQL_Connection = new OdbcConnection(ConnectionString);
            OdbcCommand SQL_Command = new OdbcCommand(str_Query, SQL_Connection);
            try
            {
                SQL_Command.Connection.Open();
                SQL_Parameter.OdbcType = OdbcType.VarBinary;
                //SQL_Parameter.DbType = DbType.Binary;
                SQL_Parameter.ParameterName = str_Key;
                SQL_Parameter.Value = byte_data;
                SQL_Parameter.Size = byte_data.Length;
                SQL_Command.Parameters.Add(SQL_Parameter);
                //SQL_Command.Parameters.Add(str_Key, OdbcType.VarBinary).Value = byte_data;
                int_Result = SQL_Command.ExecuteNonQuery();
                return int_Result.ToString();
            }
            catch (Exception ex)
            {
                return "SQLlib.ModuleMain.ExecuteQuery=" + ex.Message + str_Query;
            }
            finally
            {
                if (SQL_Command != null) { SQL_Command.Dispose(); }
                if (SQL_Connection != null) { SQL_Connection.Dispose(); }
            }
        }
        public string ExecuteQuery_File_Upload(string ConnectionString, string filename, int unit_id, int file_id, ref int int_Result)
        {
            OdbcParameter SQL_Parameter = new OdbcParameter();
            OdbcConnection SQL_Connection = new OdbcConnection(ConnectionString);
            OdbcCommand SQL_Command = new OdbcCommand();
            FileStream fs = null;
            int FileSize;
            byte[] rawData;
            string query = "";
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                FileSize = (int)fs.Length;
                rawData = new byte[FileSize];
                fs.Read(rawData, 0, FileSize);
                fs.Close();

                query = "INSERT INTO configurations(unit_id, file_id, configuration_data)";
                query += " VALUES";
                query += "(";
                query += "'" + unit_id + "'";
                query += ",'" + file_id + "'";
                query += "," + "?" + "";
                query += ")";
                SQL_Command.Connection = SQL_Connection;
                SQL_Command.Connection.Open();
                //SQL_Command.CommandText = query;
                //SQL_Command.Parameters.AddWithValue("@File", rawData);
                SQL_Command.CommandText = query;
                SQL_Parameter.OdbcType = OdbcType.VarBinary;
                SQL_Parameter.ParameterName = "@File";
                SQL_Parameter.Value = rawData;
                SQL_Parameter.Size = FileSize;
                SQL_Command.Parameters.Add(SQL_Parameter);
                int_Result = SQL_Command.ExecuteNonQuery();
                return int_Result.ToString();
            }
            catch (Exception ex)
            {
                return "SQLlib.ModuleMain.ExecuteQuery=" + ex.Message;
            }
            finally
            {
                if (fs != null) { fs.Dispose(); }
                if (SQL_Command != null) { SQL_Command.Dispose(); }
                if (SQL_Connection != null) { SQL_Connection.Dispose(); }
            }
        }
        public string ExecuteQuery_File_Download(string ConnectionString, string filename, int unit_id, int file_id, ref string Result, ref string error)
        {
            OdbcParameter SQL_Parameter = new OdbcParameter();
            OdbcConnection SQL_Connection = new OdbcConnection(ConnectionString);
            OdbcCommand SQL_Command = new OdbcCommand();

            FileStream fs = null;
            int FileSize = 0;
            byte[] rawData = null;
            string query = "";
            string log = "";
            try
            {
               
                query = "SELECT *,OCTET_LENGTH(configuration_data) AS 'Length' FROM configurations WHERE unit_id = '" + unit_id + "'" + " AND " + "file_id='" + file_id + "'";
                query += "";
                   
                SQL_Command.Connection = SQL_Connection;
                SQL_Command.Connection.Open();
                SQL_Command.CommandText = query;
                OdbcDataReader SQL_DATA_READER = SQL_Command.ExecuteReader();
                if (SQL_DATA_READER.HasRows)
                {
                    SQL_DATA_READER.Read();

                    FileSize = SQL_DATA_READER.GetInt32(SQL_DATA_READER.GetOrdinal("Length"));
                    rawData = new byte[FileSize];
                    SQL_DATA_READER.GetBytes(SQL_DATA_READER.GetOrdinal("configuration_data"), 0, rawData, 0, (int)FileSize);

                    try
                    {
                        if (System.IO.File.Exists(filename) == true)
                        {
                            System.IO.File.Delete(filename);
                            log = log + ",Delete Sucess" + filename + "";
                        }
                    }
                    catch(Exception ex)
                    {
                        log = log + ",Delete Fail" + filename + "";
                        error = ex.ToString();
                    }
                    finally
                    {

                    }

                    fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(rawData, 0, (int)FileSize);
                    fs.Close();
                    log = log + ",Download Stream Sucess" + filename + "";
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                error = ex.ToString();
                return "SQLlib.ModuleMain.ExecuteQuery=" + ex.Message;
            }
            finally
            {
                if (fs != null) { fs.Dispose(); }
                if (SQL_Command != null) { SQL_Command.Dispose(); }
                if (SQL_Connection != null) { SQL_Connection.Dispose(); }
            }
        }
        /*
        public int MainQuery(string str_Query)
        {
            int int_Query = 0;
            try
            {
                str_Error = ExecuteQuery(str_Query, int_Query);
                return int_Query;
            }
            catch{return int_Query;}
        }

        public bool MainDataSet(string str_Query, DataSet dataset_GedDataSet, string str_DataSetName = "DataSET")
        {
            try
            {
                if (str_Query == "")
                {
                    return false;
                }

                str_Error = GetDataSet(str_Query, dataset_GedDataSet, str_DataSetName);

                if (str_Error == ""){return true;}
                else{return false;}
            }

            catch{return false;}
        }
        */


    }
}
