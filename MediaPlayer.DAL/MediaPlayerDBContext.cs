using Microsoft.EntityFrameworkCore;
namespace MediaPlayer.DAL
{
    public class MediaPlayerDBContext : DbContext
    {
        private string connectionString = @"Initial Catalog=MediaPlayerDB;Integrated Security=True;Trusted_Connection=true;Encrypt=false";
        public DbSet<Entities.MediaFile> MediaFiles { get; set; }
        public DbSet<Entities.Playlist> Playlists { get; set; }
        public DbSet<Entities.PlaylistItem> PlaylistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
