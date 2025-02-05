using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GitExtensions.GitLab.Services;
using GitExtensions.GitLab.Models;

namespace GitExtensions.GitLab.UI
{
    public partial class PipelinesDialog : Form
    {
        private readonly IGitLabService _gitLabService;
        private List<Pipeline> _pipelines;
        private bool _isLoading;
        private string _currentBranchFilter = "";
        private string _currentStatusFilter = "All";

        public PipelinesDialog(IGitLabService gitLabService)
        {
            InitializeComponent();
            _gitLabService = gitLabService;
            _pipelines = new List<Pipeline>();

            // Set up initial state
            InitializeDialog();
        }

        private void InitializeDialog()
        {
            // Set up default values
            filterComboBox.SelectedIndex = 0; // "All"

            // Set up image list for status icons
            var imageList = new ImageList();
            imageList.Images.Add("success", Properties.Resources.StatusSuccess);
            imageList.Images.Add("failed", Properties.Resources.StatusFailed);
            imageList.Images.Add("running", Properties.Resources.StatusRunning);
            imageList.Images.Add("pending", Properties.Resources.StatusPending);
            imageList.Images.Add("canceled", Properties.Resources.StatusCanceled);
            pipelineListView.SmallImageList = imageList;

            // Load initial data
            LoadPipelines();
        }

        private async void LoadPipelines()
        {
            try
            {
                SetLoadingState(true);

                // Get pipelines from service
                var pipelines = await _gitLabService.GetPipelines();
                _pipelines = pipelines.ToList();

                // Apply current filters
                UpdatePipelineList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading pipelines: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void UpdatePipelineList()
        {
            pipelineListView.BeginUpdate();
            pipelineListView.Items.Clear();

            var filteredPipelines = _pipelines
                .Where(p => MatchesBranchFilter(p))
                .Where(p => MatchesStatusFilter(p))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            foreach (var pipeline in filteredPipelines)
            {
                var item = new ListViewItem(new[]
                {
                    pipeline.Id,
                    pipeline.Status,
                    pipeline.Branch,
                    pipeline.CommitSha.Substring(0, 8),
                    FormatDuration(pipeline.Duration),
                    pipeline.CreatedAt.ToLocalTime().ToString("g")
                })
                {
                    ImageKey = pipeline.Status.ToLower(),
                    Tag = pipeline
                };

                // Set item colors based on status
                UpdateItemColor(item, pipeline.Status);

                pipelineListView.Items.Add(item);
            }

            pipelineListView.EndUpdate();
            UpdateStatusBar(filteredPipelines.Count);
        }

        private bool MatchesBranchFilter(Pipeline pipeline)
        {
            if (string.IsNullOrWhiteSpace(_currentBranchFilter))
                return true;

            return pipeline.Branch.Contains(_currentBranchFilter, StringComparison.OrdinalIgnoreCase);
        }

        private bool MatchesStatusFilter(Pipeline pipeline)
        {
            if (_currentStatusFilter == "All")
                return true;

            return pipeline.Status.Equals(_currentStatusFilter, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateItemColor(ListViewItem item, string status)
        {
            switch (status.ToLower())
            {
                case "success":
                    item.ForeColor = Color.Green;
                    break;
                case "failed":
                    item.ForeColor = Color.Red;
                    break;
                case "running":
                    item.ForeColor = Color.Blue;
                    break;
                case "canceled":
                    item.ForeColor = Color.Gray;
                    break;
            }
        }

        private string FormatDuration(TimeSpan? duration)
        {
            if (!duration.HasValue)
                return "-";

            if (duration.Value.TotalHours >= 1)
                return $"{duration.Value.TotalHours:F1}h";
            if (duration.Value.TotalMinutes >= 1)
                return $"{duration.Value.TotalMinutes:F0}m";
            return $"{duration.Value.TotalSeconds:F0}s";
        }

        private void UpdateStatusBar(int count)
        {
            statusLabel.Text = $"Showing {count} pipeline{(count != 1 ? "s" : "")}";
        }

        private void SetLoadingState(bool loading)
        {
            _isLoading = loading;
            refreshButton.Enabled = !loading;
            filterComboBox.Enabled = !loading;
            branchFilterBox.Enabled = !loading;

            if (loading)
                statusLabel.Text = "Loading pipelines...";
        }

        #region Event Handlers

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadPipelines();
        }

        private void FilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentStatusFilter = filterComboBox.SelectedItem.ToString();
            UpdatePipelineList();
        }

        private void BranchFilterBox_TextChanged(object sender, EventArgs e)
        {
            _currentBranchFilter = branchFilterBox.Text;
            UpdatePipelineList();
        }

        private void PipelineListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var hasSelection = pipelineListView.SelectedItems.Count > 0;
            retryPipelineMenuItem.Enabled = hasSelection;
            cancelPipelineMenuItem.Enabled = hasSelection;

            if (hasSelection)
            {
                var pipeline = (Pipeline)pipelineListView.SelectedItems[0].Tag;
                retryPipelineMenuItem.Enabled = pipeline.Status.ToLower() == "failed";
                cancelPipelineMenuItem.Enabled = pipeline.Status.ToLower() == "running";
            }
        }

        private void PipelineListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && pipelineListView.SelectedItems.Count > 0)
            {
                contextMenu.Show(pipelineListView, e.Location);
            }
        }

        private async void OpenInBrowser_Click(object sender, EventArgs e)
        {
            if (pipelineListView.SelectedItems.Count == 0)
                return;

            var pipeline = (Pipeline)pipelineListView.SelectedItems[0].Tag;
            var url = await _gitLabService.GetPipelineUrl(pipeline.Id);
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private async void RetryPipeline_Click(object sender, EventArgs e)
        {
            if (pipelineListView.SelectedItems.Count == 0)
                return;

            var pipeline = (Pipeline)pipelineListView.SelectedItems[0].Tag;

            try
            {
                await _gitLabService.RetryPipeline(pipeline.Id);
                LoadPipelines();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error retrying pipeline: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void CancelPipeline_Click(object sender, EventArgs e)
        {
            if (pipelineListView.SelectedItems.Count == 0)
                return;

            var pipeline = (Pipeline)pipelineListView.SelectedItems[0].Tag;

            var result = MessageBox.Show(
                $"Are you sure you want to cancel pipeline #{pipeline.Id}?",
                "Confirm Cancel",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    await _gitLabService.CancelPipeline(pipeline.Id);
                    LoadPipelines();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error canceling pipeline: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}
