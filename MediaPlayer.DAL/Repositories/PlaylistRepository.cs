using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories.IRepository;

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

        public void UpdateName(string name, int playlistId)
        {
            using (_context = new())
            {
                // Find the playlist entity by its ID
                var playlist = _context.Playlists
                    .FirstOrDefault(p => p.PlaylistId == playlistId);

                if (playlist != null)
                {
                    // Update the name
                    playlist.Title = name;

                    // Save changes to the database
                    _context.SaveChanges();
                }
                else
                {
                    // Handle the case when the playlist is not found
                    throw new Exception("Playlist not found.");
                }
            }
        }
    }

}
