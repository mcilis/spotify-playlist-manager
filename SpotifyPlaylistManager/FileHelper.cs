using System;
using System.IO;
using System.Text;
using System.Configuration;

namespace SpotifyPlaylistManager
{
    public class FileHelper
    {
        const string _directory =  @"c:\log\spotifyplaylistmanager\";
        static string _logLevel = ConfigurationManager.AppSettings["LogLevel"]; // Error / Trace

        public static bool FileContainsText(string filename, string text)
        {
            var path = Path.Combine(_directory, $"{filename.Replace(" ", ".")}.log");

            if (!File.Exists(path))
                return false;

            var fileData = File.ReadAllText(path, Encoding.UTF8);

            return fileData.Contains(text);
        }

        public static void AddTextToFile(string filename, string text)
        {
            var path = Path.Combine(_directory, $"{filename.Replace(" ", ".")}.log");

            File.AppendAllText(path, $"{DateTime.Now.ToString("o")} | {text}\n");
        }

        public static void LogError(string text)
        {
            var path = Path.Combine(_directory, "SpotifyPlaylistManagerCritical.log");

            File.AppendAllText(path, $"\n{DateTime.Now.ToString("o")}\n {text}\n---------------------------------------------------------------------------------------------");
        }

        public static void LogTrace(string text)
        {
            if (_logLevel != "Trace")
                return;

            var path = Path.Combine(_directory, "SpotifyPlaylistManagerTrace.log");

            File.AppendAllText(path, $"{DateTime.Now.ToString("o")} | {text}\n");
        }
    }
}
