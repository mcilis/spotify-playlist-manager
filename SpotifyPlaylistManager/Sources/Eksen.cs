using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class Eksen
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"Artist":"PARQUET COURTS", "TrackName":"ONE MAN, NO CITY"}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.Eksen"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<Song>(responseContent);
                    }
                    catch (Exception exception)
                    {
                        FileHelper.AddTextToFile("SpotifyPlaylistManager", $"Eksen.GetCurrentSongAsync - Error: {exception.Message} Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

// {"PlayDate":"","Artist":"PARQUET COURTS","TrackName":"ONE MAN, NO CITY","CoverPath":""}
