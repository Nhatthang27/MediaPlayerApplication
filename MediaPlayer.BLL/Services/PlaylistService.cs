using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories;

namespace MediaPlayer.BLL.Services
{
    public class PlaylistService
    {
        private PlaylistRepository _playlistRepo = new PlaylistRepository();

        public void addPlaylist(Playlist playlist)
        {
            _playlistRepo.Add(playlist);
        }

        public IEnumerable<Playlist> GetAllPlaylist()
        {
            return _playlistRepo.GetAll();
        }

        public void Remove(Playlist playlist)
        {
            _playlistRepo.Delete(playlist);
        }

        public void UpdateName(string name, int playlistId)
        {
            _playlistRepo.UpdateName(name, playlistId);
        }
    }
}
