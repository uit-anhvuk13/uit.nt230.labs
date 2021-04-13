using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2_b._1._1._4
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern void WTSSendMessage(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.I4)] int SessionId,
            String pTitle,
            [MarshalAs(UnmanagedType.U4)] int TitleLength,
            String pMessage,
            [MarshalAs(UnmanagedType.U4)] int MessageLength,
            [MarshalAs(UnmanagedType.U4)] int Style,
            [MarshalAs(UnmanagedType.U4)] int Timeout,
            [MarshalAs(UnmanagedType.U4)] out int pResponse,
            bool bWait
        );

        // that event occurs when switch users (???)
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            if (changeDescription.Reason == SessionChangeReason.SessionLogon)
            {
                // from win vista, session id simply start with 1 and increase
                // so just brute force the session id
                for (int session = 5; session > 0; --session)
                {
                    Thread t = new Thread(() =>
                    {
                        try
                        {
                            String title = "Alert", msg = "18520401 - 18521205";
                            int resp;
                            WTSSendMessage(
                                IntPtr.Zero, session,
                                title, title.Length,
                                msg, msg.Length,
                                4, 0, out resp, true
                            );
                        }
                        catch { }
                    });
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();
                }
                base.OnSessionChange(changeDescription);
            }
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
