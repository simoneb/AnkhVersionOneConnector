using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;

namespace AnkhVersionOneConnector.Impl.Controls
{
	public partial class IssuesListView : UserControl
	{
	    private readonly WorkItemService _workItemService;

	    public IssuesListView(WorkItemService workItemService) : this()
	    {
	        _workItemService = workItemService;
	    }

	    public IssuesListView()
		{
			InitializeComponent();

	        Load += LoadWorkItems;
            _list.MouseUp += ListOnMouseUp;
		}

	    private void ListOnMouseUp(object sender, MouseEventArgs mouseEventArgs)
	    {
            var selectedNode = _list.GetNodeAt(mouseEventArgs.X, mouseEventArgs.Y);

            _list.SelectedNode = selectedNode;

            contextMenuStrip1.Items[1].Enabled = selectedNode != null;
	    }

	    private void LoadWorkItems(object sender, EventArgs eventArgs)
	    {
	        RefreshTree(_workItemService.GetWorkItems());
	    }

	    private static TreeNode[] GetAllNodes(IEnumerable<VersionOneWorkItem> workItems)
	    {
	        return workItems.Select(GetNode).ToArray();
	    }

	    private static TreeNode GetNode(VersionOneWorkItem wi)
	    {
	        return new TreeNode(string.Format("{0} - {1}", wi.DisplayId, wi.Name),
	                            wi.Children.Select(c => new TreeNode(string.Format("{0} - {1}", c.DisplayId, c.Name)) {Tag = c}).ToArray()) {Tag = wi};
	    }

	    private void RefreshTree(ICollection<VersionOneWorkItem> workItems)
	    {
	        if (InvokeRequired)
	            Invoke(new Action<ICollection<VersionOneWorkItem>>(DoRefreshTree), new object[] {workItems});
	        else
	            DoRefreshTree(workItems);
	    }

	    private void DoRefreshTree(ICollection<VersionOneWorkItem> workItems)
	    {
	        _list.Nodes.Clear();
	        _list.Nodes.AddRange(GetAllNodes(workItems));
	    }

	    private void ForceReloadWorkItems(object sender, EventArgs eventArgs)
	    {
            RefreshTree(_workItemService.GetWorkItems(true));
	    }

	    public IEnumerable<VersionOneWorkItem> CheckedWorkItems
	    {
            get { return (IEnumerable<VersionOneWorkItem>)(InvokeRequired ? Invoke(new Func<IEnumerable<VersionOneWorkItem>>(GetCheckedWorkItems)) : GetCheckedWorkItems()); }
	    }

        private IEnumerable<VersionOneWorkItem> GetCheckedWorkItems()
	    {
            return AllNodes().Where(i => i.Checked).Select(i => i.Tag).Cast<VersionOneWorkItem>();
	    }

	    public void ClearCheckedWorkItems()
	    {
            if(InvokeRequired)
	            Invoke(new Action(DoClearCheckedWorkItems));
            else
                DoClearCheckedWorkItems();
	    }

	    private void DoClearCheckedWorkItems()
	    {
	        foreach (var node in AllNodes())
	            node.Checked = false;
	    }

	    private IEnumerable<TreeNode> AllNodes(TreeNodeCollection root = null)
	    {
	        foreach (TreeNode node in root ?? _list.Nodes)
	        {
	            yield return node;

	            foreach (var child in AllNodes(node.Nodes))
	                yield return child;
	        }
	    }

        private void openInVersionOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = _list.SelectedNode;

            if (selectedNode == null)
                return;

            Process.Start(((VersionOneWorkItem) selectedNode.Tag).Url);
        }
	}
}