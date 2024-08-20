using MediaPlayer.DAL.Entities;

namespace MediaPlayer.DAL.Repositories
{
    public class PlaylistRepository : IRepository<Playlist>
    {
        private MediaPlayerDBContext _context;

        public void Add(Playlist entity)
        {
            _context = new MediaPlayerDBContext();
            _context.Playlists.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Playlist entity)
        {
            _context = new MediaPlayerDBContext();
            _context.Playlists.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<Playlist> GetAll()
        {
            _context = new MediaPlayerDBContext();
            return _context.Playlists.ToList();
        }

        public Playlist GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Playlist entity)
        {
            throw new NotImplementedException();
        }
    }

}
