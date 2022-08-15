using CommunityToolkit.Mvvm.ComponentModel;
using Files.Sdk.Models.Dialogs;
using Files.Sdk.Models.Imaging;

#nullable enable

namespace Files.Sdk.ViewModels.Dialogs.AddItemDialog
{
    public sealed class AddItemDialogListItemViewModel : ObservableObject
    {
        public string? Header { get; set; }

        public string? SubHeader { get; set; }

        public string? Glyph { get; set; }

        public ImageModel? Icon { get; set; }

        public bool IsItemEnabled { get; set; }

        public AddItemDialogResultModel? ItemResult { get; set; }
    }
}
