using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class Team
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Functie { get; set; }
        public string Naam { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    //Defined functies used for validation in the hub
    public static class TeamFuncties
    {
        public static List<string> Functies = new List<string>
        {
            "MCS",
            "WFM",
            "Vraagbaak",
            "Teamlead"
        };
    }
}