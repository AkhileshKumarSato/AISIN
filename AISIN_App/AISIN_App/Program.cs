using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AISIN_App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DirectoryInfo _dir = new DirectoryInfo(Application.StartupPath + "\\" + "SatoAppResource");

            _dir = new DirectoryInfo(Application.StartupPath + "\\" + "Log");
            if (_dir.Exists == false)
            {
                _dir.Create();
            }

            bool CreatedOn;
            var mutex = new System.Threading.Mutex(true, "AISIN", out CreatedOn);
            if (!CreatedOn)
            {
                MessageBox.Show("Application already running", "AISIN", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                return;
            }
            
            Application.Run(new frmLogin());
        }
    }
}
