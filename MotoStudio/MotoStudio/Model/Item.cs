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

        public Item(ITreeItem parent, string[] segments, bool isFile)
        {
            Parent = parent;
            Items = new List<ITreeItem>();
            this.name = segments[0];
            if (segments.Length > 1)
                this.Items.Add(new Item(this, segments.Skip(1).ToArray(), isFile));
            else
                this.IsFolder = !isFile;
        }

        public string Name { get { return name; } }
        private bool folder = true;
        public bool IsFolder { get { return folder; } private set { folder = value; } }


        public ITreeItem Parent
        {
            get; private set;
        }
    }
}
