using System;

namespace cds
{
    static public class Class_Serial_Common
    {
        public struct Rcv_Data
        {
            public DateTime dt_rcvtime;
            public byte address;
            public byte function_code;
            public int read_data_byte_cnt;
            public string[] reading_data;
            public string crc;
            public string rcv_data_to_string;
        }


    }
}
