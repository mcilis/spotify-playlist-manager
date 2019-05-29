using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyPlaylistManager.Models;

namespace SpotifyPlaylistManager
{
    public static class Spotify
    {
        static string _key;
        static ClientCredentials _clientCredentials;

        static Spotify()
        {
            var clientId = ConfigurationManager.AppSettings["ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
            _key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
        }

        public static async Task<string> InitialConfiguration()
        {
            // Use: GetSpotifyAccessTokenCode.html

            var token = await GetAccessTokenAsync();

            return token;
        }


        public static async Task<Track> SearchForATrackAsync(string trackName, string artistName)
        {
            // curl -X GET "https://api.spotify.com/v1/search?q=Belfast+Child++Simple+Minds&type=track&market=TR&limit=1" -H "Accept: application/json"

            if (string.IsNullOrWhiteSpace(trackName) || string.IsNullOrWhiteSpace(artistName))
                return null;

            var query = $"{trackName.Trim().Replace(" ", "+")}+{artistName.Trim().Replace(" ", "+")}";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/search?q={query}&type=track&market=TR"),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                                
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    try
                    {
                        var spotifySearch = JObject.Parse(responseContent);
                        IList<JToken> results = spotifySearch["tracks"]["items"].Children().ToList();
                        IList<Track> trackResults = new List<Track>();
                        foreach (JToken result in results)
                        {
                            Track trackResult = JsonConvert.DeserializeObject<Track>(result.ToString());
                            trackResults.Add(trackResult);
                        }

                        return trackResults.OrderByDescending(x => x.Popularity).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        FileHelper.LogError($"Spotify.SearchForATrackAsync \n Error: {exception.Message} \n {exception.InnerException?.Message} \n Query: {query}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    FileHelper.LogError($"Spotify.SearchForATrackAsync \n Error: Unauthorized! Check for refresh token... \n Query: {query}");
                }
            }
            return null;
        }

        public static async Task<List<Playlist>> GetPlaylistsAsync(string ownerId = "mcilis")
        {
            // curl -X GET "https://api.spotify.com/v1/users/mcilis/playlists" -H "Accept: application/json" -H "Authorization: Bearer BQAeoomBlUeG..."

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists"),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var spotifyPlaylist = JObject.Parse(responseContent);
                    IList<JToken> results = spotifyPlaylist["items"].Children().ToList();
                    IList<Playlist> playlistResults = new List<Playlist>();
                    foreach (JToken result in results)
                    {
                        Playlist playlistResult = JsonConvert.DeserializeObject<Playlist>(result.ToString());
                        playlistResult.TracksHref = result["tracks"]["href"].ToString();
                        playlistResult.TracksCount = result["tracks"]["total"].Value<int>();
                        playlistResults.Add(playlistResult);
                    }
                    return playlistResults.ToList();
                }
                return null;
            }
        }

