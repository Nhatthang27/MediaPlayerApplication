using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
	public class PlayQueueService
	{
		public List<MediaFile> PlayQueue { get; set; }

        public void Add(MediaFile mediaFile)
        {
            if (PlayQueue == null)
                PlayQueue = new List<MediaFile>();
            PlayQueue.Add(mediaFile);
        }

        public void AddPriority(MediaFile mediaFile)
        {
            if (PlayQueue == null)
                PlayQueue = new List<MediaFile>();
            //add at the beginning of PlayQueue
            PlayQueue.Insert(0, mediaFile);
        }

    }
}
