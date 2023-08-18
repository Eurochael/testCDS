using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG
{
    class IniFileModule
    {
        public string gstrErrMsg;
        private IniFileClass cls_IniFileRead;
        private IniFileClass cls_IniFileSave;

        public string IniFileRead(string str_Section, string str_Key, string str_Default, string str_FileName = "")
        {
            gstrErrMsg = "";
            string str_Result;
            try
            {
                cls_IniFileRead = new IniFileClass(str_FileName);
                str_Result = cls_IniFileRead.GetString(str_Section, str_Key, str_Default);
                if (str_Result == "")
                {
                    return str_Default;
                }
                else
                {
                    return str_Result;
                }
            }
            catch(Exception ex)
            {
                gstrErrMsg = "IniFileModule.IniFileRead.ErrMsg:" + ex.Message;
                return "";
            }
            finally
            {
                cls_IniFileRead = null;
            }
        }

        public void IniFileWrite(string str_Section, string str_Key, string str_Value, string str_FileName = "")
        {
            gstrErrMsg = "";
            try
            {
                cls_IniFileSave = new IniFileClass(str_FileName);
                cls_IniFileSave.WriteString(str_Section, str_Key, str_Value);
            }
            catch(Exception ex)
            {
                gstrErrMsg = "IniFileModule.IniFileWrite.ErrMsg:" + ex.Message;
            }
            finally
            {
                cls_IniFileSave = null;
            }
        }
    }
}
