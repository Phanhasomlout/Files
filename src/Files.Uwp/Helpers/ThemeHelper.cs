using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;

namespace Files.Uwp.Helpers
{    
    internal sealed class ThemeHelper
    {
        private readonly AppWindow _appWindow;
        private readonly UISettings _uiSettings;
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly Dictionary<string, Action<ApplicationTheme>> _themeChangedCallbacks;

        public static ApplicationTheme CurrentTheme { get; private set; } = Application.Current.RequestedTheme;

        private static Dictionary<AppWindow, ThemeHelper> _ThemeHelpers { get; } = new();
        public static IReadOnlyDictionary<AppWindow, ThemeHelper> ThemeHelpers
        {
            get => _ThemeHelpers;
        }

        private ThemeHelper(AppWindow appWindow)
        {
            _appWindow = appWindow;
            _uiSettings = new();
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _themeChangedCallbacks = new();
            _uiSettings.ColorValuesChanged += Settings_ColorValuesChanged;
        }

        public void UpdateTheme()
        {
            switch (CurrentTheme)
            {
                case ApplicationTheme.Dark:
                case ApplicationTheme.Light:
                    if (AppWindowTitleBar.IsCustomizationSupported())
                    {
                        _appWindow.TitleBar.ButtonHoverBackgroundColor = (Color)Application.Current.Resources["SystemBaseLowColor"];
                        _appWindow.TitleBar.ButtonForegroundColor = (Color)Application.Current.Resources["SystemBaseHighColor"];
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RegisterForThemeChangedCallback(string className, Action<ApplicationTheme> callback)
        {
            _themeChangedCallbacks.Add(className, callback);
        }

        public void UnregisterForThemeChangedCallback(string className)
        {
            _themeChangedCallbacks.Remove(className);
        }

        private async void Settings_ColorValuesChanged(UISettings sender, object args)
        {
            CurrentTheme = CurrentTheme == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark;
            await _dispatcherQueue.EnqueueAsync(() =>
            {
                UpdateTheme();

                foreach (var item in _themeChangedCallbacks.Values)
                {
                    item(CurrentTheme);
                }
            }, DispatcherQueuePriority.Low);
        }

        public static ThemeHelper RegisterWindowInstance(AppWindow appWindow)
        {
            if (_ThemeHelpers.TryGetValue(appWindow, out var themeHelper))
            {
                return themeHelper;
            }

            themeHelper = new(appWindow);
            _ThemeHelpers.Add(appWindow, themeHelper);

            return themeHelper;
        }

        public static bool UnregisterWindowInstance(AppWindow appWindow)
        {
            if (_ThemeHelpers.Remove(appWindow, out var themeHelper))
            {
                themeHelper._uiSettings.ColorValuesChanged -= themeHelper.Settings_ColorValuesChanged;
            }

            return false;
        }
    }
}