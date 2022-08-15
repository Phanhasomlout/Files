using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.System;
using Files.Sdk.Services;
using Microsoft.UI.Xaml;

namespace Files.Uwp.ServicesImplementation
{
    /// <inheritdoc cref="IApplicationService"/>
    internal sealed class ApplicationService : IApplicationService
    {
        /// <inheritdoc/>
        public Task CloseApplicationAsync()
        {
            Application.Current.Exit();
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Version GetAppVersion()
        {
            var packageVersion = Package.Current.Id.Version;
            return new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build,
                packageVersion.Revision);
        }

        /// <inheritdoc/>
        public async Task OpenUriAsync(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
