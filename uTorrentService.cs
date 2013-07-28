using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace uTorrentService
{
    public partial class uTorrentService : ServiceBase
    {
        private Process process;

        public uTorrentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string exeFile = Environment.ExpandEnvironmentVariables("%AppData%\\uTorrent\\uTorrent.exe");

            ProcessStartInfo psi = new ProcessStartInfo(exeFile);
            psi.UseShellExecute = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;

            process = Process.Start(psi);
            process.WaitForInputIdle();
        }

        protected override void OnStop()
        {
            process.Close();
            process.Dispose();
        }
    }
}
