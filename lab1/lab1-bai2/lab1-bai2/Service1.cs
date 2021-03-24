using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace lab1_bai2
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 3000; //number in milisecinds
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("notepad");
            // if notepad.exe is running
            if (pname.Length > 0)
            {
                // terminate it
                foreach (Process p in pname)
                {
                    p.Kill();
                }
            }
            else
            {
                // start a new process
                System.Diagnostics.Process.Start("notepad.exe");
            }
        }
    }
}
