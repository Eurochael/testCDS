using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace LOG
{
    public class LogClass
    {
        public string gstrErrMsg;
        private string str_BasicLogPath = @"Log\";
        private bool bol_LogEnd = false;
        private ConcurrentQueue<string> que_LogData = new ConcurrentQueue<string>();
        private Thread thd_LogWrite;

        public LogClass()
        {
            thd_LogWrite = new Thread(ThreadingLogWrite);
            thd_LogWrite.Start();
        }

        public void close()
        {
            bol_LogEnd = true;
            if (thd_LogWrite !=null)
            {
                thd_LogWrite.Join(10000);
            }
            
        }
        //모듈에는 Finalize 설정 후 close 시 Finalize를 호출
        //finalize
        //protected override void Finalize()
        //{
        //    base.Finalize();
        //}

        public void LogWrite(string str_Log, string str_PathName = "", string str_FileName = "")
        {
            que_LogData.Enqueue(str_Log + "@" + str_PathName + "@" + str_FileName);
        }

        private void ThreadingLogWrite()
        {
            string str_buff;
            while(bol_LogEnd == false)
            {
                try
                {
                    if(que_LogData.Count > 0)
                    {
                        str_buff = "";
                        que_LogData.TryDequeue(out str_buff);
                        if(str_buff != null && str_buff != "" && str_buff.Split('@').Length > 2)
                        {
                            subLogWrite(str_buff.Split('@')[0], str_buff.Split('@')[1], str_buff.Split('@')[2]);
                        }
                    }
                    System.Threading.Thread.Sleep(2);
                }
                catch(Exception ex)
                {
                    gstrErrMsg = "LogClass.ThreadingLogWrite.ErrMsg:" + ex.ToString();
                    Console.WriteLine(gstrErrMsg); 
                }
            }
        }

        private void subLogWrite(string str_ReadLog, string str_RealPathName = "", string str_RealFileName = "")
        {
            try
            {
                str_ReadLog = str_ReadLog + "\r\n";

                if(str_RealPathName =="")
                {
                    str_RealPathName = str_BasicLogPath;
                }

                if (Directory.Exists(str_RealPathName)==false)
                {
                    Directory.CreateDirectory(str_RealPathName);
                }
                if (str_RealFileName=="")
                {
                    str_RealFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                }
                File.AppendAllText(str_RealPathName + str_RealFileName, str_ReadLog);
            }
            catch(Exception ex)
            {
                gstrErrMsg = "LogClass.subLogWrite.ErrMsg:" + ex.ToString();
                Console.WriteLine(gstrErrMsg);
            }
        }



    }
}
