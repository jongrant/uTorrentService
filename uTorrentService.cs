using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace uTorrentService
{
    using System.IO;

    public partial class uTorrentService : ServiceBase
    {
        private Process process;

        public uTorrentService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var exePath = GetExeLocation(args);

            if (!File.Exists(exePath))
            {
                var message = string.Format("The file {0} could not be found.", exePath);
                this.EventLog.WriteEntry(message, EventLogEntryType.Error);

                Environment.Exit(1);
            }

            ProcessStartInfo psi = new ProcessStartInfo(exePath);
            psi.UseShellExecute = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;

            try
            {
                this.process = Process.Start(psi);

                if (this.process != null)
                {
                    process.WaitForInputIdle();
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("Executing the utorrent.exe failed: {0}", ex);
                this.EventLog.WriteEntry(message, EventLogEntryType.Error);

                Environment.Exit(2);
            }
        }

        protected override void OnStop()
        {
            process.Close();
            process.Dispose();
        }

        private string GetExeLocation(string[] args)
        {
            if (args.Length != 0)
            {
                return args[0];
            }

            var sameDirPath = Path.Combine(Environment.CurrentDirectory, "uTorrent.exe");

            if (File.Exists(sameDirPath))
            {
                return sameDirPath;
            }

            return Environment.ExpandEnvironmentVariables("%AppData%\\uTorrent\\uTorrent.exe");
        }
    }
}