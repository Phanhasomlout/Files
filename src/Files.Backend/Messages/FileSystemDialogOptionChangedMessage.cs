using CommunityToolkit.Mvvm.Messaging.Messages;
using Files.Sdk.ViewModels.Dialogs.FileSystemDialog;

namespace Files.Sdk.Messages
{
    public sealed class FileSystemDialogOptionChangedMessage : ValueChangedMessage<FileSystemDialogConflictItemViewModel>
    {
        public FileSystemDialogOptionChangedMessage(FileSystemDialogConflictItemViewModel value)
            : base(value)
        {
        }
    }
}
