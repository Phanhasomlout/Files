using System.Collections.Generic;

namespace Files.Sdk.Models
{
    public interface ICustomFormattable
    {
        IReadOnlyCollection<string>? Formats { get; }

        bool AppendFormat(string formatInfo);
    }
}
