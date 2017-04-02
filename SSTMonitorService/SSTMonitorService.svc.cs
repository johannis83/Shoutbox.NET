using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Timers;

namespace SSTMonitorService
{
    public class SSTMonitorService : ISSTMonitorService
    {
        private Status CurrentStatus;
        private Timer timer;
        private string XMLDirectory;
        private bool LoggingEnabled;
        private string LogDirectory;

        //Everytime the files don't get picked up in time, this will be logged to this log file
        private const string LOGFILENAME = "SSTMonitor-Log-output.log";

        public TimeSpan Online;
        public TimeSpan Warning;
        public TimeSpan Offline;

        public SSTMonitorService()
        {
            LoggingEnabled = bool.Parse(ConfigurationManager.AppSettings["LoggingEnabled"]);
            Online = TimeSpan.FromMilliseconds(Double.Parse(ConfigurationManager.AppSettings["Online"]));
            Warning = TimeSpan.FromMilliseconds(Double.Parse(ConfigurationManager.AppSettings["Warning"]));
            Offline = TimeSpan.FromMilliseconds(Double.Parse(ConfigurationManager.AppSettings["Offline"]));
            XMLDirectory = ConfigurationManager.AppSettings["XMLDirectory"];
            LogDirectory = ConfigurationManager.AppSettings["LogDirectory"];

            timer = new Timer()
            {
                Interval = int.Parse(ConfigurationManager.AppSettings["Interval"])
            };

            timer.Elapsed += new ElapsedEventHandler(Timer_elapsed);
            timer.Start();

            SetStatus();
        }

        private void Timer_elapsed(object sender, ElapsedEventArgs e)
        {
            SetStatus();
        }


        private void SetStatus()
        {
            //No files there? Service must be running fine!
            if(Directory.GetFiles(XMLDirectory).Length == 0)
            {
                CurrentStatus = Status.Online;
                return;
            }

            //Get the oldest file in the specified directory
            DateTime oldestFileDate = Directory.GetFiles(XMLDirectory, "*.xml", SearchOption.TopDirectoryOnly).
                Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTime).FirstOrDefault().CreationTime;

            TimeSpan fileAge = DateTime.Now - oldestFileDate;

            if (fileAge.Minutes <= Online.Minutes)
                CurrentStatus = Status.Online;
            else if (fileAge.Minutes >= Offline.Minutes)
            {
                CurrentStatus = Status.Offline;
                Log(CurrentStatus, fileAge);
            }
            else if (fileAge.Minutes >= Warning.Minutes)
            {
                CurrentStatus = Status.Warning;
                Log(CurrentStatus, fileAge);
            }
        }


        private void Log(Status status, TimeSpan fileAge)
        {
            if (!LoggingEnabled) return;

            File.AppendAllText(Path.Combine(LogDirectory, LOGFILENAME),
                        string.Format("[{0}] {1}: The SST form process is {2} minutes late with processing SST files. \n", DateTime.Now, status.ToString().ToUpper(), fileAge.Minutes - Online.Minutes));
        }

        public Status GetStatus()
        {
            return CurrentStatus;
        }
    }
}
