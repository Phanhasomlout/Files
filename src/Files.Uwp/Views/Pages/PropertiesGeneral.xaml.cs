using Files.Shared;
using Files.Uwp.Filesystem;
using Files.Uwp.Helpers;
using Files.Shared.Enums;
using Files.Uwp.ViewModels.Properties;
using CommunityToolkit.WinUI;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using CommunityToolkit.Mvvm.DependencyInjection;
using Files.Sdk.Services;

namespace Files.Uwp.Views
{
    public sealed partial class PropertiesGeneral : PropertiesTab
    {
        private IThreadingService ThreadingService { get; } = Ioc.Default.GetRequiredService<IThreadingService>();

        public PropertiesGeneral()
        {
            this.InitializeComponent();
        }

        public override async Task<bool> SaveChangesAsync(ListedItem item)
        {
            if (BaseProperties is DriveProperties driveProps)
            {
                var drive = driveProps.Drive;
                ViewModel.ItemName = ItemFileName.Text; // Make sure ItemName is updated
                if (!string.IsNullOrWhiteSpace(ViewModel.ItemName) && ViewModel.OriginalItemName != ViewModel.ItemName)
                {
                    var remDrive = new System.Text.RegularExpressions.Regex(@"\s*\(\w:\)$");
                    ViewModel.ItemName = remDrive.Replace(ViewModel.ItemName, ""); // Remove "(C:)" from the new label
                    var connection = await AppServiceConnectionHelper.Instance;
                    if (connection != null && AppInstance.FilesystemViewModel != null)
                    {
                        _ = await connection.SendMessageForResponseAsync(new ValueSet()
                        {
                            { "Arguments", "SetVolumeLabel" },
                            { "drivename", drive.Path },
                            { "newlabel", ViewModel.ItemName }
                        });

                        await ThreadingService.ExecuteOnUiThreadAsync();
                        await drive.UpdateLabelAsync();
                        await AppInstance.FilesystemViewModel?.SetWorkingDirectoryAsync(drive.Path);
                        
                        return true;
                    }
                }
            }
            else if (BaseProperties is LibraryProperties libProps)
            {
                var library = libProps.Library;
                ViewModel.ItemName = ItemFileName.Text; // Make sure ItemName is updated
                var newName = ViewModel.ItemName;
                if (!string.IsNullOrWhiteSpace(newName) && ViewModel.OriginalItemName != newName)
                {
                    if (AppInstance.FilesystemViewModel != null && App.LibraryManager.CanCreateLibrary(newName).result)
                    {
                        var libraryPath = library.ItemPath;
                        var renamed = await AppInstance.FilesystemHelpers.RenameAsync(new StorageFileWithPath(null, libraryPath), $"{newName}{ShellLibraryItem.EXTENSION}", Windows.Storage.NameCollisionOption.FailIfExists, false);
                        if (renamed == ReturnResult.Success)
                        {
                            var newPath = Path.Combine(Path.GetDirectoryName(libraryPath), $"{newName}{ShellLibraryItem.EXTENSION}");

                            await ThreadingService.ExecuteOnUiThreadAsync();
                            await AppInstance.FilesystemViewModel?.SetWorkingDirectoryAsync(newPath);

                            return true;
                        }
                    }
                }
            }
            else if (BaseProperties is CombinedProperties combinedProps)
            {
                // Handle the visibility attribute for multiple files
                if (AppInstance?.SlimContentPage?.ItemManipulationModel != null) // null on homepage
                {
                    await ThreadingService.ExecuteOnUiThreadAsync();
                    foreach (var fileOrFolder in combinedProps.List)
                    {
                        UIFilesystemHelpers.SetHiddenAttributeItem(fileOrFolder, ViewModel.IsHidden, AppInstance.SlimContentPage.ItemManipulationModel);
                    }
                }
                return true;
            }
            else
            {
                await ThreadingService.ExecuteOnUiThreadAsync();

                // Handle the visibility attribute for a single file
                if (AppInstance?.SlimContentPage?.ItemManipulationModel != null) // null on homepage
                {
                    UIFilesystemHelpers.SetHiddenAttributeItem(item, ViewModel.IsHidden, AppInstance.SlimContentPage.ItemManipulationModel);
                }

                ViewModel.ItemName = ItemFileName.Text; // Make sure ItemName is updated
                if (!string.IsNullOrWhiteSpace(ViewModel.ItemName) && ViewModel.OriginalItemName != ViewModel.ItemName)
                {
                    return await UIFilesystemHelpers.RenameFileItemAsync(item, ViewModel.ItemName, AppInstance);
                }
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
        }

        private void DiskCleanupButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (BaseProperties is DriveProperties driveProps)
            {
                var drive = driveProps.Drive;

                StorageSenseHelper.OpenStorageSense(drive.Path);
            }
        }
    }
}