using MediaPlayer.BLL;
using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System.Windows;
using System.Windows.Controls;
namespace MediaPlayer.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MedieFileService _mediaFileService = new MedieFileService();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MultiAdd.Content = "Open File";
            MultiHeaderTitle.Content = "Recent Files";
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            MultiAdd.Content = "Create Playlist";
            MultiHeaderTitle.Content = "Playlist";
        }

        private void PlayQueueButton_Click(object sender, RoutedEventArgs e)
        {
            MultiAdd.Content = "Add File";
            MultiHeaderTitle.Content = "Play Queue";
        }
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //change base duration
        }

        //đổi vị trí của 2 element
        private void SwapZIndex(UIElement element1, UIElement element2)
        {
            int zindex1 = Panel.GetZIndex(element1);
            int zindex2 = Panel.GetZIndex(element2);

            Panel.SetZIndex(element1, zindex2);
            Panel.SetZIndex(element2, zindex1);
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            SwapZIndex(PauseButton, PlayButton);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            SwapZIndex(PauseButton, PlayButton);
        }
        private void TitleButton_Click(object sender, RoutedEventArgs e)
        {
            SwapZIndex(Screen, StPanelSongQueue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillMediaFileList();
            HomeButton_Click(sender, e);
        }

        private void FillMediaFileList()
        {
            MediaFileList.ItemsSource = null;
            MediaFileList.ItemsSource = _mediaFileService.GetAllMediaFiles().AsEnumerable().Reverse();
        }


        //trước khi tắt app thì nó thực hiện lưu recent sóng xuống file json
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Music files (*.mp3;*.mp4)|*.mp3;*.mp4|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                // Nạp video vào MediaElement
                mediaElement.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);
                // Bắt đầu phát video
                mediaElement.Play();

                //cập nhật title và artist
                MediaFile newFile = Utils.GetPropertiesFromFilePath(filePath);
                TitleCurSong.Text = newFile.Title;
                ArtistCurSong.Text = newFile.Artists;

                //add vào Media File List
                _mediaFileService.AddMediaFile(newFile);

                //cập nhật recentsongs
                FillMediaFileList();
            }
            else
            {
                MessageBox.Show("File format doesnot support!", "Open Failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

            }
            else if (MultiAdd.Content.Equals("Create Playlist"))
            {

            }
        }
    }
}