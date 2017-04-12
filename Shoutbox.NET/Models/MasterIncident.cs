﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoutbox.NET.Models
{
    public class MasterIncident
    {
        public int MasterIncidentID { get; set; }
        public string Description { get; set; }
        public string KM { get; set; }
        public string IM { get; set; }
        public DateTime Timestamp { get; set; }
        [JsonIgnore]
        public bool Active { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}