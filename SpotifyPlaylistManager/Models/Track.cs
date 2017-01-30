using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpotifyPlaylistManager.Models
{
    public class Track
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("artists")]
        public List<Artist> Artists { get; set; }
    }
}
