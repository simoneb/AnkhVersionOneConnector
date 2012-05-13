using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VersionOne.SDK.ObjectModel;
using System.Linq;

namespace AnkhVersionOneConnector.Impl.Controls
{
	public partial class IssuesListView : UserControl
	{
	    private readonly WorkItemService _workItemService;

	    public IssuesListView(WorkItemService workItemService) : this()
	    {
	        _workItemService = workItemService;
	        _workItemService.WorkItemsLoaded += ReloadWorkItems;
	    }

	    public IssuesListView()
		{
			InitializeComponent();
		}

	    private void ReloadWorkItems(ICollection<Workitem> workItems)
	    {
	        var selection = SelectedWorkItems.Select(i => i.DisplayID).ToArray();

	        _list.Items.Clear();
	        _list.Items.AddRange(Convert(workItems));

	        foreach (var item in _list.Items.Cast<ListViewItem>().Join(selection, l => l.Text, s => s, (l, s) => l))
	            item.Checked = true;
	    }

	    private void ForceReloadWorkItems(object sender, EventArgs eventArgs)
	    {
	        ReloadWorkItems(_workItemService.GetWorkItems());
	    }

	    private static ListViewItem[] Convert(IEnumerable<Workitem> workItems)
	    {
            return workItems.Select(i => new ListViewItem(i.DisplayID){SubItems = { i.Name }, Tag = i}).ToArray();
	    }

	    public IEnumerable<Workitem> SelectedWorkItems
	    {
	        get { return (IEnumerable<Workitem>) (InvokeRequired ? Invoke(new Func<IEnumerable<Workitem>>(GetSelectedWorkItems)) : GetSelectedWorkItems()); }
	    }

	    private IEnumerable<Workitem> GetSelectedWorkItems()
	    {
	        return _list.Items.Cast<ListViewItem>().Where(i => i.Checked).Select(i => i.Tag).Cast<Workitem>();
	    }

	    public void ClearSelection()
	    {
            if(InvokeRequired)
	            Invoke(new Action(DoClearSelection));
            else
                DoClearSelection();
	    }

	    private void DoClearSelection()
	    {
	        foreach (ListViewItem item in _list.Items)
	            item.Checked = false;
	    }
	}
}