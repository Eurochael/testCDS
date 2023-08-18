using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cds
{
    class Module_DB
    {
        public int MariaDB_MainQuery(string connection, string str_Query, ref string str_Error)
        {
            int int_Query = 0;
            try
            {
                str_Error = Program.DB_MARIA.ExecuteQuery(connection, str_Query, ref int_Query);
                return int_Query;
            }
            catch { return int_Query; }
        }
        public int MariaDB_MainQuery(string connection, string str_Query, string str_Key, Byte[] byte_Data, ref string str_Error)
        {
            int int_Query = 0;
            try
            {
                str_Error = Program.DB_MARIA.ExecuteQuery(connection, str_Query, str_Key, byte_Data, ref int_Query);
                return int_Query;
            }
            catch { return int_Query; }
        }
        public int MariaDB_MainQuery_File_Upload(string ConnectionString, string filename, int unit_id, int file_id, ref string str_Error)
        {
            int int_Query = 0;
            try
            {
                str_Error = Program.DB_MARIA.ExecuteQuery_File_Upload(ConnectionString, filename, unit_id, file_id, ref int_Query);
                return int_Query;
            }
            catch { return int_Query; }
        }
        public string MariaDB_MainQuery_File_Download(string ConnectionString, string filename, int unit_id, int file_id, ref string result, ref string error)
        {
            try
            {
                result = Program.DB_MARIA.ExecuteQuery_File_Download(ConnectionString, filename, unit_id, file_id, ref result, ref error);
                return result;
            }
            catch { return result; }
        }
        public bool MariaDB_MainDataSet(string connection, string str_Query, DataSet dataset_GedDataSet, ref string str_Error, string str_DataSetName = "DataSET")
        {
            try
            {
                if (str_Query == "") { return false; }
                str_Error = Program.DB_MARIA.GetDataSet(connection, str_Query, dataset_GedDataSet, str_DataSetName);
                if (str_Error == "") { return true; }
                else { return false; }
            }

            catch { return false; }

        }
    }
}
