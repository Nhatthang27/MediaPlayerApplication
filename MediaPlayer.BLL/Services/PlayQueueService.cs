using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
    public class PlayQueueService
    {
        public List<MediaFile> PlayQueue { get; set; }

        public Stack<MediaFile> PlayedStack { get; set; }

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
            //add at the beginning of PlayQueue
            PlayQueue.Insert(0, mediaFile);
        }

        public void Remove(MediaFile mediaFile)
        {
            //remove by id
            PlayQueue.Remove(mediaFile);
        }

        public void RemoveAt(int indexQueue)
        {
            PlayQueue.RemoveAt(indexQueue);
        }

        public void PushToStack(MediaFile mediaFile)
        {
            if (PlayedStack == null)
                PlayedStack = new Stack<MediaFile>();
            PlayedStack.Push(mediaFile);
        }

        public MediaFile PopFromStack()
        {
            if (PlayedStack != null && PlayedStack.Count > 0)
                return PlayedStack.Pop();
            else
                return null;
        }
    }
}
