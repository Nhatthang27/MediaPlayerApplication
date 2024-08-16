using MediaPlayer.DAL.Entities;

namespace MediaPlayer.BLL
{
    public class Utils
    {
        public const string COMMON_MEDIAFILE = "Media files (*.mp3;*.wav;*.flac;*.aac;*.ogg;*.wma;*.m4a;*.mp4;*.avi;*.mkv;*.mov;*.flv;*.wmv;*.webm)|*.mp3;*.wav;*.flac;*.aac;*.ogg;*.wma;*.m4a;*.mp4;*.avi;*.mkv;*.mov;*.flv;*.wmv;*.webm|All files (*.*)|*.*";
        public static MediaFile GetPropertiesFromFilePath(string filePath)
        {
            TagLib.File tagFile = TagLib.File.Create(filePath);

            string fileName = Path.GetFileName(filePath);
            TimeSpan duration = tagFile.Properties.Duration;
            string title = tagFile.Tag.Title ?? "Untitled";
            string artists = tagFile.Tag.Artists?.Length > 0 ? tagFile.Tag.Artists[0] : "Unknown";
            DateTime createdAt = DateTime.Now;


            // Định dạng duration chỉ lấy giờ, phút, giây
            string formattedDuration = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);
            return new MediaFile() { FileName = fileName, Duration = TimeSpan.Parse(formattedDuration), Title = title, Artists = artists, CreatedAt = createdAt, FilePath = filePath };
        }

    }
}
