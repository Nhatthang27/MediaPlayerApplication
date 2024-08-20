using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
using System.Configuration;
namespace MediaPlayer.DAL
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
		//protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		//{
		//	optionsBuilder.UseSqlServer(GetConnectionString());
		//}
		//private string GetConnectionString()
		//{
		//	IConfiguration config = new ConfigurationBuilder()
		//		 .SetBasePath(Directory.GetCurrentDirectory())
		//				.AddJsonFile("appsettings.json", true, true)
		//				.Build();
		//	var strConn = config["ConnectionStrings:DefaultConnectionString"];

		//	return strConn;
		//}



		public DbSet<Entities.MediaFile> MediaFiles { get; set; }
        public DbSet<Entities.Playlist> Playlists { get; set; }
        public DbSet<Entities.PlaylistItem> PlaylistItems { get; set; }


    }
}
