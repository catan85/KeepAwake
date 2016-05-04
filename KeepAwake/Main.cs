using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Configuration;

namespace KeepAwake
{
    class Program
    {

        public static void Main()
        {
            var program = new Program();
            program.run();
        }

        PerformanceCounter readBytesSec;
        PerformanceCounter writeByteSec;
        PerformanceCounter dataBytesSec;

        string pn = ConfigurationManager.AppSettings["ProcessName"];
        
        public void run()
        {
            
            readBytesSec = new PerformanceCounter("Process", "IO Read Bytes/sec", pn);
            writeByteSec = new PerformanceCounter("Process", "IO Write Bytes/sec", pn);
            dataBytesSec = new PerformanceCounter("Process", "IO Data Bytes/sec", pn);

            while (true)
            {
                getProcessMeasures();

                System.Threading.Thread.Sleep(5000);
            }
        }

        Awaker awaker = new Awaker();

        


        private void getProcessMeasures()
        {

            List<PerformanceCounter> counters = new List<PerformanceCounter>
                        {
                        readBytesSec,
                        writeByteSec,
                        dataBytesSec
                        };


            bool valueExeedLimit = false;

            // get current counter value and check if it exeed the speed limit
            foreach (PerformanceCounter counter in counters)
            {
                try
                {
                    float rawValue = counter.NextValue();
                    Console.WriteLine(counter.CounterName + "\t\tKb/s:\t" + (rawValue/1000).ToString());

                    if ((rawValue / 1000) > Convert.ToInt32(ConfigurationManager.AppSettings["ProcessSpeedLimit"]))
                    {
                        valueExeedLimit = true;
                    }

                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            // If a value exeed the speed limit (upload or download transfer rate), then start the awaker, else stop it
            if (valueExeedLimit) 
            { 
                awaker.Start(); 
            } 
            else 
            { 
                awaker.Stop(); 
            }


        }


    }
}
