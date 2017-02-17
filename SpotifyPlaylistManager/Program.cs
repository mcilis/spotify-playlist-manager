using System;
using System.Threading.Tasks;
using SpotifyPlaylistManager.Models;
using SpotifyPlaylistManager.Sources;

namespace SpotifyPlaylistManager
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            FileHelper.LogTrace("Program.MainAsync() - SpotifyPlaylistManager started!");

            var eksenLivePlaylist = await GetPlaylist("Eksen Live");
            await Spotify.RemovePlaylistTracksAsync(eksenLivePlaylist);

            var eksenPlaylist = await GetPlaylist($"Eksen {DateTime.Now.ToString("MMMM yyyy")}");
            var redPlaylist = await GetPlaylist($"Red {DateTime.Now.ToString("MMMM yyyy")}");
            var veronicaPlaylist = await GetPlaylist($"Veronica {DateTime.Now.ToString("MMMM yyyy")}");
            var veronicaRockPlaylist = await GetPlaylist($"Veronica Rock {DateTime.Now.ToString("MMMM yyyy")}");
            var odtuPlaylist = await GetPlaylist($"Radyo ODTU {DateTime.Now.ToString("MMMM yyyy")}");
            var joyTurkRockPlaylist = await GetPlaylist($"Joy Turk Rock {DateTime.Now.ToString("MMMM yyyy")}");
            var joyFmPlaylist = await GetPlaylist($"Joy {DateTime.Now.ToString("MMMM yyyy")}");

            Song previousEksenSong = null;
            while (true)
            {
                var eksenSong = await Eksen.GetCurrentSongAsync();

                if (previousEksenSong == null || previousEksenSong.TrackName != eksenSong.TrackName)
                {
                    await AddSongToLivePlaylist(eksenLivePlaylist, eksenSong);
                    previousEksenSong = eksenSong;
                }

                await AddSongToPlaylist(eksenPlaylist, eksenSong);

                await AddSongToPlaylist(redPlaylist, await RedFm.GetCurrentSongAsync());

                await AddSongToPlaylist(veronicaPlaylist, await Veronica.GetCurrentSongAsync());

                await AddSongToPlaylist(veronicaRockPlaylist, await VeronicaRock.GetCurrentSongAsync());

                await AddSongToPlaylist(odtuPlaylist, await Odtu.GetCurrentSongAsync());

                await AddSongToPlaylist(joyTurkRockPlaylist, await JoyTurkRock.GetCurrentSongAsync());

                await AddSongToPlaylist(joyFmPlaylist, await JoyFm.GetCurrentSongAsync());

                if (DateTime.Now.Hour >= 20)
                    return; 

                await Task.Delay(240000); // wait 4 minutes for the new song
            }
        }

        static async Task<Playlist> GetPlaylist(string playlistName)
        {
            FileHelper.LogTrace($"Program.GetPlaylist({playlistName})");

            var playlist = await Spotify.FindPlaylistAsync(playlistName);
            if (playlist == null)
            {
                // create playlist
                playlist = await Spotify.CreatePlaylistAsync(playlistName);
                if (playlist == null)
                {
                    FileHelper.LogError("GetPlaylist - Playlist could not be created!");
                }
            }
            return playlist;
        }

        static async Task AddSongToPlaylist(Playlist playlist, Song song)
        {
            if (playlist == null || song == null)
                return;
                        
            if (!FileHelper.FileContainsText(playlist.Name, $"{song.Artist} - {song.TrackName}"))
            {
                // add track to playlist
                var track = await Spotify.SearchForATrackAsync(song.TrackName, song.Artist);

                if (track == null)
                {
                    FileHelper.AddTextToFile(playlist.Name, $"{song.Artist} - {song.TrackName} : [SpotifyFileNotFound!]");
                    FileHelper.LogTrace($"Program.AddSongToPlaylist({playlist.Name}, {song.Artist} - {song.TrackName}) - Spotify file not found!");
                }
                else
                {
                    if (await Spotify.AddTrackToPlaylistAsync(playlist, track))
                    {
                        FileHelper.AddTextToFile(playlist.Name, $"{song.Artist} - {song.TrackName} : [{track.Id}]");
                    }
                    else
                    {
                        FileHelper.AddTextToFile(playlist.Name, $"{song.Artist} - {song.TrackName} : [SpotifyTrackAddFailed!]");
                        FileHelper.LogTrace($"Program.AddSongToPlaylist({playlist.Name}, {song.Artist} - {song.TrackName}) - Spotify track add failed!");
                    }
                }
            }
            else
            {
                FileHelper.LogTrace($"Program.AddSongToPlaylist({playlist.Name}, {song.Artist} - {song.TrackName}) - Playlist contains song!");
            }

        }

        static async Task AddSongToLivePlaylist(Playlist playlist, Song song)
        {
            if (playlist == null || song == null)
                return;

            var track = await Spotify.SearchForATrackAsync(song.TrackName, song.Artist);

            if (track != null)
                await Spotify.AddTrackToPlaylistAsync(playlist, track);
        }

    }
}
