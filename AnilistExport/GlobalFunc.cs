using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AnilistExport
{
    public static class GlobalFunc
    {
        // Static Variables
        public static string AnilistURL = "https://graphql.anilist.co"; // API URL, send requests

        // validators for data types
        public static string ValidateString(string text)
        {
            if (String.IsNullOrEmpty(text)) { return ""; }
            return text.Replace('"', '\'');
        }
        public static string ValidateDate(int year, int month, int day, bool toMal = false)
        {
            string ret = "";
            if (year > 0) { ret += year.ToString("0000") + "-"; } else { ret += "0";  }
            if (month > 0) { ret += month.ToString("00") + "-"; } else { ret += "0"; }
            if (day > 0) { ret += day.ToString("00"); } else { ret += "0"; }
            if (ret == "000")
            {
                return (toMal ? "0000-00-00" : "");
            }
            return ret;
        }

        // Write to File, append
        public static bool WriteAppend(string file, string content)
        {
            try {
                using (StreamWriter sw = new StreamWriter(File.Open(file, FileMode.Append), Encoding.UTF8))
                {
                    sw.WriteLine(content);
                    return true;
                }
            } catch { return false; }
        }

        // Remove last characters from file
        public static bool WriteRemove(string file, int count)
        {
            try
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite))
                {
                    fileStream.Seek(-1, SeekOrigin.End);
                    fileStream.SetLength(fileStream.Length - count);
                    return true;
                }
            } catch { return false; }
        }

        // MAL String formats
        public static string toMalString(string name, string content)
        {
            return "<" + name + "><![CDATA[" + content + "]]></" + name + ">";
        }
        public static string toMalVal(string name, string content)
        {
            return "<" + name + ">" + content + "</" + name + ">";
        }
        public static string toMalStatus(string status, string media = "anime")
        {
            if (status == "COMPLETED") { return "Completed"; }
            else if (status == "PAUSED") { return "On-Hold"; }
            else if (status == "CURRENT") { return (media == "anime" ? "Watching" : "Reading"); }
            else if (status == "DROPPED") { return "Dropped"; }
            else if (status == "PLANNING") { return (media == "anime" ? "Plan to Watch" : "Plan to Read"); }
            else { return ""; }
        }

        // Return JSON String, formatted from Anilist
        public static string EntryJson(JsonMediaEntry entry, string media = "anime")
        {
            string jsontoAdd = "\t{\n";
            // ID
            jsontoAdd += "\t\t\"idAnilist\": " + entry.media.id.ToString() + ",\n";
            string malID = entry.media.idMal.ToString();
            jsontoAdd += "\t\t\"idMal\": " + malID + ",\n";
            // Titles
            jsontoAdd += "\t\t\"titleEnglish\": \"" + ValidateString(entry.media.title.english) + "\",\n";
            jsontoAdd += "\t\t\"titleRomaji\": \"" + ValidateString(entry.media.title.romaji) + "\",\n";
            jsontoAdd += "\t\t\"synonyms\": \"" + ValidateString(string.Join(", ", entry.media.synonyms)) + "\",\n";
            // Format and Source
            jsontoAdd += "\t\t\"format\": \"" + ValidateString(entry.media.format) + "\",\n";
            jsontoAdd += "\t\t\"source\": \"" + ValidateString(entry.media.source) + "\",\n";
            // Status and dates
            jsontoAdd += "\t\t\"status\": \"" + ValidateString(entry.status) + "\",\n";
            jsontoAdd += "\t\t\"startedAt\": \"" + ValidateDate(entry.startedAt.year, entry.startedAt.month, entry.startedAt.day) + "\",\n";
            jsontoAdd += "\t\t\"completedAt\": \"" + ValidateDate(entry.completedAt.year, entry.completedAt.month, entry.completedAt.day) + "\",\n";
            // Progress
            jsontoAdd += "\t\t\"progress\": " + ValidateString(entry.progress.ToString()) + ",\n";

            if (media == "anime")
            {
                jsontoAdd += "\t\t\"totalEpisodes\": " + entry.media.episodes.ToString() + ",\n";
            }
            else
            {
                jsontoAdd += "\t\t\"progressVolumes\": " + entry.progressVolumes.ToString() + ",\n";
                jsontoAdd += "\t\t\"totalChapters\": " + entry.media.chapters.ToString() + ",\n";
                jsontoAdd += "\t\t\"totalVol\": " + entry.media.volumes.ToString() + ",\n";
            }

            // Others
            jsontoAdd += "\t\t\"score\": " + entry.score.ToString() + ",\n";
            jsontoAdd += "\t\t\"notes\": \"" + ValidateString(entry.notes) + "\"\n\t},\n";
            return jsontoAdd;
        }
    // End of class: GlobalFunc
    }
}
