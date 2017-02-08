using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class VeronicaRock
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"current": {"title": "Chelsea Dagger","artist": "The Fratellis"}}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.VeronicaRock"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var currentSongResult = JObject.Parse(responseContent);
                        var song = new Song
                        {
                            Artist = currentSongResult["current"]["artist"].ToString().Trim(),
                            TrackName = currentSongResult["current"]["title"].ToString().Trim()
                        };
                        FileHelper.LogTrace($"VeronicaRock.GetCurrentSongAsync(): {song.Artist} - {song.TrackName}");
                        return song;
                    }
                    catch (Exception exception)
                    {
                        FileHelper.LogError($"VeronicaRock.GetCurrentSongAsync \n Error: {exception.Message} \n Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

/*
{
	"mount": "SRGSTR11",
	"previous": [{
			"type": "track",
			"id": "6557",
			"startTime": "2017-01-25T09:54:14+01:00",
			"duration": 217000,
			"title": "Heat Of The Moment",
			"artist": "Asia",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6557.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6557.jpg"
		}, {
			"type": "track",
			"id": "6401",
			"startTime": "2017-01-25T09:50:39+01:00",
			"duration": 215000,
			"title": "How You Remind Me",
			"artist": "Nickelback",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6401.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6401.jpg"
		}, {
			"type": "track",
			"id": "1352",
			"startTime": "2017-01-25T09:46:24+01:00",
			"duration": 250000,
			"title": "Runaway Train",
			"artist": "Soul Asylum",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1352.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1352.jpg"
		}
	],
	"current": {
		"id": "757",
		"startTime": "2017-01-25T09:58:57+01:00",
		"duration": 199000,
		"title": "Chelsea Dagger",
		"type": "track",
		"artist": "The Fratellis",
		"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_757.jpg",
		"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_757.jpg",
		"youtube": "sEXHeTcxQy4",
		"bol": "https:\/\/partnerprogramma.bol.com\/click\/click?p=1\u0026s=1687\u0026t=s\u0026f=SBX\u0026sec=music\u0026st=The Fratellis\u0026name=veronica_rock_radio\u0026subid=player",
		"itunes": "https:\/\/itunes.apple.com\/nl\/album\/chelsea-dagger\/id202781249?i=202781269\u0026uo=4"
	},
	"next": [],
	"processtime": "2017-01-25T09:58:57"
}
 */
