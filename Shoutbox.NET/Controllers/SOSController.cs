using Microsoft.AspNet.SignalR;
using Shoutbox.NET.Hubs;
using Shoutbox.NET.Misc;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class SOSController : Controller, ISOSRepository
    {
        //Keep a list in memory so we don't have to query the databse everytime.
        private static List<SOS> SOSList = new List<SOS>();
        //For initialization of our SOS list, we can track if it has updated atleast once
        private static bool isSOSListInitialized = false;
        private static bool isTimerStarted = false;
        //SOS timer to send periodic updates to all clients
        private static Timer timer = new Timer();
        //Get the shoutbox context to call shouthub functions outside of the class
        private static IHubContext shoutContext = GlobalHost.ConnectionManager.GetHubContext<ShoutHub>();


        public SOSController()
        {
            //Only run the timer if it isn't running yet
            if (isTimerStarted) return;

            timer.Interval = int.Parse(ConfigurationManager.AppSettings["SOSUpdateInterval"]);
            timer.Elapsed += new ElapsedEventHandler(UpdateSOS_ToClients);
            timer.Start();
            isTimerStarted = true;
        }

        //Broadcast to clients
        public void UpdateSOS_ToClients(object sender, ElapsedEventArgs e)
        {
            //Store old state
            List<SOS> oldList = GetList();
            //Update the previous list
            GetList();
            //Store new state
            List<SOS> newList = GetList();

            //Compare the time of the latest updated issues, or the amount of SOS's in the list, if they're uneven, this means
            //the list has changed, so broadcast the new list to the clients
            if (!(oldList.Where(f => newList.Any(x => x.Time == f.Time)).Count() == newList.Count()) ||
                newList.Count != oldList.Count)
            {
                shoutContext.Clients.All.UpdateSOS(Newtonsoft.Json.JsonConvert.SerializeObject(newList));
            }
        }

        public List<SOS> GetList()
        {
            if (!isSOSListInitialized)
                UpdateSOSList();

            return SOSList;
        }

        // Function to update the SOSList
        public void UpdateSOSList()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SOSDatabase"].ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(HelperFunctions.LoadSQLStatement("GetSOSMeldingen.sql"), sqlConnection))
                {
                    using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                    {
                        List<SOS> newSOSList = new List<SOS>();

                        while (sqlReader.Read())
                        {
                            newSOSList.Add(
                                new SOS
                                {
                                    Name = sqlReader["NAME"].ToString(),
                                    //Descriptions are often returned with <p> tags, this regex strips those.
                                    Description = Regex.Replace(sqlReader["DESCRIPTION"].ToString(), @"<[^>]*>", ""),
                                    Time = DateTime.Parse(sqlReader["LATEST_UPDATE"].ToString())
                                });
                        }

                        //Sort by time so we can easily put the newest items on top
                        newSOSList = newSOSList.OrderByDescending(f => f.Time).ToList();
                        SOSList = newSOSList;
                        isSOSListInitialized = true;
                    }
                }
            }
        }
        
    }
}