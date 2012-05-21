using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VersionOne.SDK.ObjectModel;
using VersionOne.SDK.ObjectModel.Filters;
using Task = System.Threading.Tasks.Task;

namespace AnkhVersionOneConnector.Impl
{
    public class WorkItemService : IDisposable
    {
        private readonly V1Instance _instance;
        private Task<ICollection<VersionOneWorkItem>> _task;
        private ICollection<VersionOneWorkItem> _currentWorkItems;

        public WorkItemService(V1Instance instance)
        {
            _instance = instance;

            _task = Task.Factory.StartNew<ICollection<VersionOneWorkItem>>(LoadWorkItems);
        }

        private ICollection<VersionOneWorkItem> LoadWorkItems()
        {
            var currentUser = _instance.Get.MemberByUserName(_instance.LoggedInMember.Username);
            return Convert(_instance.Get.Workitems(new WorkitemFilter { Owners = { currentUser }, State = { State.Active } }));
        }

        private static ICollection<VersionOneWorkItem> Convert(IEnumerable<Workitem> workItems)
        {
            return workItems.GroupBy(wi => wi is PrimaryWorkitem ? wi : ((SecondaryWorkitem)wi).Parent).Select(g => new VersionOneWorkItem(g.Key, g.Except(new[]{g.Key}))).ToList();
        }

        public ICollection<VersionOneWorkItem> GetWorkItems(bool forceReload = false)
        {
            return forceReload ? (_currentWorkItems = LoadWorkItems()) : (_currentWorkItems ?? _task.Result);
        }

        public void Dispose()
        {
            if(_task != null)
            {
                _task.Dispose();
                _task = null;
            }
        }
    }
}