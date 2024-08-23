using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.BLL.Services
{
    public class PlaylistItemService
    {
        private PlaylistItemRepository _playlistItemsRepo = new PlaylistItemRepository();

        public IEnumerable<MediaFile> GetMediaFilesByPlaylistID(int playlistId)
        {
            return _playlistItemsRepo.GetMediaFilesByPlaylistID(playlistId).ToList();
        }

        public void Add(int playlistId, int songId)
        {
            _playlistItemsRepo.AddSongToPlaylist(playlistId, songId);
        }

        public void DeletePlaylist(int playlistId)
        {
            _playlistItemsRepo.DeletePlaylist(playlistId);
        }

        public void Delete(int mediaId, int playlistId)
        {
            _playlistItemsRepo.Delete(mediaId, playlistId);
        }
        public int CountMediaFiles(int playlistId)
        {
            return _playlistItemsRepo.CountMediaFiles(playlistId);
        }

	}
}
