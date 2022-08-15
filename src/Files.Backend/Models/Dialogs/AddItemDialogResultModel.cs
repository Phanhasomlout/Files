using Files.Sdk.Enums;
using Files.Shared;

#nullable enable

namespace Files.Sdk.Models.Dialogs
{
    public sealed class AddItemDialogResultModel
    {
        public AddItemDialogItemType ItemType { get; set; }

        public ShellNewEntry? ItemInfo { get; set; }
    }
}
