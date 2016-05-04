using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace KeepAwake
{
    class Awaker
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        public bool isStarted
        {
            get { return worker != null; }
        }

        Thread worker;
        public void Start()
        {
            if (isStarted) return;
            worker = new Thread(worker_thread);
            worker.Start();
        }
        public void Stop()
        {
            if (isStarted)
            {
                worker.Abort();
            }
            worker = null;
        }
        void worker_thread()
        {
            while (true)
            {
                SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                Console.WriteLine("awaked!");
                Thread.Sleep(1000);
            }
        }
    }
}
