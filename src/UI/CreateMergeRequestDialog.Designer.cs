namespace GitExtensions.GitLab.UI
{
    partial class CreateMergeRequestDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

            // Branch selection group
            this.branchGroupBox = new System.Windows.Forms.GroupBox();
            this.sourceBranchLabel = new System.Windows.Forms.Label();
            this.sourceBranchComboBox = new System.Windows.Forms.ComboBox();
            this.targetBranchLabel = new System.Windows.Forms.Label();
            this.targetBranchComboBox = new System.Windows.Forms.ComboBox();

            // Merge request details group
            this.detailsGroupBox = new System.Windows.Forms.GroupBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();

            // Options group
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.removeSourceBranchCheckBox = new System.Windows.Forms.CheckBox();
            this.squashCommitsCheckBox = new System.Windows.Forms.CheckBox();
            this.assignToMeCheckBox = new System.Windows.Forms.CheckBox();

            // Template selection
            this.templateLabel = new System.Windows.Forms.Label();
            this.templateComboBox = new System.Windows.Forms.ComboBox();

            // Labels selection
            this.labelsLabel = new System.Windows.Forms.Label();
            this.labelsCheckedListBox = new System.Windows.Forms.CheckedListBox();

            // Reviewers selection
            this.reviewersLabel = new System.Windows.Forms.Label();
            this.reviewersCheckedListBox = new System.Windows.Forms.CheckedListBox();

            // Buttons
            this.createButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.previewButton = new System.Windows.Forms.Button();

            // Error provider
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);

            // Branch group setup
            this.branchGroupBox.SuspendLayout();
            this.branchGroupBox.Text = "Branch Selection";
            this.branchGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.branchGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.branchGroupBox.Height = 100;

            this.sourceBranchLabel.Text = "Source Branch:";
            this.sourceBranchLabel.AutoSize = true;
            this.sourceBranchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceBranchComboBox.Width = 300;

            this.targetBranchLabel.Text = "Target Branch:";
            this.targetBranchLabel.AutoSize = true;
            this.targetBranchComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.targetBranchComboBox.Width = 300;

            // Details group setup
            this.detailsGroupBox.SuspendLayout();
            this.detailsGroupBox.Text = "Merge Request Details";
            this.detailsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.detailsGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.detailsGroupBox.Height = 200;

            this.titleLabel.Text = "Title:";
            this.titleLabel.AutoSize = true;
            this.titleTextBox.Width = 500;

            this.descriptionLabel.Text = "Description:";
            this.descriptionLabel.AutoSize = true;
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descriptionTextBox.Height = 100;
            this.descriptionTextBox.Width = 500;

            // Options group setup
            this.optionsGroupBox.SuspendLayout();
            this.optionsGroupBox.Text = "Options";
            this.optionsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.optionsGroupBox.Padding = new System.Windows.Forms.Padding(10);
            this.optionsGroupBox.Height = 100;

            this.removeSourceBranchCheckBox.Text = "Delete source branch when merge request is accepted";
            this.removeSourceBranchCheckBox.AutoSize = true;

            this.squashCommitsCheckBox.Text = "Squash commits when merge request is accepted";
            this.squashCommitsCheckBox.AutoSize = true;

            this.assignToMeCheckBox.Text = "Assign to me";
            this.assignToMeCheckBox.AutoSize = true;

            // Template setup
            this.templateLabel.Text = "Template:";
            this.templateLabel.AutoSize = true;
            this.templateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateComboBox.Width = 300;

            // Labels setup
            this.labelsLabel.Text = "Labels:";
            this.labelsLabel.AutoSize = true;
            this.labelsCheckedListBox.CheckOnClick = true;
            this.labelsCheckedListBox.Height = 100;
            this.labelsCheckedListBox.Width = 200;

            // Reviewers setup
            this.reviewersLabel.Text = "Reviewers:";
            this.reviewersLabel.AutoSize = true;
            this.reviewersCheckedListBox.CheckOnClick = true;
            this.reviewersCheckedListBox.Height = 100;
            this.reviewersCheckedListBox.Width = 200;

            // Buttons setup
            this.createButton.Text = "Create";
            this.createButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.createButton.Width = 75;

            this.cancelButton.Text = "Cancel";
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Width = 75;

            this.previewButton.Text = "Preview";
            this.previewButton.Width = 75;

            // Form setup
            this.AcceptButton = this.createButton;
            this.CancelButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Text = "Create Merge Request";
            this.Icon = Properties.Resources.MergeRequestIcon;

            // Layout using TableLayoutPanel
            var mainLayout = new System.Windows.Forms.TableLayoutPanel
            {
                RowCount = 7,
                ColumnCount = 2,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(10)
            };

            mainLayout.Controls.Add(this.branchGroupBox, 0, 0);
            mainLayout.Controls.Add(this.detailsGroupBox, 0, 1);
            mainLayout.Controls.Add(this.optionsGroupBox, 0, 2);

            var rightPanel = new System.Windows.Forms.TableLayoutPanel
            {
                RowCount = 3,
                ColumnCount = 1,
                Dock = System.Windows.Forms.DockStyle.Fill
            };

            rightPanel.Controls.Add(this.labelsCheckedListBox, 0, 0);
            rightPanel.Controls.Add(this.reviewersCheckedListBox, 0, 1);
            rightPanel.Controls.Add(this.templateComboBox, 0, 2);

            mainLayout.Controls.Add(rightPanel, 1, 0);
            mainLayout.SetRowSpan(rightPanel, 3);

            var buttonPanel = new System.Windows.Forms.FlowLayoutPanel
            {
                Dock = System.Windows.Forms.DockStyle.Bottom,
                FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft,
                Height = 40
            };

            buttonPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.cancelButton,
                this.createButton,
                this.previewButton
            });

            mainLayout.Controls.Add(buttonPanel, 0, 6);
            mainLayout.SetColumnSpan(buttonPanel, 2);

            this.Controls.Add(mainLayout);

            // Events
            this.sourceBranchComboBox.SelectedIndexChanged += new System.EventHandler(this.SourceBranchComboBox_SelectedIndexChanged);
            this.targetBranchComboBox.SelectedIndexChanged += new System.EventHandler(this.TargetBranchComboBox_SelectedIndexChanged);
            this.titleTextBox.TextChanged += new System.EventHandler(this.TitleTextBox_TextChanged);
            this.templateComboBox.SelectedIndexChanged += new System.EventHandler(this.TemplateComboBox_SelectedIndexChanged);
            this.previewButton.Click += new System.EventHandler(this.PreviewButton_Click);

            this.branchGroupBox.ResumeLayout(false);
            this.detailsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox branchGroupBox;
        private System.Windows.Forms.Label sourceBranchLabel;
        private System.Windows.Forms.ComboBox sourceBranchComboBox;
        private System.Windows.Forms.Label targetBranchLabel;
        private System.Windows.Forms.ComboBox targetBranchComboBox;

        private System.Windows.Forms.GroupBox detailsGroupBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;

        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.CheckBox removeSourceBranchCheckBox;
        private System.Windows.Forms.CheckBox squashCommitsCheckBox;
        private System.Windows.Forms.CheckBox assignToMeCheckBox;

        private System.Windows.Forms.Label templateLabel;
        private System.Windows.Forms.ComboBox templateComboBox;

        private System.Windows.Forms.Label labelsLabel;
        private System.Windows.Forms.CheckedListBox labelsCheckedListBox;

        private System.Windows.Forms.Label reviewersLabel;
        private System.Windows.Forms.CheckedListBox reviewersCheckedListBox;

        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button previewButton;

        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
