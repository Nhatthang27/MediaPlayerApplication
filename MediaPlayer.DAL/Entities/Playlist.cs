using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.DAL.Entities
{
    public class Playlist
    {
        public int PlaylistId { get; set; }  // Khóa chính

        [MaxLength(255)]
        public string Title { get; set; }  // Tên playlist

        public DateTime CreatedAt { get; set; }  // Ngày tạo

        // Navigation property
        public ICollection<PlaylistItem> PlaylistItems { get; set; }  // Các mục liên kết với PlaylistItem
    }

}
