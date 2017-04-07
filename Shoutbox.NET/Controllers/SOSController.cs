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
using System.Web;
using System.Web.Mvc;

namespace Shoutbox.NET.Controllers
{
    public class SOSController : Controller, ISOSRepository
    {
        //Keep a list in memory so we don't have to query the databse everytime.
        private static List<SOS> SOSList = new List<SOS>();
        //For initialization of our SOS list, we can track if it has updated atleast once
        private bool isSOSListInitialized = false;

        public SOSController() { }

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
                using (SqlCommand sqlCommand = new SqlCommand(LoadSQLStatement("GetSOSMeldingen.sql"), sqlConnection))
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

                        SOSList = newSOSList;
                        isSOSListInitialized = true;
                    }
                }
            }
        }
        private string LoadSQLStatement(string statementName)
        {
            string sqlStatement = string.Empty;

            string namespacePart = "Shoutbox.NET.Data.SQLQueries";
            string resourceName = namespacePart + "." + statementName;

            using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stm != null)
                {
                    sqlStatement = new StreamReader(stm).ReadToEnd();
                }
            }

            return sqlStatement;
        }
    }
}