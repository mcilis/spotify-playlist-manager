using Newtonsoft.Json;

namespace SpotifyPlaylistManager.Models
{
    public class Playlist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        public string TracksHref { get; set; }

        public int TracksCount { get; set; }

    }
}
