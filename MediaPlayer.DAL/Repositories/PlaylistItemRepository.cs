using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories.IRepository;

namespace MediaPlayer.DAL.Repositories
{
    public class PlaylistItemRepository : IRepository<PlaylistItem>
    {
        private MediaPlayerDBContext _context;
        public void Add(PlaylistItem entity)
        {
            _context = new MediaPlayerDBContext();
            _context.Add(entity);
            _context.SaveChanges();
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
		public IEnumerable<MediaFile> GetMediaFilesByPlaylistID(int playlistId)
		{
			using (var context = new MediaPlayerDBContext())
			{
				// Retrieve MediaFiles associated with the given PlaylistId
				return context.PlaylistItems
							  .Where(pi => pi.PlaylistId == playlistId) // Filter by PlaylistId
							  .OrderBy(pi => pi.Order) // Sort by Order
							  .Select(pi => pi.MediaFile) // Select MediaFile
							  .ToList(); // Convert to list
			}
		}

	}
}
