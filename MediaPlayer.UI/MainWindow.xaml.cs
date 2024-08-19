using MediaPlayer.BLL;
using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Forms;
namespace MediaPlayer.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MedieFileService _mediaFileService = new MedieFileService();
		private PlayQueueService _playQueueService = new PlayQueueService();
		private MediaFile _curMediaFile = null;
		private Visibility _buttonVisibility = Visibility.Visible;
		public MainWindow()
		{
			InitializeComponent();

		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			FillMediaFileList(_mediaFileService.GetAllMediaFiles().AsEnumerable().Reverse());
			HomeButton_Click(sender, e);
		}
		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			MultiAdd.Content = "Open File";
			MultiHeaderTitle.Content = "Recent Files";
			FillMediaFileList(_mediaFileService.GetAllMediaFiles().AsEnumerable().Reverse());
		}
		private void PlaylistButton_Click(object sender, RoutedEventArgs e)
		{
			MultiAdd.Content = "Create Playlist";
			MultiHeaderTitle.Content = "Playlist";
			ShowItem(StPanelMediaFileList, Screen);
		}
		private void PlayQueueButton_Click(object sender, RoutedEventArgs e)
		{
			MultiAdd.Content = "Add File";
			MultiHeaderTitle.Content = "Play Queue";
			FillMediaFileList(_playQueueService.PlayQueue);
			ShowItem(StPanelMediaFileList, Screen);
			
			

			
		}
		
		
		private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			//change base duration
		}

		//đổi vị trí của 2 element, if zindex1 < zindex2 return true, else return false
		private void SwapZIndex(UIElement element1, UIElement element2)
		{
			int zindex1 = Panel.GetZIndex(element1);
			int zindex2 = Panel.GetZIndex(element2);

			Panel.SetZIndex(element1, zindex2);
			Panel.SetZIndex(element2, zindex1);
		}

		private void ShowItem(UIElement element1, UIElement element2)
		{
			if (Panel.GetZIndex(element1) < Panel.GetZIndex(element2))
			{
				SwapZIndex(element1, element2);
			}
		}

		private void PauseButton_Click(object sender, RoutedEventArgs e)
		{
			SwapZIndex(PauseButton, PlayButton);
			mediaElementVideo.Pause();
		}

		private void PlayButton_Click(object sender, RoutedEventArgs e)
		{
			if (_curMediaFile != null)
			{
				SwapZIndex(PlayButton, PauseButton);
				mediaElementVideo.Play();
			}

		}

		private void TitleButton_Click(object sender, RoutedEventArgs e)
		{
			SwapZIndex(Screen, StPanelMediaFileList);
			if (Panel.GetZIndex(Screen) > Panel.GetZIndex(StPanelMediaFileList))
			{
				FillMediaFileList(null);
			}
			else
			{
				FillMediaFileList(_mediaFileService.GetAllMediaFiles().AsEnumerable().Reverse());
			}
		}

		//hàm fill MediaFileList - isQueue = true: fill queue, false: fill recent files
		private void FillMediaFileList(IEnumerable<MediaFile> mediaListFile)
		{
			MediaFileList.ItemsSource = null;
			MediaFileList.ItemsSource = mediaListFile;
		}

		private void RunFile(string filePath)
		{
			//Show pause button
			ShowItem(PauseButton, PlayButton);

			//show screen
			ShowItem(Screen, StPanelMediaFileList);

			_curMediaFile = Utils.GetPropertiesFromFilePath(filePath);
			// Nạp video vào MediaElement
			mediaElementVideo.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);
			// Bắt đầu phát video
			mediaElementVideo.Play();
		}

		private void UpdateTitleAndArtist(MediaFile mediaFile)
		{
			TitleCurSong.Text = mediaFile.Title;
			ArtistCurSong.Text = mediaFile.Artists;
		}

		private void OpenFile()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = Utils.COMMON_MEDIAFILE;
			if (openFileDialog.ShowDialog() == true)
			{
				string filePath = openFileDialog.FileName;
				RunFile(filePath);

				MediaFile newFile = Utils.GetPropertiesFromFilePath(filePath);

				//cập nhật title và artist
				UpdateTitleAndArtist(newFile);

				// set lastPlayedAt
				newFile.LastPlayedAt = DateTime.Now;

				//add vào Media File List
				_mediaFileService.AddMediaFile(newFile);
				_playQueueService.Add(newFile);

				//cập nhật MediaFileList, xoa de hien thi video
				FillMediaFileList(null);
			}
			else
			{
				MessageBox.Show("File format doesnot support!", "Open Failed", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void AddFile()
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
			openFileDialog.Filter = Utils.COMMON_MEDIAFILE;
			if (openFileDialog.ShowDialog() == true)
			{
				string filePath = openFileDialog.FileName;
				MediaFile newFile = Utils.GetPropertiesFromFilePath(filePath);
				// set lastPlayedAt
				newFile.LastPlayedAt = DateTime.Now;
				//add queue
				_playQueueService.Add(newFile);

			}
			FillMediaFileList(_playQueueService.PlayQueue);
		}

		private void CreateAPlaylist()
		{
			//create a playlist
		}
		//handle + icon on each media file
		private void AddToQueueFromHomeBtn_MouseEnter(object sender, MouseEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				// Find the Popup within the same DataTemplate
				var stackPanel = button.Parent as StackPanel;
				if (stackPanel != null)
				{
					var popup = stackPanel.Children.OfType<Popup>().FirstOrDefault();
					if (popup != null)
					{
						
							popup.IsOpen = true;
						
					}
				}
			}
		}
		private void AddToQueueFromHomeBtn_MouseLeave(object sender, MouseEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				// Find the Popup within the same DataTemplate
				var stackPanel = button.Parent as StackPanel;
				if (stackPanel != null)
				{
					var popup = stackPanel.Children.OfType<Popup>().FirstOrDefault();
					if (popup != null )
					{
						if (!popup.IsMouseOver && !button.IsMouseOver)
							popup.IsOpen = false;
					}
				}
			}
		}
		private void AddQueueButton_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var mediaFile = button?.DataContext as MediaFile; // Replace with the correct way to get the MediaFile

			if (mediaFile != null)
			{
				// Add the media file to the in-memory queue
				_playQueueService.Add(mediaFile);

				// Optionally, show a message or update the UI
				MessageBox.Show($"{mediaFile.FileName} has been added to the queue.");
			}
		}
		private void MultiAdd_Click(object sender, RoutedEventArgs e)
		{
			if (MultiAdd.Content.Equals("Open File"))
			{
				OpenFile();
			}
			else if (MultiAdd.Content.Equals("Add File"))
			{
				AddFile();
			}
			else if (MultiAdd.Content.Equals("Create Playlist"))
			{
				CreateAPlaylist();
			}
		}
	}
}