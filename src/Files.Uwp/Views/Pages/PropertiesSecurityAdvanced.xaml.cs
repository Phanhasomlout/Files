using Files.Uwp.DataModels.NavigationControlItems;
using Files.Uwp.Filesystem;
using Files.Uwp.Filesystem.Permissions;
using Files.Uwp.ViewModels.Properties;
using CommunityToolkit.WinUI;
using System;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Sdk.Services;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace Files.Uwp.Views
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class PropertiesSecurityAdvanced : Page
    {
        private IThreadingService ThreadingService { get; } = Ioc.Default.GetRequiredService<IThreadingService>();

        private static ApplicationViewTitleBar TitleBar;

        private object navParameterItem;

        public string DialogTitle => string.Format("SecurityAdvancedPermissionsTitle".GetLocalized(), ViewModel.Item.ItemName);

        public SecurityProperties ViewModel { get; set; }

        public PropertiesSecurityAdvanced()
        {
            this.InitializeComponent();

            var flowDirectionSetting = /*
                TODO ResourceContext.GetForCurrentView and ResourceContext.GetForViewIndependentUse do not exist in Windows App SDK
                Use your ResourceManager instance to create a ResourceContext as below. If you already have a ResourceManager instance,
                replace the new instance created below with correct instance.
                Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/mrtcore
            */new Microsoft.Windows.ApplicationModel.Resources.ResourceManager().CreateResourceContext().QualifierValues["LayoutDirection"];

            if (flowDirectionSetting == "RTL")
            {
                FlowDirection = FlowDirection.RightToLeft;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var args = e.Parameter as PropertiesPageNavigationArguments;
            navParameterItem = args.Item;

            if (args.Item is ListedItem listedItem)
            {
                ViewModel = new SecurityProperties(listedItem);
            }
            else if (args.Item is DriveItem driveitem)
            {
                ViewModel = new SecurityProperties(driveitem);
            }

            base.OnNavigatedTo(e);
        }

        private async void Properties_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppSettings.ThemeModeChanged += AppSettings_ThemeModeChanged;
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                // Set window size in the loaded event to prevent flickering
                TitleBar = 
                /*
                   TODO UA315_A Use Microsoft.UI.Windowing.AppWindow for window Management instead of ApplicationView/CoreWindow or Microsoft.UI.Windowing.AppWindow APIs
                   Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/windowing
                */
                ApplicationView.GetForCurrentView().TitleBar;
                TitleBar.ButtonBackgroundColor = Colors.Transparent;
                TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

                await ThreadingService.ExecuteOnUiThreadAsync();
                App.AppSettings.UpdateThemeElements.Execute(null);
            }
            else
            {
            }

            ViewModel.GetFilePermissions();
        }

        private void Properties_Unloaded(object sender, RoutedEventArgs e)
        {
            // Why is this not called? Are we cleaning up properly?
        }

        private async void AppSettings_ThemeModeChanged(object sender, EventArgs e)
        {
            await /*
                TODO UA306_A2: UWP CoreDispatcher : Windows.UI.Core.CoreDispatcher is no longer supported. Use DispatcherQueue instead. Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/threading
            */Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
                {
                    switch (RequestedTheme)
                    {
                        case ElementTheme.Default:
                            TitleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["SystemBaseLowColor"];
                            TitleBar.ButtonForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                            break;

                        case ElementTheme.Light:
                            TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(51, 0, 0, 0);
                            TitleBar.ButtonForegroundColor = Colors.Black;
                            break;

                        case ElementTheme.Dark:
                            TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(51, 255, 255, 255);
                            TitleBar.ButtonForegroundColor = Colors.White;
                            break;
                    }
                }
            });
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                if (await ViewModel.SetFilePermissions())
                {
                    await 
                /*
                   TODO UA315_A Use Microsoft.UI.Windowing.AppWindow for window Management instead of ApplicationView/CoreWindow or Microsoft.UI.Windowing.AppWindow APIs
                   Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/windowing
                */
                ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
            }
            else
            {
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                await 
                /*
                   TODO UA315_A Use Microsoft.UI.Windowing.AppWindow for window Management instead of ApplicationView/CoreWindow or Microsoft.UI.Windowing.AppWindow APIs
                   Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/windowing
                */
                ApplicationView.GetForCurrentView().TryConsolidateAsync();
            }
            else
            {
            }
        }

        private async void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(VirtualKey.Escape))
            {
                if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
                {
                    await 
                /*
                   TODO UA315_A Use Microsoft.UI.Windowing.AppWindow for window Management instead of ApplicationView/CoreWindow or Microsoft.UI.Windowing.AppWindow APIs
                   Read: https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/migrate-to-windows-app-sdk/guides/windowing
                */
                ApplicationView.GetForCurrentView().TryConsolidateAsync();
                }
                else
                {
                }
            }
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            // This manually adds the user's theme resources to the page
            // I was unable to get this to work any other way
            try
            {
                var xaml = XamlReader.Load(App.ExternalResourcesHelper.CurrentThemeResources) as ResourceDictionary;
                App.Current.Resources.MergedDictionaries.Add(xaml);
            }
            catch (Exception)
            {
            }
        }

        public class PropertiesPageNavigationArguments
        {
            public object Item { get; set; }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedAccessRules = (sender as ListView).SelectedItems.Cast<FileSystemAccessRuleForUI>().ToList();

            if (e.AddedItems != null)
            {
                foreach (var item in e.AddedItems)
                {
                    (item as FileSystemAccessRuleForUI).IsSelected = true;
                }
            }
            if (e.RemovedItems != null)
            {
                foreach (var item in e.RemovedItems)
                {
                    (item as FileSystemAccessRuleForUI).IsSelected = false;
                }
            }
        }
    }
}