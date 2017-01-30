using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class RedFm
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"songtitle":Rory Gallagher - Moonchild"}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.RedFm"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var currentSongResult = JObject.Parse(responseContent);
                        var songTitle = currentSongResult.GetValue("songtitle").ToString();
                        var splittedTitle = songTitle.Split('-');
                        if (splittedTitle.Length == 2)
                        {
                            return new Song { Artist = splittedTitle[0].Trim(), TrackName = splittedTitle[1].Trim() };
                        }
                        return null;
                    }
                    catch (Exception exception)
                    {
                        FileHelper.AddTextToFile("SpotifyPlaylistManager", $"RedFm.GetCurrentSongAsync - Error: {exception.Message} Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

/*
{
	"currentlisteners": 1220,
	"peaklisteners": 1342,
	"maxlisteners": 10000,
	"uniquelisteners": 1191,
	"averagetime": 4658,
	"servergenre": "Rock",
	"servergenre2": "",
	"servergenre3": "",
	"servergenre4": "",
	"servergenre5": "",
	"serverurl": "http:\/\/www.redfm.gr",
	"servertitle": "Red",
	"songtitle": "Rory Gallagher - Moonchild",
	"streamhits": 222042,
	"streamstatus": 1,
	"backupstatus": 0,
	"streamlisted": 0,
	"streamlistederror": 200,
	"streampath": "\/stream",
	"streamuptime": 9536,
	"bitrate": "128",
	"content": "audio\/mpeg",
	"version": "2.4.7.256 (posix(linux x64))"
}
 */
