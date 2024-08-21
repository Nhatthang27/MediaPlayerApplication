using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL.Services
{
    public class PlayStackService
    {
        private Stack<MediaFile> _playStack;

        public void PushToStack(MediaFile mediaPlayer)
        {
            if (_playStack == null)
                _playStack = new Stack<MediaFile>();
            _playStack.Push(mediaPlayer);
        }

        public MediaFile PopFromStack()
        {
            if (!IsStackEmpty())
                return _playStack.Pop();
            else
                return null;
        }

        public bool IsStackEmpty()
        {
            return _playStack == null || _playStack.Count == 0;
        }
    }
}
