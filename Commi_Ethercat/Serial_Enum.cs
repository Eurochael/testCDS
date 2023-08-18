using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comi_Ethercat
{
    public partial class Class_Main
    {
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
            noen=0, odd=1, even=2
        }
        public enum enum_aes_cbc_serial_baudrate
        {
            _2400 = 0, _4800=1, _9600=2, _14400=3, _19200=4, _38400=5, _57600=6, _115200=7
        }
        public enum enum_aes_cbc_serial_type
        {
           rs232_422=0, rs485=1
        }
        //"0 : -10.24 ~ 10.24 (V)",
        //"1 : -5.12 ~ 5.12 (V)",
        //"2 : -2.56 ~ 2.56 (V)",
        //"3 : 0 ~ 10.24 (V)",
        //"4 : 0 ~ 5.12 (V)",
        //"5 : 4 ~ 20 (mA)",
        //"6 : 0 ~ 20 (mA)",
        //"7 : 0 ~ 24 (mA)",
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
    }
}
