using System;
using System.Collections.Generic;

namespace Files.Sdk.Models.Coloring
{
    [Serializable]
    public abstract class ColorModel : ICustomFormattable
    {
        public virtual IReadOnlyCollection<string>? Formats { get; }

        public virtual bool AppendFormat(string formatInfo) => false;
    }
}
