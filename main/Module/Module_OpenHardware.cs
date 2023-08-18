using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cds
{
    public class Module_OpenHardware
    {
        public enum EInit
        {
            Required,
            InitStart,
            InitEnd,
        }

        public class DisplayItem
        {
            public string Name { get; set; }
            public float Data { get; set; }
        }

        public class PCInformation
        {
            private static readonly Lazy<PCInformation> lazy = new Lazy<PCInformation>(() => new PCInformation());

            public static PCInformation Instance
            { get { return lazy.Value; } }

            public ObservableCollection<DisplayItem> DisplayItems { get; set; } = new ObservableCollection<DisplayItem>();

            public PCInformation()
            {
                this.PC = new Computer { CPUEnabled = true, MainboardEnabled = true, RAMEnabled = true, FanControllerEnabled = true };
                this.PC.Open();
            }

            private Computer PC;

            private EInit InitListItem = EInit.Required;
            private bool DisplayUse = false;
            private Dictionary<string, int> HardwareSensorCounts = new Dictionary<string, int>();

            private bool HourLogWrite = false;
            private string HourLog = string.Empty;

            public void DataUpdate()
            {

                if (this.PC == null) return;

                DateTime current = DateTime.Now;
                if ((current.Minute == 59) && (current.Minute > 58) && (this.HourLogWrite == false))
                {
                    this.HourLogWrite = true;
                }

                int itemCount = 0;

                switch (this.InitListItem)
                {
                    case EInit.Required:
                        this.DisplayItems.Clear();
                        this.InitListItem = EInit.InitStart;
                        break;

                    case EInit.InitStart:
                        this.InitListItem = EInit.InitEnd;
                        break;
                }

                Program.main_form.hardware_info = "";
                foreach (IHardware hardware in this.PC.Hardware)
                {
                    try
                    {
                        hardware.Update();
                        this.CheckInitState(hardware);

                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (this.DisplayUse) this.SetDisplay(sensor, itemCount);

                            itemCount++;
                            Program.main_form.hardware_info = Program.main_form.hardware_info + " ## " + $"{sensor.Name} / {sensor.SensorType} / Data: {sensor.Value} ";
                            //this.WriteLog(hardware, sensor, (current.Minute == 0) && (current.Second > 0) && this.HourLogWrite);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                    
                }

                if (this.HourLogWrite)
                {
                    Console.WriteLine(this.HourLog);
                    this.HourLog = string.Empty;
                    this.HourLogWrite = false;
                }
            }

            private void WriteLog(IHardware hardware, ISensor sensor, bool hourLog = false)
            {
                bool writeLog = false;
                string log = "";
                switch (sensor.SensorType)
                {
                    case SensorType.Load:
                        switch (hardware.HardwareType)
                        {
                            case HardwareType.CPU:

                                if (sensor.Value >= 30) writeLog = true;
                                break;

                            case HardwareType.RAM:

                                if (sensor.Value >= 70) writeLog = true;
                                break;
                        }
                        break;

                    case SensorType.Temperature:
                        if (sensor.Value >= 70) writeLog = true;
                        break;

                    case SensorType.Fan:

                        if (sensor.Value < 500) writeLog = true;
                        break;
                }
                //Program.main_form.hardware_info = Program.main_form.hardware_info + " ## " + $"{sensor.Name} / {sensor.SensorType} / Data: {sensor.Value} ";
                //if (writeLog) Console.WriteLine($"{sensor.Name} / {sensor.SensorType} / Data: {sensor.Value} ");
                if (this.HourLogWrite && hourLog) this.HourLog += $"{sensor.Name} | {sensor.SensorType} | {sensor.Value}, ";
            }

            private void SetDisplay(ISensor sensor, int itemCount)
            {
                if (this.InitListItem == EInit.InitStart)
                {
                    this.DisplayItems.Add(
                        new DisplayItem
                        {
                            Name = $"{sensor.Name} {sensor.SensorType}",
                            Data = (float)sensor.Value
                        });
                }
                else
                {
                    this.DisplayItems[itemCount].Data = (float)sensor.Value;
                }
            }

            private void CheckInitState(IHardware hardware)
            {
                string identifier = hardware.Identifier.ToString();
                if (this.InitListItem == EInit.InitStart)
                {
                    if (hardware.Sensors != null)
                        this.HardwareSensorCounts.Add(identifier, hardware.Sensors.Length);
                }
                else
                {
                    if (this.HardwareSensorCounts.ContainsKey(identifier) == false)
                    {
                        this.HardwareSensorCounts.Add(identifier, hardware.Sensors.Length);
                        this.InitListItem = EInit.Required;
                    }
                    else
                    {
                        if (this.HardwareSensorCounts[identifier] != hardware.Sensors.Length)
                        {
                            this.InitListItem = EInit.Required;
                        }
                    }
                }
            }
        }
    }

}
