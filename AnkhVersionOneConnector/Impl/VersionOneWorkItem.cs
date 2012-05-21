using System.Collections.Generic;
using System.Linq;
using VersionOne.SDK.ObjectModel;

namespace AnkhVersionOneConnector.Impl
{
    public class VersionOneWorkItem
    {
        public VersionOneWorkItem(Workitem self, IEnumerable<Workitem> children = null, VersionOneWorkItem parent = null)
        {
            Parent = parent;
            DisplayId = self.DisplayID;
            Name = self.Name;

            Children = children != null
                ? children.Select(i => new VersionOneWorkItem(i, parent: this))
                           : Enumerable.Empty<VersionOneWorkItem>();
        }

        public IEnumerable<VersionOneWorkItem> Children { get; private set; }
        public VersionOneWorkItem Parent { get; set; }

        public string Name { get; private set; }

        public string DisplayId { get; private set; }

        public bool HasParent
        {
            get { return Parent != null; }
        }
    }
}