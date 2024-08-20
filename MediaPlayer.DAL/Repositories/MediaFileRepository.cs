using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories.IRepository;

namespace MediaPlayer.DAL.Repositories
{
    public class MediaFileRepository : IRepository<MediaFile>
    {
        private MediaPlayerDBContext _context;
        public void Add(MediaFile entity)
        {
            _context = new MediaPlayerDBContext();
            _context.MediaFiles.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(MediaFile entity)
        {

        }

        public IEnumerable<MediaFile> GetAll()
        {
            _context = new MediaPlayerDBContext();
            return _context.MediaFiles.ToList();
        }

        public MediaFile GetById(int id)
        {
            _context = new();
            return _context.MediaFiles.Find(id);
        }
        public void UpdateLastPlayedAt(int id, bool featureStatus)
        {
            _context = new MediaPlayerDBContext();
            MediaFile mediaFile = _context.MediaFiles.Find(id);
            if (featureStatus)
                mediaFile.LastPlayedAt = DateTime.Now;
            else mediaFile.LastPlayedAt = null;
            _context.SaveChanges();
        }
        public void Update(MediaFile entity)
        {
            throw new NotImplementedException();
        }

        //get media file by filepath
        public MediaFile GetMediaFile(string filepath)
        {
            return _context.MediaFiles.FirstOrDefault(file => file.FilePath == filepath);
        }
    }
}
