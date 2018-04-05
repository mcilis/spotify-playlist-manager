using System;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class Veronica
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // {"current": {"title": "Runaway Train","artist": "Soul Asylum"}}

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.Veronica"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var currentSongResult = JObject.Parse(responseContent);

                        var type = currentSongResult["current"]["type"].ToString();                    
                        if (type != "track")
                        {
                            return null;
                        }

                        var song = new Song
                        {
                            Artist = currentSongResult["current"]["artist"].ToString().Trim(),
                            TrackName = currentSongResult["current"]["title"].ToString().Trim()
                        };
                        FileHelper.LogTrace($"Veronica.GetCurrentSongAsync(): {song.Artist} - {song.TrackName}");
                        return song;
                    }
                    catch (Exception exception)
                    {
                        FileHelper.LogError($"Veronica.GetCurrentSongAsync \n Error: {exception.Message} \n {exception.InnerException?.Message} \n Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

/*
{
	"mount": "VERONICA",
	"previous": [{
			"type": "track",
			"id": "7011",
			"startTime": "2017-01-25T09:48:43+01:00",
			"duration": 287000,
			"title": "Something Beautiful",
			"artist": "Robbie Williams",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_7011.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_7011.jpg"
		}, {
			"type": "track",
			"id": "836",
			"startTime": "2017-01-25T09:45:28+01:00",
			"duration": 203000,
			"title": "Ruby",
			"artist": "Kaiser Chiefs",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_836.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_836.jpg"
		}, {
			"type": "track",
			"id": "5420",
			"startTime": "2017-01-25T09:41:36+01:00",
			"duration": 175000,
			"title": "I Would Die 4 You",
			"artist": "Prince and The Revolution",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_5420.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_5420.jpg"
		}
	],
	"current": {
		"id": "1352",
		"startTime": "2017-01-25T09:53:54+01:00",
		"duration": 260000,
		"title": "Runaway Train",
		"type": "track",
		"artist": "Soul Asylum",
		"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1352.jpg",
		"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1352.jpg",
		"album": "Black Gold",
		"year": 1993,
		"youtube": "NRtvqT_wMeY",
		"bol": "https:\/\/partnerprogramma.bol.com\/click\/click?p=1\u0026s=1687\u0026t=s\u0026f=SBX\u0026sec=music\u0026st=Soul Asylum\u0026name=radio_veronica\u0026subid=player",
		"itunes": "https:\/\/itunes.apple.com\/nl\/album\/runaway-train\/id597541213?i=597545530\u0026uo=4",
		"twitter": "https:\/\/twitter.com\/intent\/tweet?text=Ik+luister+naar+%27Runaway+Train%27+van+Soul+Asylum+bij+%40radioveronica%3A+http%3A%2F%2Fwww.radioveronica.nl",
		"facebook": "https:\/\/www.facebook.com\/dialog\/share?app_id=354799163951\u0026display=popup\u0026href=http%3A%2F%2Fwww.radioveronica.nl%2Fluister%2Fstations%2Fradio-veronica\u0026redirect_uri=http%3A%2F%2Fwww.radioveronica.nl\u0026description=Ik luister nu naar \u0027Runaway Train\u0027 van Soul Asylum",
		"share": "Ik luister nu naar Runaway Train van Soul Asylum bij Radio Veronica. Luister ook! http:\/\/www.radioveronica.nl",
		"shareLink": "http:\/\/www.radioveronica.nl\/luister\/stations\/radio-veronica"
	},
	"next": [{
			"type": "track",
			"id": "6362",
			"startTime": "2017-01-25T04:53:58+01:00",
			"duration": 212218000,
			"title": "Deeper Underground",
			"artist": "Jamiroquai",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6362.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6362.jpg"
		}, {
			"type": "track",
			"id": "1930",
			"startTime": "2017-01-25T04:58:47+01:00",
			"duration": 228075000,
			"title": "Rolling In The Deep",
			"artist": "Adele",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1930.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_1930.jpg"
		}, {
			"type": "track",
			"id": "2682",
			"startTime": "2017-01-25T05:02:33+01:00",
			"duration": 228440000,
			"title": "I Can\u0027t Dance",
			"artist": "Genesis",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_2682.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_2682.jpg"
		}, {
			"type": "track",
			"id": "6783",
			"startTime": "2017-01-25T05:06:15+01:00",
			"duration": 232437000,
			"title": "Kryptonite",
			"artist": "3 Doors Down",
			"image": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6783.jpg",
			"image2": "http:\/\/www.radioveronica.nl\/cdn\/radioveronica_player_600x600_6783.jpg"
		}
	],
	"processtime": "2017-01-25T09:53:54"
}
*/
