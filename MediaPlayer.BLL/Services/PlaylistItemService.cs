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
    }
}
