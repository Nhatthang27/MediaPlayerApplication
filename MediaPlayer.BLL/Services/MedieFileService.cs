using MediaPlayer.DAL.Entities;
using MediaPlayer.DAL.Repositories;

namespace MediaPlayer.BLL.Services
{
    public class MedieFileService
    {
        private MediaFileRepository _mediaFileRepo = new MediaFileRepository();

        //get recent media files 
        public IEnumerable<MediaFile> GetRecentMediaFiles()
        {
            List<MediaFile> mediaFiles = _mediaFileRepo.GetAll().ToList();
            return mediaFiles.Where(file => file.LastPlayedAt != null).OrderByDescending(file => file.LastPlayedAt).Take(20);
            // take 20 elements first -- sort by LastPlayedAt
        }


        //add a media file 
        public void AddMediaFile(MediaFile mediaFile)
        {
            List<MediaFile> mediaFiles = _mediaFileRepo.GetAll().ToList();
            bool isExisted = false;
            mediaFiles.ForEach(mediaF =>
            {
                if (mediaF.FilePath == mediaFile.FilePath)
                {
                    _mediaFileRepo.UpdateLastPlayedAt(mediaF, true);
                    isExisted = true;
                }
            });
            if (!isExisted)
                _mediaFileRepo.Add(mediaFile);
        }

        //delete recent a file 
        public void RemoveAMediaFile(MediaFile mediaFile)
        {
            _mediaFileRepo.UpdateLastPlayedAt(mediaFile, false);
        }
    }
}
