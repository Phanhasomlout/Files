using Files.Uwp.Helpers;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Files.Uwp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        #nullable disable
        public static MainWindow Instance { get; private set; }
        #nullable restore

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();
            EnsureEarlyWindow();
        }

        private void EnsureEarlyWindow()
        {
            // Set title
            AppWindow.Title = "SecureFolderFS";

            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                // Extend title bar
                AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

                // Set window buttons background to transparent
                AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
            else
            {
                ExtendsContentIntoTitleBar = true;
                //WINUI3
                //SetTitleBar(HostControl.CustomTitleBar);
            }

            // Register ThemeHelper
            var themeHelper = ThemeHelper.RegisterWindowInstance(AppWindow);
            themeHelper.UpdateTheme();

            // Set min size
            base.MinHeight = 572;
            base.MinWidth = 648;
        }
    }
}