        public static async Task<Playlist> FindPlaylistAsync(string playlistName, string ownerId = "mcilis")
        {
            // curl -X GET "https://api.spotify.com/v1/users/mcilis/playlists" -H "Accept: application/json" -H "Authorization: Bearer BQAeoomBlUeG..."

            if (string.IsNullOrWhiteSpace(playlistName))
                return null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists"),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var spotifyPlaylist = JObject.Parse(responseContent);
                    IList<JToken> results = spotifyPlaylist["items"].Children().ToList();
                    IList<Playlist> playlistResults = new List<Playlist>();
                    foreach (JToken result in results)
                    {
                        Playlist playlistResult = JsonConvert.DeserializeObject<Playlist>(result.ToString());
                        playlistResult.TracksHref = result["tracks"]["href"].ToString();
                        playlistResult.TracksCount = result["tracks"]["total"].Value<int>();
                        playlistResults.Add(playlistResult);
                    }
                    return playlistResults.FirstOrDefault(x => x.Name == playlistName);
                }
                return null;
            }
        }

        public static async Task<Playlist> CreatePlaylistAsync(string playlistName, bool isPublic = true, string ownerId = "mcilis")
        {
            // curl -X POST "https://api.spotify.com/v1/users//playlists" -H "Accept: application/json" -H "Authorization: Bearer BQAeoo..." -H "Content-Type: application/json" --data "{\"name\":\"NewPlaylist\",\"public\":false}"

            if (string.IsNullOrWhiteSpace(playlistName))
                return null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists"),
                    Method = HttpMethod.Post
                };

                var body = JsonConvert.SerializeObject(new { name = playlistName, @public = isPublic }, Formatting.Indented);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return JsonConvert.DeserializeObject<Playlist>(responseContent);
                }
                return null;
            }
        }

        public static async Task<bool> AddTrackToPlaylistAsync(Playlist playlist, Track track, string ownerId = "mcilis")
        {
            // curl -X POST "https://api.spotify.com/v1/users/jmperezperez/playlists/3cEYpjA9oz9GiPac4AsH4n/tracks?position=0&uris=spotify%3Atrack%3A4iV5W9uYEdYUVa79Axb7Rh,spotify%3Atrack%3A1301WleyT98MSxVHPZCA6M" -H "Accept: application/json" -H "Authorization: Bearer BQAeoom..."

            if (playlist == null || track == null)
                return false;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists/{playlist.Id}/tracks?position=0&uris={track.Uri}"),
                    Method = HttpMethod.Post
                };

                var response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();  // {"snapshot_id" : "d+E/vkyoy+hRobCgiCNbsGDeFNGnz1PKzVGOX7kbO12LXllCMb917TOPC36tKbum"}
                    FileHelper.LogError($"AddTrackToPlaylistAsync - add track failed, responseContent:{responseContent}");
                    return false;
                }
            }
        }

        public static async Task GetPlaylistTracksAsync(Playlist playlist, string ownerId = "mcilis")
        {
            // GET https://api.spotify.com/v1/users/{user_id}/playlists/{playlist_id}/tracks

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists/{playlist.Id}/tracks?fields=items(added_at,track(track_number,uri))"),
                    Method = HttpMethod.Get
                };

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var spotifyPlaylist = JObject.Parse(responseContent);
                    IList<JToken> results = spotifyPlaylist["items"].Children().ToList();
                    foreach (JToken result in results)
                    {
                        var addedAt = Convert.ToDateTime(result["added_at"]);
                        var trackNumber = Convert.ToInt16(result["track"]["track_number"]);
                        var trackUri = result["track"]["uri"].ToString();

                    }
                }
            }
        }

        public static async Task<bool> RemovePlaylistTracksAsync(Playlist playlist, string ownerId = "mcilis")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {await GetAccessTokenAsync()}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://api.spotify.com/v1/users/{ownerId}/playlists/{playlist.Id}/tracks"),
                    Method = HttpMethod.Put
                };

                var body = JsonConvert.SerializeObject(new { uris = new[] { "spotify:track:6Z5XZ8fjmWGbkdahOgreVe" } }, Formatting.Indented); // You have to add at least one track!
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
            }
            return false;
        }



        #region PRIVATE METHODS

        static async Task<string> GetAccessTokenAsync()
        {
            if (_clientCredentials != null && _clientCredentials.CreateDate.AddSeconds(_clientCredentials.ExpiresIn - 10) > DateTime.Now)
                return _clientCredentials.AccessToken;

            var mode = ConfigurationManager.AppSettings["AuhorizationMode"];
            switch (mode)
            {
                case "InitialConfiguration":
                    {
                        // Use: GetSpotifyAccessTokenCode.html
                        // Old reference: http://jsfiddle.net/JMPerez/62wafrm7/

                        _clientCredentials = await CreateClientCredentialsAsync();
                        if (_clientCredentials == null)
                        {
                            FileHelper.LogError("GetAccessTokenAsync InitialConfiguration - Retrieving client credentials is failed!");
                            throw new Exception("GetAccessTokenAsync - Retrieving client credentials is failed!");
                        }

                        FileHelper.LogError($"GetAccessTokenAsync \n Write the following refresh token value to the AppSettings: \n RefreshToken=\n\n{_clientCredentials.RefreshToken} \n");
                        // Bu degeri App.config.AppSettings.RefreshToken alanina yaz.
                        break;
                    }
                case "RefreshToken":
                    {
                        _clientCredentials = await RefreshClientCredentialsAsync();
                        if (_clientCredentials == null)
                        {
                            FileHelper.LogError("GetAccessTokenAsync RefreshToken - Retrieving client credentials is failed!");
                            throw new Exception("GetAccessTokenAsync RefreshToken - Retrieving client credentials is failed!");
                        }
                        break;
                    }
                default:
                    FileHelper.LogError("GetAccessTokenAsync - Invalid mode!");
                    throw new Exception("GetAccessTokenAsync - Invalid mode!");
            }

            return _clientCredentials.AccessToken;
        }

        static async Task<ClientCredentials> CreateClientCredentialsAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {_key}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                    Method = HttpMethod.Post
                };

                var code = ConfigurationManager.AppSettings["Code"];
                request.Content = new StringContent($"grant_type=authorization_code&code={code}&redirect_uri=http://jmperezperez.com/spotify-oauth-jsfiddle-proxy/", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<ClientCredentials>(responseContent);
                }
            }
            return null;
        }

        static async Task<ClientCredentials> RefreshClientCredentialsAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {_key}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                    Method = HttpMethod.Post
                };

                var refreshToken = ConfigurationManager.AppSettings["RefreshToken"];
                request.Content = new StringContent($"grant_type=refresh_token&refresh_token={refreshToken}", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<ClientCredentials>(responseContent);
                }
            }
            return null;
        }

        static async Task<ClientCredentials> CreateDefaultClientCredentialsAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {_key}");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://accounts.spotify.com/api/token"),
                    Method = HttpMethod.Post
                };

                request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await client.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ClientCredentials>(responseContent);

                }
            }
            return null;
        }

        #endregion

    }
}
