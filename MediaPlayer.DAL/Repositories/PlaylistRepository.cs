using MediaPlayer.DAL.Data;
using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories.IRepository;

namespace MediaPlayer.DAL.Repositories
{
    public class PlaylistRepository : IRepository<Playlist>
    {
        private MediaPlayerDBContext _context;

        public void Add(Playlist entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Playlist entity)
        {
            throw new NotImplementedException();
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
