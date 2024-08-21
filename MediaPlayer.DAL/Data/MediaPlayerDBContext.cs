using Microsoft.EntityFrameworkCore;

using System.Configuration;
namespace MediaPlayer.DAL.Data
{
    public class MediaPlayerDBContext : DbContext
    {
		//private string connectionString = @"Initial Catalog=MediaPlayerDB;Integrated Security=True; Trusted_Connection=true;Encrypt=false";
		//private string connectionString = @"Database=MediaPlayer;Trusted_Connection=True;TrustServerCertificate=True";
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
				optionsBuilder.UseSqlServer(connectionString);
			}
		}
		private readonly string _connectionString;

		//public MediaPlayerDBContext()
		//{
		//	// Load configuration from appsettings.json
		//	var builder = new ConfigurationBuilder()
		//		.SetBasePath(Directory.GetCurrentDirectory())
		//		.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
		//	var configuration = builder.Build();

		//	_connectionString = configuration.GetConnectionString("DefaultConnection");
		//}
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	if (!optionsBuilder.IsConfigured)
		//		optionsBuilder.UseSqlServer(_connectionString);
		//}
		public DbSet<Entities.MediaFile> MediaFiles { get; set; }
        public DbSet<Entities.Playlist> Playlists { get; set; }
        public DbSet<Entities.PlaylistItem> PlaylistItems { get; set; }

        
    }
}
