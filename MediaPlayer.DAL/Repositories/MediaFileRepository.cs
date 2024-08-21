using MediaPlayer.DAL.Data;
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
            throw new NotImplementedException();
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

        public void Update(MediaFile entity)
        {
            throw new NotImplementedException();
        }
    }
}
