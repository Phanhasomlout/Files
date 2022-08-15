using Files.Uwp.Filesystem;
using Files.Uwp.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Backend.Services;

namespace Files.Uwp.ViewModels.Previews
{
    public class ImagePreviewViewModel : BasePreviewModel
    {
        private IThreadingService ThreadingService { get; } = Ioc.Default.GetRequiredService<IThreadingService>();

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get => imageSource;
            private set => SetProperty(ref imageSource, value);
        }

        public ImagePreviewViewModel(ListedItem item) : base(item) {}

        public static bool ContainsExtension(string extension)
            => extension is ".png" or ".jpg" or ".jpeg" or ".bmp" or ".gif" or ".tiff" or ".ico" or ".webp";

        public override async Task<List<FileProperty>> LoadPreviewAndDetailsAsync()
        {
            using IRandomAccessStream stream = await Item.ItemFile.OpenAsync(FileAccessMode.Read);
            await ThreadingService.ExecuteOnUiThreadAsync();

            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            ImageSource = bitmap;
            
            return new List<FileProperty>();
        }
    }
}