using Newtonsoft.Json;

namespace SpotifyPlaylistManager.Models
{
    public class Song
    {
        [JsonProperty("PlayDate")]
        public string PlayDate { get; set; }

        [JsonProperty("Artist")]
        public string Artist { get; set; }

        [JsonProperty("TrackName")]
        public string TrackName { get; set; }

        [JsonProperty("CoverPath")]
        public string CoverPath { get; set; }
    }
}
