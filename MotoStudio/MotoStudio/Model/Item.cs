using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoStudio.Model
{
    public class Item : ITreeItem
    {
        private string name;
        private string[] segments;

        public IList<ITreeItem> Items { get; private set; }

        public Item(string[] segments, bool isFile)
        {
            Items = new List<ITreeItem>();
            this.name = segments[0];
            if (segments.Length > 1)
                this.Items.Add(new Item(segments.Skip(1).ToArray(), isFile));
            else
                this.IsFile = isFile;
        }

        public string Name { get { return name; } }
        public bool IsFile { get; private set; }
    }
}
