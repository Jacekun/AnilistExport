using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnilistExport
{
    public class JsonMedia
    {
        [JsonProperty("lists")]
        public Lists[] list { get; set; }
    }

    public class Lists
    {
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("entries")]
        public Entry[] entries { get; set; }
    }

    public class Entry
    {
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("completedAt")]
        public Date completedAt { get; set; }
        [JsonProperty("startedAt")]
        public Date startedAt { get; set; }
    }

    public class Date
    {
        [JsonProperty("year")]
        public int year { get; set; }
        [JsonProperty("month")]
        public int month { get; set; }
        [JsonProperty("day")]
        public int day { get; set; }
    }
}
