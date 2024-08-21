using MediaPlayer.BLL;
using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
        private PlayStackService _playStackService = new PlayStackService();
        private PlaylistService _playlistService = new PlaylistService();
        private Point _dragStartPoint;
        private MediaFile _curMediaFile = null;
        private int _mode = 1;
        //mode 1 == home, mode 2 == play queue, mode 3 == playlist

        private DispatcherTimer timer; // thực thi các hoạt động trong 1 khoảng thời gian định sẵn
        private Visibility _buttonVisibility = Visibility.Visible;

        private double _volumeValue;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void SetUpTimerMediaFile()
        {
            // Khởi tạo timer để cập nhật thời gian phát hiện tại
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500); // xác định khoảng thgian của mỗi lần tick
            timer.Tick += Timer_Tick;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetUpTimerMediaFile();
            _curMediaFile = _mediaFileService.GetRecentMediaFiles().FirstOrDefault();
            if (_curMediaFile != null)
                MediaElementVideo.Source = new Uri(_curMediaFile.FilePath, UriKind.RelativeOrAbsolute);
            if (_curMediaFile != null)
            {
                _playQueueService.AddPriority(_curMediaFile);
            }
            HomeButton_Click(sender, e);
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _mode = 1;
            MultiAdd.Content = "Open File";
            MultiHeaderTitle.Content = "Recent Files";
            UpdateTitleAndArtist();
            ShowItem(StPanelMediaFileList, Screen);
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            _mode = 3;
            MultiAdd.Content = "Create Playlist";
            MultiHeaderTitle.Content = "Playlist";

            var playlists = _playlistService.GetAllPlaylist().ToList();


            MediaFileList.ItemsSource = playlists;

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
                ShowItem(Screen, StPanelMediaFileList);
                FillMediaFileList(null);
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

        //chạy được một bài hát từ đầu // dependence: OpenFile, Next, Previous, PlayInList
        private void RunFile(string filePath)
        {

            _curMediaFile = Utils.GetPropertiesFromFilePath(filePath);

            //Show pause button
            ShowItem(PauseButton, PlayButton);

            //cập nhật MediaFileList, xoa de hien thi video
            FillMediaFileList(null);
            //show screen
            ShowItem(Screen, StPanelMediaFileList);

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
                //remove queue priority
                _playQueueService.RemoveAt(0);

                string filePath = openFileDialog.FileName;
                RunFile(filePath);
                _curMediaFile = _mediaFileService.GetMediaFileByFilePath(filePath);

                //add queue priority
                _playQueueService.AddPriority(_curMediaFile);
            }
        }

        //add to queue
        private void AddFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = Utils.COMMON_MEDIAFILE;
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MediaFile newFile = Utils.GetPropertiesFromFilePath(filePath);
                //add queue
                _playQueueService.Add(newFile);

            }
            PlayQueueButton_Click(sender, e);
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
                    if (popup != null)
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
            //FillMediaFileList(_playQueueService.PlayQueue); -> doi sang bam nut Play Queue
            PlayQueueButton_Click(sender, e);
        }
        private void MultiAdd_Click(object sender, RoutedEventArgs e)
        {
            if (MultiAdd.Content.Equals("Open File"))
            {
                OpenFile();
            }
            else if (MultiAdd.Content.Equals("Add File"))
            {
                AddFile(sender, e);
            }
            else if (MultiAdd.Content.Equals("Create Playlist"))
            {
                if (PlaylistCreationGrid.Visibility == Visibility.Visible)
                {
                    PlaylistCreationGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PlaylistCreationGrid.Visibility = Visibility.Visible;
                }
            }
        }
        //handle sắp xếp thứ tự bài hát trong list queue
        private void MediaFileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }
        private void MediaFileList_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(null);

            //Check if the drag distance is significant enough
            //xử lý con chuột ở mỗi hàng mediafile
            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(currentPosition.X - _dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(currentPosition.Y - _dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                ListView listView = sender as ListView;
                if (listView != null)
                {
                    //find which mediafile at the place where mouse pressed
                    ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                    if (listViewItem != null)
                    {
                        MediaFile mediaFile = (MediaFile)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                        DragDrop.DoDragDrop(listViewItem, mediaFile, DragDropEffects.Move);
                    }
                }
            }
        }
        //Handle when you press mouse at any element of the ListViewItem such as button play, name,...
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
        //event drag mouse
        private void MediaFileList_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }
        private void MediaFileList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MediaFile)))
            {
                MediaFile droppedData = e.Data.GetData(typeof(MediaFile)) as MediaFile;
                ListView listView = sender as ListView;
                ListViewItem targetItem = GetNearestContainer(e.OriginalSource);
                if (targetItem == null) return;
                MediaFile target = listView.ItemContainerGenerator.ItemFromContainer(targetItem) as MediaFile;

                if (droppedData != null && target != null && !ReferenceEquals(droppedData, target))
                {
                    if (_playQueueService.PlayQueue != null)
                    {
                        int removedIdx = _playQueueService.PlayQueue.IndexOf(droppedData);
                        int targetIdx = _playQueueService.PlayQueue.IndexOf(target);

                        if (removedIdx != -1 && targetIdx != -1)
                        {
                            _playQueueService.PlayQueue.RemoveAt(removedIdx);
                            _playQueueService.PlayQueue.Insert(targetIdx, droppedData);
                            MediaFileList.Items.Refresh();
                        }
                    }
                }

            }
        }
        // Helper method to get the nearest ListViewItem
        private static ListViewItem GetNearestContainer(object originalSource)
        {
            var current = originalSource as UIElement;
            while (current != null && !(current is ListViewItem))
            {
                current = VisualTreeHelper.GetParent(current) as UIElement;
            }
            return current as ListViewItem;
        }

        //Handle: track the current playing song
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_playQueueService.PlayQueue != null && _playQueueService.PlayQueue.Count > 1)
            {
                _playStackService.PushToStack(_curMediaFile);
                _curMediaFile = _playQueueService.PlayQueue[1];
                _playQueueService.RemoveAt(0);
                RunFile(_curMediaFile.FilePath);
            }
            else
            {
                MessageBox.Show("There are no next songs in the queue!");
            }
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_playStackService.IsStackEmpty())
            {
                _curMediaFile = _playStackService.PopFromStack();
                RunFile(_curMediaFile.FilePath);
                _playQueueService.AddPriority(_curMediaFile);
            }
            else
            {
                MessageBox.Show("There are no previous songs in the queue!");
            }
        }

        //handle auto move to next song when the current one end
        //will call when the playpack end
        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_playQueueService.PlayQueue != null && _playQueueService.PlayQueue.Count > 1)
            {
                _playStackService.PushToStack(_curMediaFile);
                string filePath = _playQueueService.PlayQueue[1].FilePath;
                _playQueueService.RemoveAt(0);
                RunFile(filePath);
                _curMediaFile = _mediaFileService.GetMediaFileByFilePath(filePath);
            }
            else
            {
                PauseButton_Click(sender, e);
            }
        }
        private void CreatePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            string playlistName = PlaylistNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(playlistName))
            {
                MessageBox.Show("Please enter a valid playlist name.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime createdAt = DateTime.Now;

            CreateAPlaylist(playlistName, createdAt);

            PlaylistCreationGrid.Visibility = Visibility.Collapsed;

            PlaylistButton_Click(sender, e);

        }
        private void CreateAPlaylist(string name, DateTime createdAt)
        {

            List<Playlist> existingPlaylists = _playlistService.GetAllPlaylist().ToList();

            string uniqueName = GetUniquePlaylistName(name, existingPlaylists);

            Playlist playlist = new Playlist() { Title = uniqueName, CreatedAt = createdAt };

            _playlistService.addPlaylist(playlist);

            MessageBox.Show($"Playlist '{name}' created successfully on {createdAt}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private string GetUniquePlaylistName(string baseName, List<Playlist> existingPlaylists)
        {
            string uniqueName = baseName;

            int count = existingPlaylists.Count(p => p.Title == baseName || p.Title.StartsWith(baseName + "("));

            if (count > 0)
            {
                uniqueName = $"{baseName}({count})";
            }
            return uniqueName;
        }

        private void Recent_RemoveInListButton_Click(object sender, RoutedEventArgs e)
        {
            // Ép kiểu sender thành Button
            var button = sender as Button;

            // Lấy đối tượng của phần tử từ thuộc tính Tag
            var mediaFile = button?.Tag as MediaFile;

            _mediaFileService.RemoveAMediaFile(mediaFile.MediaFileId);
            FillMediaFileList(_mediaFileService.GetRecentMediaFiles());
        }

        private void Queue_RemoveInListButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult answer = MessageBox.Show("Do you really want to delete?", "Confirm?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (answer == MessageBoxResult.No)
                return;

            Button button = sender as Button;
            if (button != null)
            {
                var mediaFile = button?.Tag as MediaFile;
                _playQueueService.Remove(mediaFile);
                FillMediaFileList(_playQueueService.PlayQueue);
                MediaFileList.Items.Refresh();
            }
        }

        private void RemoveInListButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mode == 1)
            {
                Recent_RemoveInListButton_Click(sender, e);
            }
            else if (_mode == 2)
            {
                Queue_RemoveInListButton_Click(sender, e);
            }
            else
            {
                //playlist
                //Queue_RemoveInListButton_Click(sender, e);
            }
        }

        private void PlayInListButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            var mediaFile = button?.Tag as MediaFile;

            //xoa cai được chọn nếu nó khong phải là cái đầu tiên trong queue
            if (_mode == 2 && _playQueueService.GetIndexInQueue(mediaFile) != 0)
            {
                _playQueueService.Remove(mediaFile);
            }
            //chọn thằng đàu trong queue thì không thêm vào stack
            if (_mode != 2 || _playQueueService.GetIndexInQueue(mediaFile) != 0)
            {
                _playStackService.PushToStack(_curMediaFile);
            }
            _playQueueService.Remove(_curMediaFile);
            RunFile(mediaFile.FilePath);
            _playQueueService.AddPriority(_curMediaFile);
        }

        private void VolumeProgressBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AdjustVolume(e);
        }

        private void VolumeProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AdjustVolume(e);
            }
        }

        private void AdjustVolume(MouseEventArgs e)
        {
            var pos = e.GetPosition(VolumeProgressBar);
            double volume = (pos.X / VolumeProgressBar.ActualWidth);
            if (volume > 0)
            {
                VolumeIconBlock.Icon = FontAwesome.Sharp.IconChar.VolumeUp;
            } else
            {
                VolumeIconBlock.Icon = FontAwesome.Sharp.IconChar.VolumeMute;
            }
            VolumeProgressBar.Value = volume;
            MediaElementVideo.Volume = VolumeProgressBar.Value;
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            if (VolumeIconBlock.Icon == FontAwesome.Sharp.IconChar.VolumeUp)
            {
                _volumeValue = VolumeProgressBar.Value;
                VolumeProgressBar.Value = 0;
                MediaElementVideo.Volume = 0;
                VolumeIconBlock.Icon = FontAwesome.Sharp.IconChar.VolumeMute;
            }
            else
            {
                VolumeProgressBar.Value = _volumeValue;
                MediaElementVideo.Volume = VolumeProgressBar.Value;
                VolumeIconBlock.Icon = FontAwesome.Sharp.IconChar.VolumeUp;
            }

        }

        private void VolumeButton_MouseMove(object sender, MouseEventArgs e)
        {
            VolumeProgressBar.Visibility = Visibility.Visible;
        }

        private void VolumeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(VolumeProgressBar);
            var progressBarRect = new Rect(0, 0, VolumeProgressBar.ActualWidth, VolumeProgressBar.ActualHeight);

            double limit = 10;

            if (!progressBarRect.Contains(mousePos) &&
                (mousePos.X < -limit || mousePos.X > VolumeProgressBar.ActualWidth + limit ||
                mousePos.Y < -limit || mousePos.Y > VolumeProgressBar.ActualHeight + limit))
            {
                VolumeProgressBar.Visibility = Visibility.Collapsed;
            }
        }
        //handle when click on a shuffle button
        private void ShuffleQueueButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_playQueueService.IsEmpty())
            {
                _playQueueService.Shuffle();
                FillMediaFileList(_playQueueService.PlayQueue);
                ShowItem(StPanelMediaFileList, Screen);
            }
            else
            {
                MessageBox.Show("There are no next songs in the queue!");
            }
        }
    }
}