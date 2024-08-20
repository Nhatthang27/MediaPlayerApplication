using MediaPlayer.BLL;
using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private int _mode = 1;
        //mode 1 == home, mode 2 == play queue, mode 3 == playlist

        private DispatcherTimer timer; // thực thi các hoạt động trong 1 khoảng thời gian định sẵn
        public MainWindow()
        {
            InitializeComponent();
        }
        private void SetUpTimerMediaFile()
        {
            // Khởi tạo timer để cập nhật thời gian phát hiện tại
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1); // xác định khoảng thgian của mỗi lần tick
            timer.Tick += Timer_Tick;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetUpTimerMediaFile();
            _curMediaFile = _mediaFileService.GetRecentMediaFiles().FirstOrDefault();
            if (_curMediaFile != null)
                MediaElementVideo.Source = new Uri(_curMediaFile.FilePath, UriKind.RelativeOrAbsolute);
            HomeButton_Click(sender, e);
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _mode = 1;
            MultiAdd.Content = "Open File";
            MultiHeaderTitle.Content = "Recent Files";
            UpdateTitleAndArtist();
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
            ShowItem(StPanelMediaFileList, Screen);
        }
        private void PlayQueueButton_Click(object sender, RoutedEventArgs e)
        {
            _mode = 2;
            MultiAdd.Content = "Add File";
            MultiHeaderTitle.Content = "Play Queue";
            FillMediaFileList(_playQueueService.PlayQueue);
            ShowItem(StPanelMediaFileList, Screen);
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            _mode = 3;
            MultiAdd.Content = "Create Playlist";
            MultiHeaderTitle.Content = "Playlist";
            FillMediaFileList(null);
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
            MediaElementVideo.Pause();

        }


        //chạy tiếp tục cur file
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //nếu file có tồn tại
            if (_curMediaFile != null)
            {
                ShowItem(PauseButton, PlayButton);
                //nếu bài hát có totaltime
                if (MediaElementVideo.NaturalDuration.HasTimeSpan && MediaElementVideo.Position == MediaElementVideo.NaturalDuration.TimeSpan)
                {
                    //nếu đã hết video
                    MediaElementVideo.Position = TimeSpan.Zero;
                }
                MediaElementVideo.Play();
            }

        }

        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            //nếu đang chieus video
            if (Panel.GetZIndex(Screen) > Panel.GetZIndex(StPanelMediaFileList))
            {
                //bấm vào chế độ nào thì bấm title sẽ trả về list của mode đó -> không gọi recent file ở đây
                //FillMediaFileList(MediaFileList.ItemsSource.Cast<MediaFile>().ToList());
                if (_mode == 1)
                {
                    FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
                }
                else if (_mode == 2)
                {
                    FillMediaFileList(_playQueueService.PlayQueue);
                }
                else
                {
                    //fill data của playlist
                    FillMediaFileList(null);
                }
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

        //chạy được một bài hát từ đầu
        private void RunFile(string filePath)
        {
            _curMediaFile = Utils.GetPropertiesFromFilePath(filePath);

            //Show pause button
            ShowItem(PauseButton, PlayButton);

            //show screen
            ShowItem(Screen, StPanelMediaFileList);

            //cập nhật MediaFileList, xoa de hien thi video
            FillMediaFileList(null);

            //cap nhat artist
            UpdateTitleAndArtist();


            // Nạp video vào MediaElement
            MediaElementVideo.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);

            // Bắt đầu phát video
            MediaElementVideo.Play();


            //add or update last time open
            _mediaFileService.AddMediaFile(_curMediaFile);

        }
        private void ProgressSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Tạm thời dừng Timer khi người dùng bắt đầu kéo Slider
            timer.Stop();
        }

        private void ProgressSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Khi người dùng thả Slider, cập nhật vị trí của MediaElement
            MediaElementVideo.Position = TimeSpan.FromSeconds(ProgressSlider.Value);

            // Khởi động lại Timer sau khi cập nhật vị trí
            timer.Start();
        }

        //ham bất đồng bộ, sẽ tự gọi sau 1 milisecond
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Kiểm tra xem media đã sẵn sàng và đang phát hay chưa
            if (MediaElementVideo.Source != null && MediaElementVideo.NaturalDuration.HasTimeSpan)
            {
                // Cập nhật thời gian phát hiện tại
                TimeElapsedTextBlock.Text = MediaElementVideo.Position.ToString(@"mm\:ss");

                // Cập nhật vị trí của Slider theo thời gian phát
                ProgressSlider.Value = MediaElementVideo.Position.TotalSeconds;
            }
        }
        private void MediaElementVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            PauseButton_Click(sender, e);
        }
        private void MediaElementVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem media có một giá trị thời lượng hợp lệ hay không
            if (MediaElementVideo.NaturalDuration.HasTimeSpan)
            {
                // Cập nhật thời gian tổng của media
                ProgressSlider.Maximum = MediaElementVideo.NaturalDuration.TimeSpan.TotalSeconds;
                TotalTimeTextBlock.Text = MediaElementVideo.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

                // Bắt đầu timer để cập nhật thời gian phát hiện tại
                timer.Start();
            }
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Chỉ cập nhật nếu người dùng không tương tác với Slider
            //IsMouseCaptureWithin check event của chuột
            if (!ProgressSlider.IsMouseCaptureWithin)
            {
                MediaElementVideo.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
            }


        }

        private void UpdateTitleAndArtist()
        {
            if (_curMediaFile != null)
            {
                TitleCurSong.Text = _curMediaFile.Title;
                ArtistCurSong.Text = _curMediaFile.Artists;
            }
        }

        //open new file
        private void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = Utils.COMMON_MEDIAFILE;
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                RunFile(filePath);
            }
        }

        //add to queue
        private void AddFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = Utils.COMMON_MEDIAFILE;
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MediaFile newFile = Utils.GetPropertiesFromFilePath(filePath);
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

            // Lấy đối tượng của phần tử từ thuộc tính Tag
            var mediaFileId = button?.Tag as int?;

            _mediaFileService.RemoveAMediaFile(mediaFileId.Value);
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
        }

        private void PlayInListButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var filePath = button?.Tag as string;

            RunFile(filePath);
        }
    }
}