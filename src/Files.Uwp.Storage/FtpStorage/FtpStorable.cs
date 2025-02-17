﻿using System.Threading;
using System.Threading.Tasks;
using Files.Sdk.Storage.LocatableStorage;
using FluentFTP;

#nullable enable

namespace Files.Uwp.Storage.FtpStorage
{
    public abstract class FtpStorable : ILocatableStorable
    {
        /// <inheritdoc/>
        public string Path { get; protected set; }

        /// <inheritdoc/>
        public string Name { get; protected set; }

        /// <inheritdoc/>
        public string Id { get; protected set; }

        protected internal FtpStorable(string path, string name)
        {
            Path = FtpHelpers.GetFtpPath(path);
            Name = name;
            Id = string.Empty;
        }

        public virtual Task<ILocatableFolder?> GetParentAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<ILocatableFolder?>(null);
        }

        protected FtpClient GetFtpClient()
        {
            return FtpHelpers.GetFtpClient(Path);
        }
    }
}
