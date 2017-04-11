using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoutbox.NET;
using Oracle.ManagedDataAccess.Client;
using Shoutbox.NET.Misc;
using System.Data;

namespace TopKM_Oracle_DataTester
{
    class Program
    {
        /* 
         * This tool is developed with the purpose of testing the Oracle SQL query for receiving the Top KMs
         * Typically we don't use Oracle databases on the development nor test environments
         * so we never actually get to test these connections.
         * By running this executable in the *production environment* you can verify if the Query/dataconnection
         * is still valid.
         */

        static void Main(string[] args)
        {
            //Get the correct connection string from the Shoutbox.NET web.config
            string oradb = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=prodcl301-scan.oracle.rabobank.nl)(PORT=39000))(CONNECT_DATA=(SERVICE_NAME=SRV0PSIB101)));User Id=user_lkt;Password=us3r_lkt;";
            OracleConnection conn = new OracleConnection(oradb);
            conn.Open();
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            //Get the live query from Shoutbox.NET -> Data -> SQLQueries -> GetTopKMs.sql
            cmd.CommandText = 
                "SELECT * FROM(SELECT  distinct \"KM nummer\", \"OWNER_SM1\".\"VW_KMUSAGE_LKT\".\"AANTAL\", \"OWNER_SM1\"." +
                "\"VW_KMUSAGE_LKT\".\"TITEL\"FROM \"OWNER_SM1\".\"VW_INTERACTIONS_DETAIL_LKT\" vwinteractions LEFT JOIN \"OWNER_SM1\".\"VW_KMUSAGE_LKT\" " +
                "ON \"OWNER_SM1\".\"VW_KMUSAGE_LKT\".\"ID\" = vwinteractions.\"KM nummer\" WHERE  \"Opened By\" != 'KPL-CC' order by \"OWNER_SM1\".\"VW_KMUSAGE_LKT\".\"AANTAL\" " +
                "desc) WHERE ROWNUM <= 14";

            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Console.WriteLine("KM Nummer: {0} Aantal: {1} Titel: {2}", dr["KM nummer"], dr["AANTAL"], dr["TITEL"]);
            }

            Console.ReadLine();
        }
    }
}
