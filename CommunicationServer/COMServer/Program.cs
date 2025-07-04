using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMServer
{
    static class Program
    {
        public static string PrinterName = "";
        public static string MachiningPrinterIP = "";
        public static string MachiningPrinterPort="";
        public static string FinalPackingPrinterIP = "";
        public static string FinalPackingPrinterPort = "";
        public static string MachiningPrnName = "MACHINING.prn";
        public static string TrolleyBox = "TROLLEYBOX.prn";
        public static string FinalPackingPrnName = "FINALPACKING.prn";
        static string Exe = "SchedulerExeAutoRestart";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    if (Process.GetProcessesByName(Exe).Length == 0)
                    {
                        Process.Start(Application.StartupPath + "\\" + Exe);
                    }
                }
                catch (Exception)
                {
                }
                bool CreatedOn;
                System.Threading.Mutex m = new System.Threading.Mutex(true, "SatoCOMServer", out CreatedOn);
                if (!CreatedOn)
                {
                    MessageBox.Show("Comm Server already running", "SatoCOMServer", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
                else
                { Application.Run(new frmServer()); }
            }
            catch (Exception)
            {

                MessageBox.Show("Communitation Server already running");
            }
        }
    }
}
