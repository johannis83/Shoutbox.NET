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
        private Status SM9Status;
        private Status EasyVistaStatus;

        private Timer timer;
        private string SM9Directory;
        private string EasyVistaDirectory;

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

            SM9Directory = ConfigurationManager.AppSettings["SM9Directory"];
            EasyVistaDirectory = ConfigurationManager.AppSettings["EasyVistaDirectory"];

            LogDirectory = ConfigurationManager.AppSettings["LogDirectory"];

            timer = new Timer()
            {
                Interval = int.Parse(ConfigurationManager.AppSettings["Interval"])
            };

            timer.Elapsed += new ElapsedEventHandler(Timer_elapsed);
            timer.Start();

            CheckSM9Status();
            CheckEasyVistaStatus();
        }

        private void Timer_elapsed(object sender, ElapsedEventArgs e)
        {
            CheckSM9Status();
            CheckEasyVistaStatus();
        }

        private void CheckSM9Status()
        {
            //Invalid directory? We can't know!
            if (!Directory.Exists(SM9Directory))
            {
                SM9Status = Status.Unknown;
                return;
            }

            //No files there? Service must be running fine!
            if (Directory.GetFiles(SM9Directory, "*.xml", SearchOption.TopDirectoryOnly).Length == 0)
            {
                SM9Status = Status.Online;
                return;
            }

            //Get the oldest file in the specified directory
            DateTime oldestFileDate = Directory.GetFiles(SM9Directory, "*.xml", SearchOption.TopDirectoryOnly).
                Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTime).FirstOrDefault().CreationTime;

            TimeSpan fileAge = DateTime.Now - oldestFileDate;

            if (fileAge.Minutes <= Online.Minutes)
                SM9Status = Status.Online;
            else if (fileAge.Minutes >= Offline.Minutes)
            {
                SM9Status = Status.Offline;
                Log(SM9Status, fileAge, "SM9");
            }
            else if (fileAge.Minutes >= Warning.Minutes)
            {
                SM9Status = Status.Warning;
                Log(SM9Status, fileAge, "SM9");
            }
        }

        private void CheckEasyVistaStatus()
        {
            //Invalid directory? We can't know!
            if(!Directory.Exists(EasyVistaDirectory))
            {
                EasyVistaStatus = Status.Unknown;
                return;
            }

            //No files there? Service must be running fine!
            if (Directory.GetFiles(EasyVistaDirectory, "*.json", SearchOption.TopDirectoryOnly).Length == 0)
            {
                EasyVistaStatus = Status.Online;
                return;
            }

            //Get the oldest file in the specified directory
            DateTime oldestFileDate = Directory.GetFiles(EasyVistaDirectory, "*.json", SearchOption.TopDirectoryOnly).
                Select(f => new FileInfo(f)).OrderByDescending(f => f.CreationTime).FirstOrDefault().CreationTime;

            TimeSpan fileAge = DateTime.Now - oldestFileDate;

            if (fileAge.Minutes <= Online.Minutes)
                EasyVistaStatus = Status.Online;
            else if (fileAge.Minutes >= Offline.Minutes)
            {
                EasyVistaStatus = Status.Offline;
                Log(EasyVistaStatus, fileAge, "EasyVista");
            }
            else if (fileAge.Minutes >= Warning.Minutes)
            {
                EasyVistaStatus = Status.Warning;
                Log(EasyVistaStatus, fileAge, "EasyVista");
            }
        }


        private void Log(Status status, TimeSpan fileAge, string Service)
        {
            if (!LoggingEnabled) return;

            File.AppendAllText(Path.Combine(LogDirectory, LOGFILENAME),
                        string.Format("[{0} {1}: The {2} process is {2} minutes late with processing SST files. \n", 
                               DateTime.Now, status.ToString().ToUpper(), fileAge.Minutes - Online.Minutes));
        }

        public Status GetStatus(string Service)
        {
            if (Service == "SM9")
                return SM9Status;
            else if (Service == "EasyVista")
                return EasyVistaStatus;
            return Status.Unknown;
        }
    }
}
