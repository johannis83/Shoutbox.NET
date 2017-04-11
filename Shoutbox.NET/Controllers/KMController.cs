using Microsoft.AspNet.SignalR;
using Oracle.ManagedDataAccess.Client;
using Shoutbox.NET.Hubs;
using Shoutbox.NET.Misc;
using Shoutbox.NET.Models;
using Shoutbox.NET.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class KMController : Controller, IKMRepository
    {
        //Keep a list in memory so we don't have to query the databse everytime.
        private static List<KM> KMList = new List<KM>();
        //For initialization of our KM list, we can track if it has updated atleast once
        private static bool isKMListInitialized = false;
        private static bool isTimerStarted = false;
        //KM timer to send periodic updates to all clients
        private static Timer timer = new Timer();
        //Get the shoutbox context to call shouthub functions outside of the class
        private static IHubContext shoutContext = GlobalHost.ConnectionManager.GetHubContext<ShoutHub>();

        public KMController()
        {
            //Only run the timer if it isn't running yet
            if (isTimerStarted) return;

            timer.Interval = int.Parse(ConfigurationManager.AppSettings["KMUpdateInterval"]);
            timer.Elapsed += new ElapsedEventHandler(UpdateKM_ToClients);
            timer.Start();
            isTimerStarted = true;
        }

        public void UpdateKM_ToClients(object sender, ElapsedEventArgs e)
        {
            //Store old state
            List<KM> oldList = GetList();
            //Update the previous list
            GetList();
            //Store new state
            List<KM> newList = GetList();

            /* Compare the amount of meldingen on each KM, if they're uneven, this means something changed
               OR compare the KM nummers in the list, if they're uneven, something changed */
            if(!(oldList.Where(f => newList.Any(x => x.AantalMeldingen == f.AantalMeldingen)).Count() == newList.Count()) ||
               !(oldList.Where(f => newList.Any(x => x.Nummer == f.Nummer)).Count() == newList.Count()))
            {
                shoutContext.Clients.All.UpdateKM(Newtonsoft.Json.JsonConvert.SerializeObject(newList));
            }
        }

        public List<KM> GetList()
        {
            if (!isKMListInitialized)
                UpdateKMList(bool.Parse(ConfigurationManager.AppSettings["Production"]));

            return KMList;
        }

        public void UpdateKMList(bool production)
        {
            if (production)
                UpdateKMListFromOracle();
            else
                UpdateKMListFromMSSQL();
        }

        //Gets the top KM data from the SQL database, because the dev environment does not have an oracle database.
        //DO NOT use this in production.
        private void UpdateKMListFromMSSQL()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SM9DatabaseDebug"].ConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand(HelperFunctions.LoadSQLStatement("GetTopKMsDEBUG.sql"), sqlConnection))
                {
                    using (SqlDataReader sqlReader = command.ExecuteReader())
                    {
                        List<KM> newKMList = new List<KM>();
                        while (sqlReader.Read())
                        {
                            //First record is always empty, so skip this one
                            //if (dr["KM nummer"].ToString() == "") continue;

                            newKMList.Add(
                                new KM
                                {
                                    Nummer = sqlReader["KM nummer"].ToString(),
                                    Titel = sqlReader["TITEL"].ToString(),
                                    AantalMeldingen = sqlReader["AANTAL"].ToString()
                                });
                        }

                        //Make sure the KM with the highest amount of registrations is on top
                        KMList = newKMList.OrderByDescending(f => f.AantalMeldingen).ToList();
                        isKMListInitialized = true;
                    }
                }
            }
        }

        // Function to update the KMList, retrieves the top KM's from the SM9 oracle database
        private void UpdateKMListFromOracle()
        {
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SM9Database"].ConnectionString))
            {
                connection.Open();
                using (OracleCommand command = new OracleCommand(HelperFunctions.LoadSQLStatement("GetTopKMs.sql"), connection))
                {
                    command.CommandType = CommandType.Text;
                    using (OracleDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Temporary list
                            List<KM> newKMList = new List<KM>();

                            //First record is always empty, so skip this one
                            if (dr["KM nummer"].ToString() == "") continue;

                            newKMList.Add(
                                new KM {
                                    Nummer = dr["KM nummer"].ToString(),
                                    Titel = dr["TITEL"].ToString(),
                                    AantalMeldingen = dr["AANTAL"].ToString()
                                });

                            //Make sure the KM with the highest amount of registrations is on top
                            newKMList.OrderByDescending(f => f.Nummer).ToList();
                            KMList = newKMList;
                            isKMListInitialized = true;
                        }
                    }
                }
            }
        }
    }
}