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
            using (_context = new())
            {
                _context.PlaylistItems.Remove(entity);
            }
        }

        public void Delete(int mediaId, int playlistId)
        {
            using (_context = new())
            {
                // Find the entity that matches the specified mediaId and playlistId
                var entity = _context.PlaylistItems
                    .FirstOrDefault(item => item.MediaFileId == mediaId && item.PlaylistId == playlistId);

                if (entity != null)
                {
                    // Remove the entity from the context
                    _context.PlaylistItems.Remove(entity);
                    // Save changes to the database
                    _context.SaveChanges();

                    var remainingSongs = _context.PlaylistItems
                                        .Where(item => item.PlaylistId == playlistId)
                                        .OrderBy(item => item.Order) // Assuming 'Order' is the property that holds the song order
                                        .ToList();

                    // Reassign the order of the remaining songs
                    for (int i = 0; i < remainingSongs.Count; i++)
                    {
                        remainingSongs[i].Order = i + 1; // Set the new order starting from 1
                    }

                    // Save the updated orders to the database
                    _context.SaveChanges();
                }
            }
        }

        public void DeletePlaylist(int playlistId)
        {
            using (_context = new())
            {
                var playlistItems = _context.PlaylistItems.Where(pi => pi.PlaylistId == playlistId).ToList();

                if (playlistItems.Any())
                {
                    _context.PlaylistItems.RemoveRange(playlistItems);

                    _context.SaveChanges();
                }
            }
        }



        public IEnumerable<PlaylistItem> GetAll()
        {
            _context = new();
            return _context.PlaylistItems.ToList();
        }
        public IEnumerable<MediaFile> GetMediaFilesByPlaylistID(int playlistId)
        {
            using (_context = new MediaPlayerDBContext())
            {
                // Retrieve MediaFiles associated with the given PlaylistId
                return _context.PlaylistItems
                              .Where(pi => pi.PlaylistId == playlistId) // Filter by PlaylistId
                              .OrderBy(pi => pi.Order) // Sort by Order
                              .Select(pi => pi.MediaFile) // Select MediaFile
                              .ToList(); // Convert to list
            }
        }
		public int CountMediaFiles(int playlistId)
		{
			using (_context = new MediaPlayerDBContext())
			{
				// Count MediaFiles associated with the given PlaylistId
				return _context.PlaylistItems
							  .Where(pi => pi.PlaylistId == playlistId)
							  .Count();
			}
		}

		public void AddSongToPlaylist(int playlistId, int songId)
        {
            using (var _context = new MediaPlayerDBContext()) // Assuming MediaPlayerContext is your DbContext
            {
                // Step 1: Retrieve the current max order in the playlist
                var playlistItems = _context.PlaylistItems
                    .Where(pi => pi.PlaylistId == playlistId)
                    .Select(pi => pi.Order)
                    .ToList(); // Fetch data into memory

                var maxOrder = playlistItems.DefaultIfEmpty(0).Max(); // Perform aggregation in memory

                // Step 2: Create a new PlaylistItem with the next available order
                var newPlaylistItem = new PlaylistItem
                {
                    PlaylistId = playlistId,
                    MediaFileId = songId,
                    Order = maxOrder + 1 // Next available order
                };

                // Step 3: Add the new PlaylistItem to the context and save changes
                _context.PlaylistItems.Add(newPlaylistItem);
                _context.SaveChanges();
            }
        }
        public PlaylistItem GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PlaylistItem entity)
        {
            throw new NotImplementedException();
        }
		//public IEnumerable<MediaFile> GetMediaFilesByPlaylistID(int playlistId)
		//{
		//	using (var context = new MediaPlayerDBContext())
		//	{
		//		// Retrieve MediaFiles associated with the given PlaylistId
		//		return context.PlaylistItems
		//					  .Where(pi => pi.PlaylistId == playlistId) // Filter by PlaylistId
		//					  .OrderBy(pi => pi.Order) // Sort by Order
		//					  .Select(pi => pi.MediaFile) // Select MediaFile
		//					  .ToList(); // Convert to list
		//	}
		//}

	}
}
