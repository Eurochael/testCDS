using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cds
{
    public class Config_Commi_data
    {
        //DHF : AES-CBC -> ETS-AI16AH-E -> ETS-AI16AH-E -> Sol Block1 - 32port -> Sol Block2 - 16port
        //AES-CBC : DI	DO	AI	AO	Serieal
        //          48	16	8	4	8
        //ETS-AI16AH-E : AI
        //               16
        //Sol Block1 - 32port : DO
        //                      32
        //Sol Block2 - 16port : DI
        //                      16
        //Total : DI = 48 / DO = 64 / AI = 40 / AO = 4 / Serial = 8

        public Digial_Value[] digital_in = new Digial_Value[50]; //DHF : AES-CBC(48) //APM : AES-CBC(48) //DSP : AES-CBC(48)
        public Digial_Value[] digital_out = new Digial_Value[100]; //DHF : AES-CBC-DO(16) + Solblock1(32) + Solblock2(16) //APM : AES-CBC-DO(16) + ETS-DO16N-E(16) + Solblock1(32) + Solblock2(16) //DSP : AES-CBC-DO(16) + Solblock1(32) + Solblock2(16)
        public Analog_Value[] analog_in = new Analog_Value[50]; //DHF : AES-CBC-AI(8) + ETS-Ai16AH-E1(16) + ETS-Ai16AH-E2(16) //APM : AES-CBC-AI(8) + ETS-Ai16AH-E2(16) //DSP : AES-CBC-AI(8) + ETS-Ai16AH-E1(16) + ETS-Ai16AH-E2(8)
        public Analog_Value[] analog_out = new Analog_Value[10]; // DHF : AES-CBC-AO(4)
        public Serial_Value[] serial = new Serial_Value[10]; // DHF : AES-CBC-Serial(8)

    }

    public class Commi_AES_CBC
    {
    
    }
    public class Digial_Value
    {
        public Boolean use = false;
        public string name;
        public Boolean value;
    }
    public class Analog_Value
    {
        public Boolean use = false;
        public string type; //N.O OR N.C
        public string name;
        public double value;
        public int range_row;
        public int range_high;
        public float gain;
        public float offset;
    }
    public class Serial_Value
    {
        public Boolean use = false;
        public string type; //RS-232 OR RS-485
        public string name;
        public string buadrate = "";
        public string comport = "";
        public string parrit = "";

    }
    public class Commi_Al16AH
    {

    }
}

