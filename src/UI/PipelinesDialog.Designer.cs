namespace GitExtensions.GitLab.UI
{
    partial class PipelinesDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Main ListView for pipelines
            this.pipelineListView = new System.Windows.Forms.ListView();
            this.columnId = new System.Windows.Forms.ColumnHeader();
            this.columnStatus = new System.Windows.Forms.ColumnHeader();
            this.columnBranch = new System.Windows.Forms.ColumnHeader();
            this.columnCommit = new System.Windows.Forms.ColumnHeader();
            this.columnDuration = new System.Windows.Forms.ColumnHeader();
            this.columnStarted = new System.Windows.Forms.ColumnHeader();

            // ToolStrip with actions
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.refreshButton = new System.Windows.Forms.ToolStripButton();
            this.filterComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.branchFilterBox = new System.Windows.Forms.ToolStripTextBox();

            // Status bar
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();

            // Context menu for list items
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInBrowserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retryPipelineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelPipelineMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            // ListView setup
            this.pipelineListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pipelineListView.FullRowSelect = true;
            this.pipelineListView.GridLines = true;
            this.pipelineListView.MultiSelect = false;
            this.pipelineListView.View = System.Windows.Forms.View.Details;
            this.pipelineListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.columnId,
                this.columnStatus,
                this.columnBranch,
                this.columnCommit,
                this.columnDuration,
                this.columnStarted
            });

            // Column headers
            this.columnId.Text = "ID";
            this.columnId.Width = 70;
            this.columnStatus.Text = "Status";
            this.columnStatus.Width = 100;
            this.columnBranch.Text = "Branch";
            this.columnBranch.Width = 150;
            this.columnCommit.Text = "Commit";
            this.columnCommit.Width = 250;
            this.columnDuration.Text = "Duration";
            this.columnDuration.Width = 100;
            this.columnStarted.Text = "Started";
            this.columnStarted.Width = 150;

            // ToolStrip items
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.refreshButton,
                new System.Windows.Forms.ToolStripSeparator(),
                this.filterComboBox,
                this.branchFilterBox
            });
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Image = Properties.Resources.Refresh;
            this.filterComboBox.Items.AddRange(new object[] {
                "All",
                "Running",
                "Pending",
                "Success",
                "Failed",
                "Canceled"
            });
            this.filterComboBox.SelectedIndex = 0;
            this.branchFilterBox.PlaceholderText = "Filter by branch...";
            this.branchFilterBox.Width = 150;

            // Context menu items
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.openInBrowserMenuItem,
                new System.Windows.Forms.ToolStripSeparator(),
                this.retryPipelineMenuItem,
                this.cancelPipelineMenuItem
            });
            this.openInBrowserMenuItem.Text = "Open in browser";
            this.retryPipelineMenuItem.Text = "Retry pipeline";
            this.cancelPipelineMenuItem.Text = "Cancel pipeline";

            // Form setup
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.pipelineListView,
                this.toolStrip,
                this.statusStrip
            });
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Text = "GitLab Pipelines";
            this.Icon = Properties.Resources.GitLabIcon;

            // Event handlers
            this.pipelineListView.SelectedIndexChanged += new System.EventHandler(this.PipelineListView_SelectedIndexChanged);
            this.pipelineListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PipelineListView_MouseClick);
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            this.filterComboBox.SelectedIndexChanged += new System.EventHandler(this.FilterComboBox_SelectedIndexChanged);
            this.branchFilterBox.TextChanged += new System.EventHandler(this.BranchFilterBox_TextChanged);
            this.openInBrowserMenuItem.Click += new System.EventHandler(this.OpenInBrowser_Click);
            this.retryPipelineMenuItem.Click += new System.EventHandler(this.RetryPipeline_Click);
            this.cancelPipelineMenuItem.Click += new System.EventHandler(this.CancelPipeline_Click);
        }

        #endregion

        private System.Windows.Forms.ListView pipelineListView;
        private System.Windows.Forms.ColumnHeader columnId;
        private System.Windows.Forms.ColumnHeader columnStatus;
        private System.Windows.Forms.ColumnHeader columnBranch;
        private System.Windows.Forms.ColumnHeader columnCommit;
        private System.Windows.Forms.ColumnHeader columnDuration;
        private System.Windows.Forms.ColumnHeader columnStarted;

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton refreshButton;
        private System.Windows.Forms.ToolStripComboBox filterComboBox;
        private System.Windows.Forms.ToolStripTextBox branchFilterBox;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retryPipelineMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelPipelineMenuItem;
    }
}
