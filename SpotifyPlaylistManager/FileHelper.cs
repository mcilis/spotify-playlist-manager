﻿using System;
using System.IO;
using System.Text;

namespace SpotifyPlaylistManager
{
    public class FileHelper
    {
        const string _directory =  @"c:\log\spotifyplaylistmanager\";
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
    }
}
