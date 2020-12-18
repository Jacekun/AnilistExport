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
        // Dates
        [JsonProperty("completedAt")]
        public Date completedAt { get; set; }
        [JsonProperty("startedAt")]
        public Date startedAt { get; set; }
        // Progress
        [JsonProperty("progress")]
        public int progress { get; set; }
        [JsonProperty("progressVolumes")]
        public int progressVolumes { get; set; }
        // Others
        [JsonProperty("score")]
        public int score { get; set; }
        [JsonProperty("notes")]
        public string notes { get; set; }

        // Anime/Manga Info
        [JsonProperty("media")]
        public Media media { get; set; }
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

    public class Media
    {
        // Id
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("idMal")]
        public int idMal { get; set; }
        // Season
        [JsonProperty("season")]
        public string season { get; set; }
        [JsonProperty("seasonYear")]
        public int seasonYear { get; set; }
        // Format and Source
        [JsonProperty("format")]
        public string format { get; set; }
        [JsonProperty("source")]
        public string source { get; set; }
        // Total counts
        [JsonProperty("episodes")]
        public int episodes { get; set; }
        [JsonProperty("chapters")]
        public int chapters { get; set; }
        [JsonProperty("volumes")]
        public int volumes { get; set; }
        // Description
        [JsonProperty("description")]
        public string description { get; set; }
        // Title
        [JsonProperty("title")]
        public Title title { get; set; }
        // Other titles
        [JsonProperty("synonyms")]
        public string[] synonyms { get; set; }
        // Image link
        [JsonProperty("coverImage")]
        public CoverImg coverImage { get; set; }
    }

    public class Title
    {
        [JsonProperty("english")]
        public string english { get; set; }
        [JsonProperty("romaji")]
        public string romaji { get; set; }
    }

    public class CoverImg
    {
        [JsonProperty("medium")]
        public string medium { get; set; }
    }
}
