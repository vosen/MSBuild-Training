using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotoStudio.Model
{
    public interface ITreeItem
    {
        IList<ITreeItem> Items { get; }
        string Name { get; }
        bool IsFolder { get; }
        ITreeItem Parent { get; }
    }
}
