using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
    public class PlayQueueService
    {
        public List<MediaFile> PlayQueue { get; set; }

        public void AddASong(MediaFile song)
        {
            if (PlayQueue == null)
                PlayQueue = new List<MediaFile>();
            PlayQueue.Add(song);
        }

    }
}
