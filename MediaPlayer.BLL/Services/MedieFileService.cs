using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories;

namespace MediaPlayer.BLL.Services
{
    public class MedieFileService
    {
        private MediaFileRepository _mediaFileRepo = new MediaFileRepository();

        //get all media files
        public IEnumerable<MediaFile> GetAllMediaFiles()
        {
            return _mediaFileRepo.GetAll();
        }

        //add a media file
        public void AddMediaFile(MediaFile mediaFile)
        {
            _mediaFileRepo.Add(mediaFile);
        }
    }
}
