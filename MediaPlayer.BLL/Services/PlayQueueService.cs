using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
    public class PlayQueueService
    {
        public List<MediaFile> PlayQueue { get; set; }

        public void AddAMediaFile(MediaFile mediaFile)
        {
            if (PlayQueue == null)
                PlayQueue = new List<MediaFile>();
            PlayQueue.Add(mediaFile);
        }

    }
}
