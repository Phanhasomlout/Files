﻿using System.ComponentModel;
using System.Threading.Tasks;

namespace Files.Sdk.Services
{
    public interface IUpdateServiceDeprecated : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether updates are available.
        /// </summary>
        bool IsUpdateAvailable { get; }

        /// <summary>
        /// Gets a value indicating if an update is in progress.
        /// </summary>
        bool IsUpdating { get; }

        int DownloadPercentage { get; }

        Task DownloadUpdates();

        Task DownloadMandatoryUpdates();

        Task CheckForUpdates();
    }
}
