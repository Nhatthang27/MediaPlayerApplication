using MediaPlayer.DAL.Entities;

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
            throw new NotImplementedException();
        }

        public void UpdateLastPlayedAt(MediaFile mediaFile, bool featureStatus)
        {
            if (featureStatus)
                mediaFile.LastPlayedAt = DateTime.Now;
            else mediaFile.LastPlayedAt = null;
            _context.SaveChanges();
        }
        public void Update(MediaFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
