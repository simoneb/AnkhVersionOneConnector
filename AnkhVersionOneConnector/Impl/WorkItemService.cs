using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using VersionOne.SDK.ObjectModel;
using VersionOne.SDK.ObjectModel.Filters;

namespace AnkhVersionOneConnector.Impl
{
    public class WorkItemService : IDisposable
    {
        private readonly V1Instance _instance;

        private Timer _timer;
        private Action<ICollection<Workitem>> _workItemsLoaded = delegate{};
        private ICollection<Workitem> _currentWorkItems = new Collection<Workitem>();

        public event Action<ICollection<Workitem>> WorkItemsLoaded
        {
            add
            {
                _workItemsLoaded += value;

                if(_currentWorkItems.Any())
                    value(_currentWorkItems);
            }
            remove { _workItemsLoaded -= value; }
        }

        public WorkItemService(V1Instance instance)
        {
            _instance = instance;

            _timer = new Timer(ReloadWorkItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void ReloadWorkItems(object state)
        {
            var workItems = GetWorkItems();

            foreach (var @delegate in _workItemsLoaded.GetInvocationList().Cast<Action<ICollection<Workitem>>>())
            {
                try
                {
                    @delegate(workItems);
                }
                catch (Exception e)
                {
                    // ouch
                }
            }
        }

        public ICollection<Workitem> GetWorkItems()
        {
            var currentUser = _instance.Get.MemberByUserName(_instance.LoggedInMember.Username);
            return _currentWorkItems = _instance.Get.Workitems(new WorkitemFilter { Owners = { currentUser }, State = { State.Active } });
        }

        public void Dispose()
        {
            if(_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}