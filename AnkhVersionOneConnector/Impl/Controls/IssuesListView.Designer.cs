namespace AnkhVersionOneConnector.Impl.Controls
{
	partial class IssuesListView
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._list = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshListToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // refreshListToolStripMenuItem
            // 
            this.refreshListToolStripMenuItem.Name = "refreshListToolStripMenuItem";
            this.refreshListToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.refreshListToolStripMenuItem.Text = "Refresh list";
            this.refreshListToolStripMenuItem.Click += new System.EventHandler(this.ForceReloadWorkItems);
            // 
            // _list
            // 
            this._list.CheckBoxes = true;
            this._list.ContextMenuStrip = this.contextMenuStrip1;
            this._list.Dock = System.Windows.Forms.DockStyle.Fill;
            this._list.Location = new System.Drawing.Point(0, 0);
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(581, 141);
            this._list.TabIndex = 1;
            // 
            // IssuesListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._list);
            this.Name = "IssuesListView";
            this.Size = new System.Drawing.Size(581, 141);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem refreshListToolStripMenuItem;
        private System.Windows.Forms.TreeView _list;
	}
}
