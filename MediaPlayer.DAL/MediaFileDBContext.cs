using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace MediaPlayer.DAL
{
    public class MediaPlayerDBContext : DbContext
    {
        public DbSet<Entities.MediaFile> MediaFiles { get; set; }
        public DbSet<Entities.Playlist> Playlists { get; set; }
        public DbSet<Entities.PlaylistItem> PlaylistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();

            var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];
            return strConn;
        }
    }
}
