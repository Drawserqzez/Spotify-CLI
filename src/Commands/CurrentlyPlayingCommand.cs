using System;
using System.Collections.Generic;
using Draws.CLI;
using SpotifyAPI.Web;

namespace SpotifyCLI.Commands {
    [Command("playing", "Displays the currently playing song", isSingleArgument: true)]
    public class CurrentlyPlayingCommand : ICommand {
        private readonly ISpotifyClient _spotifyClient;

        public CurrentlyPlayingCommand(ISpotifyClient spotifyClient) {
            _spotifyClient = spotifyClient;
        }

        public string RunCommand() {
            try {
                string currentlyPlayingString = "Something went wrong with your request";
                
                var currentlyPlayingTask = _spotifyClient.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest());
                currentlyPlayingTask.Wait();

                var currentlyPlaying = currentlyPlayingTask.Result;

                if (currentlyPlaying.Item is FullTrack) {
                    var track = currentlyPlaying.Item as FullTrack;
                    string artistNames = "";
                    foreach (var artist in track.Artists)
                        artistNames += artist.Name + ", ";

                    currentlyPlayingString = $"Currently playing: {track.Name} by {artistNames.TrimEnd(',', ' ')}";
                }
                else if (currentlyPlaying.Item is FullEpisode) {
                    var episode = currentlyPlaying.Item as FullEpisode;
                    currentlyPlayingString = $"Currently playing {episode.Name} by {episode.Show.Name}";
                }


                return currentlyPlayingString;
            } catch (Exception e) {
                return e.Message;
            }
        }

        public void SetArguments(Dictionary<string, string> args) {

        }
    }
}