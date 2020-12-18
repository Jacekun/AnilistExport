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
    }
}
