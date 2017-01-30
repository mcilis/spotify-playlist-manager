using System;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using System.Configuration;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager.Sources
{
    public class Odtu
    {
        public static async Task<Song> GetCurrentSongAsync()
        {
            // <div id="rep_now_playing_artist">Delta Goodrem</div> <div id="rep_now_playing_song">The River</div>            

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(ConfigurationManager.AppSettings["Source.Odtu"]),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK && responseContent.Contains("rep_now_playing_song"))
                {
                    try
                    {
                        var parser = new HtmlParser();
                        var document = parser.Parse(responseContent);

                        return new Song
                        {
                            Artist = document.QuerySelector("#rep_now_playing_artist").TextContent.Trim(),
                            TrackName = document.QuerySelector("#rep_now_playing_song").TextContent.Trim()
                        };
                    }
                    catch (Exception exception)
                    {
                        FileHelper.AddTextToFile("SpotifyPlaylistManager", $"Odtu.GetCurrentSongAsync - Error: {exception.Message} Content: {responseContent}");
                    }
                }
            }
            return null;
        }
    }
}

/*
<img id="rep_now_playing_album_art_img_s" src="https://lastfm-img2.akamaized.net/i/u/64s/b84e8e0d4998a54d91141fec782d14ec.png"/>
<img id="rep_now_playing_album_art_img_m" src="https://lastfm-img2.akamaized.net/i/u/174s/b84e8e0d4998a54d91141fec782d14ec.png"/>
<div id="rep_now_playing_artist">Delta Goodrem</div>
<div id="rep_now_playing_song">The River</div>
<div id="rep_recently_played_song_1_artist">Sam Smith</div>
<div id="rep_recently_played_song_1_song">I'm Not The Only One</div>
<img id="rep_recently_played_album_art_img_1" src="https://lastfm-img2.akamaized.net/i/u/64s/c1031a30358544dcc490bf49890cce79.png"/>
<div id="rep_recently_played_song_2_artist">Audioslave</div>
<div id="rep_recently_played_song_2_song">Like A Stone</div>
<img id="rep_recently_played_album_art_img_2" src="https://lastfm-img2.akamaized.net/i/u/64s/ed071004b3a64afb8b2a8397aad1bed4.png"/>
<div id="rep_recently_played_song_3_artist">Dire Straits</div>
<div id="rep_recently_played_song_3_song">Sultans Of Swing</div>
<img id="rep_recently_played_album_art_img_3" src="https://lastfm-img2.akamaized.net/i/u/64s/cc8218235b6f438dbddd294d9022880e.png"/>
<div id="rep_recently_played_song_4_artist">Eric Clapton</div>
<div id="rep_recently_played_song_4_song">Layla (Unplugged)</div>
<img id="rep_recently_played_album_art_img_4" src="https://lastfm-img2.akamaized.net/i/u/64s/64c3d1900527465c9865fde99c6bcb3c.png"/>
<div id="rep_recently_played_song_5_artist">Lionel Richie</div>
<div id="rep_recently_played_song_5_song">Hello</div>
<img id="rep_recently_played_album_art_img_5" src="https://lastfm-img2.akamaized.net/i/u/64s/ba2021c7eeca4aeba3880cfce3af075b.png"/>  
*/
