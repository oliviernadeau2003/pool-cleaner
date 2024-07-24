using System;
using System.ServiceProcess;
using System.Timers;
using System.IO;

namespace PoolCleaner
{
    public partial class Service1 : ServiceBase
    {
        private const int timerInterval = 86400000;                        // milliseconds (86400000 -> 24 hour)
        private readonly Timer timer = new Timer();                        // C:\Windows\System32\spool\PRINTERS\
        private readonly DirectoryInfo workingDirectory = new DirectoryInfo("C:\\Windows\\System32\\spool\\PRINTERS\\");
        private readonly string logsPath = AppDomain.CurrentDomain.BaseDirectory + "\\logs";
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = timerInterval; 
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is started at " + DateTime.Now);
        }

        // When Timer Is Reached, It Execute This Code
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            int nbDeletedFile = 0;
            foreach (FileInfo file in workingDirectory.GetFiles())
            {
                file.Delete();
                nbDeletedFile++;
            }
            WriteToFile("Service is recall at " + DateTime.Now);
            WriteToFile("# Service Deleted " + nbDeletedFile + " Files --\n" );
        }

        private void WriteToFile(string message)
        {
            if (!Directory.Exists(logsPath))
                Directory.CreateDirectory(logsPath);
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
                using (StreamWriter sw = File.CreateText(filepath)) { sw.WriteLine(message); }
            else
                using (StreamWriter sw = File.AppendText(filepath)) { sw.WriteLine(message); }
        }
    }
}
