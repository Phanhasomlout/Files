using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Sdk.Services.Settings;
using Files.Uwp.Helpers;
using CommunityToolkit.WinUI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace Files.Uwp.ViewModels.SettingsViewModels
{
    public class AppearanceViewModel : ObservableObject
    {
        private IUserSettingsService UserSettingsService { get; } = Ioc.Default.GetRequiredService<IUserSettingsService>();

        private AppTheme selectedTheme = App.AppSettings.SelectedTheme;

        public AppearanceViewModel()
        {
            Themes = new List<string>()
            {
                "Default".GetLocalized(),
                "LightTheme".GetLocalized(),
                "DarkTheme".GetLocalized()
            };
        }

        /// <summary>
        /// Forces the application to use the correct styles if compact mode is turned on
        /// </summary>
        public void SetCompactStyles()
        {
            if (UseCompactStyles)
            {
                Application.Current.Resources["ListItemHeight"] = 28;
                Application.Current.Resources["NavigationViewItemOnLeftMinHeight"] = 24;
            }
            else
            {
                Application.Current.Resources["ListItemHeight"] = 36;
                Application.Current.Resources["NavigationViewItemOnLeftMinHeight"] = 32;
            }
        }

        public List<string> Themes { get; set; }
        public List<AppTheme> CustomThemes => App.ExternalResourcesHelper.Themes;

        public bool MoveOverflowMenuItemsToSubMenu
        {
            get => UserSettingsService.AppearanceSettingsService.MoveOverflowMenuItemsToSubMenu;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.MoveOverflowMenuItemsToSubMenu)
                {
                    UserSettingsService.AppearanceSettingsService.MoveOverflowMenuItemsToSubMenu = value;
                    OnPropertyChanged();
                }
            }
        }

        public AppTheme SelectedTheme
        {
            get
            {
                return selectedTheme;
            }
            set
            {
                if (SetProperty(ref selectedTheme, value))
                {
                    if (selectedTheme != null)
                    {
                        // Remove the old resource file and load the new file
                        App.ExternalResourcesHelper.UpdateTheme(App.AppSettings.SelectedTheme, selectedTheme)
                            .ContinueWith(t =>
                            {
                                App.AppSettings.SelectedTheme = selectedTheme;
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                }
            }
        }

        public bool ShowFavoritesSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowFavoritesSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowFavoritesSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowFavoritesSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool PinRecycleBinToSideBar
        {
            get => UserSettingsService.AppearanceSettingsService.PinRecycleBinToSidebar;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.PinRecycleBinToSidebar)
                {
                    UserSettingsService.AppearanceSettingsService.PinRecycleBinToSidebar = value;
                    App.SidebarPinnedController.Model.ShowHideRecycleBinItem(value);
                    OnPropertyChanged();
                }
            }
        }

        public bool UseCompactStyles
        {
            get => UserSettingsService.AppearanceSettingsService.UseCompactStyles;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.UseCompactStyles)
                {
                    UserSettingsService.AppearanceSettingsService.UseCompactStyles = value;

                    // Apply the correct styles
                    SetCompactStyles();

                    OnPropertyChanged();
                }
            }
        }

        public bool ShowLibrarySection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowLibrarySection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowLibrarySection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowLibrarySection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowDrivesSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowDrivesSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowDrivesSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowDrivesSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowCloudDrivesSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowCloudDrivesSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowCloudDrivesSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowCloudDrivesSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowNetworkDrivesSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowNetworkDrivesSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowNetworkDrivesSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowNetworkDrivesSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowWslSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowWslSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowWslSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowWslSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool AreFileTagsEnabled => UserSettingsService.PreferencesSettingsService.AreFileTagsEnabled;

        public bool ShowFileTagsSection
        {
            get => UserSettingsService.AppearanceSettingsService.ShowFileTagsSection;
            set
            {
                if (value != UserSettingsService.AppearanceSettingsService.ShowFileTagsSection)
                {
                    UserSettingsService.AppearanceSettingsService.ShowFileTagsSection = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task OpenThemesFolder()
        {
            //await CoreApplication.MainView.Dispatcher.YieldAsync(); // WINUI3
            await NavigationHelpers.OpenPathInNewTab(App.ExternalResourcesHelper.ImportedThemesFolder.Path);
        }
    }
}