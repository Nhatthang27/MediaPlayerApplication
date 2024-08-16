using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
    public class PlayQueueService
    {
        public List<MediaFile> PlayQueue { get; set; }

        public void AddASong(MediaFile song) => PlayQueue.Add(song);

    }
}
