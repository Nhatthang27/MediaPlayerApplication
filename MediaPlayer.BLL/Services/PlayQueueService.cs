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

        //add a media file with priority
        public void AddPriority(MediaFile mediaFile)
        {
            if (PlayQueue == null)
                PlayQueue = new List<MediaFile>();
            PlayQueue.Insert(0, mediaFile);
        }

        public void Remove(MediaFile mediaFile)
        {
            PlayQueue.Remove(mediaFile);
        }

        public void RemoveAt(int indexQueue)
        {
            if (PlayQueue != null && PlayQueue.Count > indexQueue)
                PlayQueue.RemoveAt(indexQueue);
        }

        public int GetIndexInQueue(MediaFile mediaFile)
        {
            return PlayQueue.IndexOf(mediaFile);
        }

        public bool IsEmpty()
        {
            return PlayQueue == null || PlayQueue.Count == 0;
        }


        public void Shuffle()
        {
            if (PlayQueue != null && PlayQueue.Count > 1)
            {
                Random random = new Random();
                //shuffle the list except first element
                for (int i = 1; i < PlayQueue.Count; i++)
                {
                    int j = random.Next(i, PlayQueue.Count);
                    MediaFile temp = PlayQueue[i];
                    PlayQueue[i] = PlayQueue[j];
                    PlayQueue[j] = temp;
                }
            }
        }
    }
}
