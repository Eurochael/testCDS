using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LOG
{
    class IniFileClass
    {
        public string gstrErrMsg;
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern int GetPrivateProfileString
        (
            string IpApplicationName, 
            string IpKeyName, 
            string IpDefault, 
            StringBuilder IpReturnedString, 
            int nSize, 
            string IpFileName
        ); 
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern int WritePrivateProfileString
        (
            string lpApplicationName,
            string IpKeyName,
            string lpString,
            String lpFileName
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern int GetPrivateProfileInt
        (
            string lpApplicationName,
            string IpKeyName,
            int nDefault,
            String lpFileName
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        static extern int FlushPrivateProfileString
        (
            int lpApplicationName,
            int IpKeyName,
            int lpString,
            String lpFileName
        );

        private void Flush()
        {
            FlushPrivateProfileString(0, 0, 0, str_FileName);
        }

        private string str_FileName;
        
        public IniFileClass(string Filename)
        {
            str_FileName = Filename;
        }

        public string FileName
        {
            get{
                return str_FileName;
            }
        }
      
        public string GetString(string str_Section, string str_Key, string str_Default)
        {
            gstrErrMsg = "";
            try
            {
                int int_CharCount;
                StringBuilder strbulid_objresult = new StringBuilder(256);
                int_CharCount = GetPrivateProfileString(str_Section, str_Key, str_Default, strbulid_objresult, strbulid_objresult.Capacity, str_FileName);
                if(int_CharCount > 0)
                {
                    return Left(Convert.ToString(strbulid_objresult), int_CharCount);
                }
                else
                {
                    return "";
                }
            }
            catch(Exception ex)
            {
                gstrErrMsg = "IniFileClass.GetString.ErrMsg:" + ex.Message;
                return "";
            }
        }

        public int GetInteger(string str_Section, string str_Key, int int_Default)
        {
            return GetPrivateProfileInt(str_Section, str_Key, int_Default, str_FileName);
        }

        public bool GetBoolean(string str_Section, string str_Key, bool bol_Default)
        {
            return (GetPrivateProfileInt(str_Section, str_Key, Convert.ToInt32(bol_Default), str_FileName) == 1);
        }

        public void WriteString(string str_Section, string str_Key,string str_Value)
        {
            WritePrivateProfileString(str_Section, str_Key, str_Value, str_FileName);
            Flush();
        }

        public void WriteInteger(string str_Section, string str_Key, int int_Value)
        {
            WriteString(str_Section, str_Key, Convert.ToString(int_Value));
            Flush();
        }

        public void WriteBoolean(string str_Section, string str_Key, bool bol_Value)
        {
            WriteString(str_Section, str_Key, Convert.ToString(Convert.ToInt32(bol_Value)));
        }

        //공용 함수
        public string Left(string str_Text, int int_TextLength)
        {
            string str_ConvertText;
            if (str_Text.Length < int_TextLength)
            {
                int_TextLength = str_Text.Length;
            }
            str_ConvertText = str_Text.Substring(0, int_TextLength);
            return str_ConvertText;
        }

        public string Right(string str_Text, int int_TextLength)
        {
            string str_ConvertText;
            if (str_Text.Length < int_TextLength)
            {
                int_TextLength = str_Text.Length;
            }
            str_ConvertText = str_Text.Substring(str_Text.Length - int_TextLength, int_TextLength);
            return str_ConvertText;
        }

        public string Mid(string str_Text, int int_Start, int int_End)
        {
            string str_ConvertText;
            if (int_Start < str_Text.Length || int_End < str_Text.Length)
            {
                str_ConvertText = str_Text.Substring(int_Start, int_End);
                return str_ConvertText;
            }
            else
            {
                return str_Text;
            }
            
        }

    }

}
