
using AngleSharp.Dom;
using MediaPlayer.DAL.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MediaPlayer.BLL.Services
{
	public class PlayQueueService
	{
		public List<MediaFile> PlayQueue { get; set; }

		public void Add(MediaFile mediaFile)
		{
			if (PlayQueue == null)
			{
				PlayQueue = new List<MediaFile>();
			}
			//else
			//{
			//	foreach (var m in PlayQueue)
			//	{
			//		if (mediaFile.FilePath == m.FilePath)
			//		{
			//			return;
			//		}
					
			//	}

			//}

			PlayQueue.Add(mediaFile);
		}




	}
}
