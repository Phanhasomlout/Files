using Files.Sdk.ViewModels.Dialogs;
using Files.Sdk.SecureStore;
using Files.Shared.Enums;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Files.Uwp.Dialogs
{
    public sealed partial class CredentialDialog : ContentDialog, IDialog<CredentialDialogViewModel>
    {
        public CredentialDialogViewModel ViewModel
        {
            get => (CredentialDialogViewModel)DataContext;
            set => DataContext = value;
        }

        public CredentialDialog()
        {
            this.InitializeComponent();
        }

        public new async Task<DialogResult> ShowAsync() => (DialogResult)await base.ShowAsync();

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.PrimaryButtonClickCommand.Execute(new DisposableArray(Encoding.UTF8.GetBytes(Password.Password)));
        }
    }
}