using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Guasco_Media_Player.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private ObservableCollection<Model> Collection = new ObservableCollection<Model>();

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var files = await KnownFolders.VideosLibrary.GetFilesAsync();
            foreach (var file in files)
            {
                var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView, 200);
                Collection.Add(new Model { Name = file.Name, Thumbnail = thumbnail });
            }
            gridView.ItemsSource = Collection;
            base.OnNavigatedTo(e);
            HideTitleBar();
        }

        public class Model
        {
            public StorageItemThumbnail Thumbnail { get; set; }
            public string Name { get; set; }
        }

        private async void NavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentDialog simpleDialog = new ContentDialog
            {
                Title = "Quit",
                Content = "Quit from Guasco Media Player?",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"

            };
            var result = await simpleDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Application.Current.Exit();
            }
            else
            {

            }
        }

        private void HideTitleBar()
        {
            ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
            TitleBar.ButtonBackgroundColor = Colors.Transparent;
            TitleBar.BackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }

        private void Play_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Play));
        }

        public async Task<BitmapImage> GetIt()
        {
            var files = await KnownFolders.VideosLibrary.GetFilesAsync();
            var thumb = await files[0].GetThumbnailAsync(ThumbnailMode.VideosView);
            var bitm = new BitmapImage();
            bitm.SetSource(thumb);
            return bitm;
        }

        private void About_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(About));
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Play));
        }
    }
}
