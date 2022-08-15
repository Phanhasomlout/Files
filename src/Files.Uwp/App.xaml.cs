using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel;
using Files.Sdk.Services;
using Files.Uwp.Filesystem.FilesystemHistory;
using Files.Uwp.ViewModels;
using Files.Uwp.Helpers;
using Files.Uwp.Controllers;
using Files.Uwp.Filesystem;
using Files.Shared;
using Files.Uwp.Filesystem.Cloud;
using Files.Uwp.ServicesImplementation;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Diagnostics;
using ExceptionHelpers = Files.Uwp.Helpers.ExceptionHelpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Files.Uwp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        // private Window? _window;

        // TODO: Replace Window with _window
        public static MainWindow Window { get; private set; }

        private IServiceProvider Services { get; set; }

        private static bool ShowErrorNotification = false;
        private static string OutputPath = null;

        public static StorageHistoryWrapper HistoryWrapper = new StorageHistoryWrapper();
        public static SettingsViewModel AppSettings { get; private set; }
        public static MainViewModel MainViewModel { get; private set; }
        public static PaneViewModel PaneViewModel { get; private set; }
        public static PreviewPaneViewModel PreviewPaneViewModel { get; private set; }
        public static JumpListManager JumpList { get; private set; }
        public static RecentItemsManager RecentItemsManager { get; private set; }
        public static SidebarPinnedController SidebarPinnedController { get; private set; }
        public static TerminalController TerminalController { get; private set; }
        public static CloudDrivesManager CloudDrivesManager { get; private set; }
        public static NetworkDrivesManager NetworkDrivesManager { get; private set; }
        public static DrivesManager DrivesManager { get; private set; }
        public static WSLDistroManager WSLDistroManager { get; private set; }
        public static LibraryManager LibraryManager { get; private set; }
        public static FileTagsManager FileTagsManager { get; private set; }
        public static ExternalResourcesHelper ExternalResourcesHelper { get; private set; }

        public static ILogger Logger { get; private set; }
        private static readonly UniversalLogWriter logWriter = new UniversalLogWriter();

        public static OngoingTasksViewModel OngoingTasksViewModel { get; } = new OngoingTasksViewModel();
        public static SecondaryTileHelper SecondaryTileHelper { get; private set; } = new SecondaryTileHelper();

        public static string AppVersion = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
        {
            // TODO This code handles app activation types. Add any other activation kinds you want to handle.
            // Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/applifecycle#file-type-association
            var activatedEventArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();
            _ = activatedEventArgs;

            // Initialize MainWindow here
            Window = new MainWindow();
            Window.Activate();
            WindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(Window);
        }

        private void EnsureEarlyApp()
        {
            // Configure exception handlers
            UnhandledException += App_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // Start AppCenter
            // TODO: Start AppCenter
        }

        private IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddSingleton<IApplicationService, ApplicationService>()
                .AddSingleton<IThreadingService, ThreadingService>();

            return serviceCollection.BuildServiceProvider();
        }

        #region Exception Handling

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            LogException(e.Exception);
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            LogException(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            LogException(e.ExceptionObject as Exception);
        }

        private void LogException(Exception? ex)
        {
            var formattedException = ExceptionHelpers.FormatException(ex);

            Debug.WriteLine(formattedException);
            Debugger.Break(); // Please check "Output Window" for exception details (View -> Output Window) (Ctr + Alt + O)

#if !DEBUG
            ExceptionHelpers.LogExceptionToFile(formattedException);
#endif
        }

        #endregion

        public static IntPtr WindowHandle { get; private set; }
    }
}
