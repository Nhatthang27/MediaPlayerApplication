namespace MediaPlayer.DAL.Entities
{
    public class PlaylistItem
    {
        public int PlaylistItemId { get; set; }  // Khóa chính

        public int MediaFileId { get; set; }  // Khóa ngoại tới MediaFile
        public MediaFile MediaFile { get; set; }  // Navigation property tới MediaFile

        public int PlaylistId { get; set; }  // Khóa ngoại tới Playlist
        public Playlist Playlist { get; set; }  // Navigation property tới Playlist

        public int Order { get; set; }  // Thứ tự trong playlist
    }

}
