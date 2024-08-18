using MediaPlayer.BLL;
using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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

        private DispatcherTimer timer; // thực thi các hoạt động trong 1 khoảng thời gian định sẵn
        public MainWindow()
        {
            InitializeComponent();

            // Khởi tạo timer để cập nhật thời gian phát hiện tại
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // xác định khoảng thgian của mỗi lần tick
            timer.Tick += Timer_Tick;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HomeButton_Click(sender, e);
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MultiAdd.Content = "Open File";
            MultiHeaderTitle.Content = "Recent Files";
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
            ShowItem(StPanelMediaFileList, Screen);
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            MultiAdd.Content = "Create Playlist";
            MultiHeaderTitle.Content = "Playlist";
            FillMediaFileList(null);
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

        private void ShowItem(UIElement element1, UIElement element2)
        {
            if (Panel.GetZIndex(element1) < Panel.GetZIndex(element2))
            {
                int zindex1 = Panel.GetZIndex(element1);
                int zindex2 = Panel.GetZIndex(element2);

                Panel.SetZIndex(element1, zindex2);
                Panel.SetZIndex(element2, zindex1);
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowItem(PlayButton, PauseButton);
            mediaElementVideo.Pause();

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_curMediaFile != null)
            {
                ShowItem(PauseButton, PlayButton);
                mediaElementVideo.Play();
            }

        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            //nếu đang chieus video
            if (Panel.GetZIndex(Screen) > Panel.GetZIndex(StPanelMediaFileList))
            {

                FillMediaFileList(MediaFileList.ItemsSource.Cast<MediaFile>().ToList());
                ShowItem(StPanelMediaFileList, Screen);
            } //đang show list
            else
            {
                FillMediaFileList(null);
                ShowItem(Screen, StPanelMediaFileList);
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Kiểm tra xem media đã sẵn sàng và đang phát hay chưa
            if (mediaElementVideo.Source != null && mediaElementVideo.NaturalDuration.HasTimeSpan)
            {
                // Cập nhật thời gian phát hiện tại
                lblTimeElapsed.Text = mediaElementVideo.Position.ToString(@"mm\:ss");

                // Cập nhật vị trí của Slider theo thời gian phát
                progressSlider.Value = mediaElementVideo.Position.TotalSeconds;

                if (mediaElementVideo.NaturalDuration.TimeSpan == mediaElementVideo.Position)
                    ShowItem(PlayButton, PauseButton);
            }
        }
        private void mediaElementVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem media có một giá trị thời lượng hợp lệ hay không
            if (mediaElementVideo.NaturalDuration.HasTimeSpan)
            {
                // Cập nhật thời gian tổng của media
                progressSlider.Maximum = mediaElementVideo.NaturalDuration.TimeSpan.TotalSeconds;
                lblTotalTime.Text = mediaElementVideo.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                // Bắt đầu timer để cập nhật thời gian phát hiện tại
                timer.Start();
            }
        }

        private void progressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Điều chỉnh thời gian phát dựa trên giá trị Slider
            if (mediaElementVideo.NaturalDuration.HasTimeSpan)
            {
                mediaElementVideo.Position = TimeSpan.FromSeconds(progressSlider.Value);
            }
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
                _playQueueService.AddAMediaFile(newFile);
            }
            FillMediaFileList(_playQueueService.PlayQueue);
        }

        private void CreateAPlaylist()
        {
            //create a playlist
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

        private void RemoveInListButton_Click(object sender, RoutedEventArgs e)
        {

            // Ép kiểu sender thành Button
            var button = sender as Button;

            // Lấy ID của phần tử từ thuộc tính Tag
            var mediaFile = button?.Tag as MediaFile;

            if (mediaFile != null)
                _mediaFileService.RemoveAMediaFile(mediaFile);
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
        }
    }
}