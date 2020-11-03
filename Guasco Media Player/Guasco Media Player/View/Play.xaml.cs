using FFmpegInterop;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Guasco_Media_Player.View
{
    public sealed partial class Play : Page
    {
        public Play()
        {
            this.InitializeComponent();
        }
        
        private FFmpegInteropMSS FFmpegMSS;
        public MediaPlayer MediaPlayer { get; }

        private async void LoadMediaFile(object sender, TappedRoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.VideosLibrary
            };

            picker.FileTypeFilter.Add(".3gp");
            picker.FileTypeFilter.Add(".avi");
            picker.FileTypeFilter.Add(".fla");
            picker.FileTypeFilter.Add(".flv");
            picker.FileTypeFilter.Add(".mkv");
            picker.FileTypeFilter.Add(".mov");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mpeg");
            picker.FileTypeFilter.Add(".mpeg2");
            picker.FileTypeFilter.Add(".mpg");
            picker.FileTypeFilter.Add(".rm");
            picker.FileTypeFilter.Add(".rmvb");
            picker.FileTypeFilter.Add(".vob");
            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".webm");

            StorageFile file = await picker.PickSingleFileAsync();


            if (file != null)
            {
                if (mediaPlayer.CanPause == true)
                {
                    try
                    {
                        mediaPlayer.Pause();
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    try
                    {
                        mediaPlayer.Stop();
                    }
                    catch (Exception)
                    {

                    }
                }
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                try
                {
                    FFmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(readStream, true, true);
                    MediaStreamSource mss = FFmpegMSS.GetMediaStreamSource();

                    if (mss != null)
                    {
                        #region Enable the hidden transport controls
                        mediaPlayer.AreTransportControlsEnabled = true;

                        mediaPlayer.TransportControls.IsFastForwardButtonVisible = true;
                        mediaPlayer.TransportControls.IsFastForwardEnabled = true;

                        mediaPlayer.TransportControls.IsFastRewindButtonVisible = true;
                        mediaPlayer.TransportControls.IsFastRewindEnabled = true;

                        mediaPlayer.TransportControls.IsNextTrackButtonVisible = true;
                        mediaPlayer.TransportControls.IsPreviousTrackButtonVisible = true;

                        mediaPlayer.TransportControls.IsPlaybackRateButtonVisible = true;
                        mediaPlayer.TransportControls.IsPlaybackRateEnabled = true;

                        mediaPlayer.TransportControls.IsSkipBackwardButtonVisible = true;
                        mediaPlayer.TransportControls.IsSkipBackwardEnabled = true;

                        mediaPlayer.TransportControls.IsSkipForwardButtonVisible = true;
                        mediaPlayer.TransportControls.IsSkipForwardEnabled = true;

                        mediaPlayer.TransportControls.IsStopButtonVisible = true;
                        mediaPlayer.TransportControls.IsStopEnabled = true;
                        mediaPlayer.TransportControls.IsRightTapEnabled = true;
                        #endregion

                        mediaPlayer.SetMediaStreamSource(mss);
                        mediaPlayer.Play();
                    }
                    else
                    {
                        var msg = new MessageDialog("An error occured");
                        await msg.ShowAsync();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void MediaPlayer_MediaOpened(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            myCmdBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void MediaPlayer_MediaEnded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            myCmdBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public async Task<BitmapImage> FetchThumbnail(StorageFile videofile)
        {
            var thumbnail = await videofile.GetThumbnailAsync(ThumbnailMode.VideosView);
            var inputBuffer = new Windows.Storage.Streams.Buffer(2048);
            
            IBuffer buf;
            IRandomAccessStream stream = new InMemoryRandomAccessStream();
            
            while ((buf = (await thumbnail.ReadAsync(inputBuffer, inputBuffer.Capacity, InputStreamOptions.None))).Length > 0)
                await stream.WriteAsync(buf);
            var image = new BitmapImage();
            image.SetSource(stream);
            return image;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
