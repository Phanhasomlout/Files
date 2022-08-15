using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Sdk.Services.Settings;

namespace Files.Sdk.ViewModels.Layouts
{
    public abstract class BaseLayoutViewModel : ObservableObject
    {
        protected IUserSettingsService UserSettingsService { get; } = Ioc.Default.GetRequiredService<IUserSettingsService>();
    }
}
