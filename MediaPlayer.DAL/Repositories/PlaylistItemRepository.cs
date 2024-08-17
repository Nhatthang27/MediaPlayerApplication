using MediaPlayer.DAL.Data;
using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories.IRepository;

namespace MediaPlayer.DAL.Repositories
{
    public class PlaylistItemRepository : IRepository<PlaylistItem>
    {
        private MediaPlayerDBContext _context;
        public void Add(PlaylistItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(PlaylistItem entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlaylistItem> GetAll()
        {
            _context = new MediaPlayerDBContext();
            return _context.PlaylistItems.ToList();
        }

        public PlaylistItem GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PlaylistItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
