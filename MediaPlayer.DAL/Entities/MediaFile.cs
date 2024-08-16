using System.ComponentModel.DataAnnotations;

namespace MediaPlayer.DAL.Entities
{
    public class MediaFile
    {
        public int MediaFileId { get; set; }  // Khóa chính

        [MaxLength(255)]
        public string FileName { get; set; }  // Tên tệp

        [MaxLength(200)]
        public string FilePath { get; set; }  // Đường dẫn tệp

        public TimeSpan Duration { get; set; }  // Thời lượng của tệp

        [MaxLength(255)]
        public string Title { get; set; }  // Tiêu đề của tệp

        [MaxLength(255)]
        public string Artists { get; set; }  // Nghệ sĩ liên quan

        public DateTime CreatedAt { get; set; }  // Ngày tạo

        public DateTime? LastPlayedAt { get; set; }  // Lần cuối phát (nullable)

        // Navigation property
        public ICollection<PlaylistItem> PlaylistItems { get; set; }  // Các mục liên kết với PlaylistItem
    }
}
