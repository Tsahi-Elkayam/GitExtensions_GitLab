using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GitExtensions.Plugin;
using GitExtensions.GitLab.Services;
using GitExtensions.GitLab.UI;
using GitUI;
using ResourceManager;

namespace GitExtensions.GitLab
{
    public class GitLabPlugin : GitPluginBase, IGitPluginForRepository
    {
        private readonly IGitLabService _gitLabService;
        private readonly TranslationString _pipelineMenuText = new("Pipelines");
        private readonly TranslationString _createMergeRequestText = new("Create Merge Request");
        private readonly TranslationString _viewMergeRequestsText = new("View Merge Requests");
        private ToolStripMenuItem _gitLabMenu;

        public GitLabPlugin()
        {
            _gitLabService = new GitLabService();
            SetNameAndDescription("GitLab Integration");
        }

        public override bool Execute(GitUIEventArgs args)
        {
            return false;
        }

        private void SetNameAndDescription(string name)
        {
            Description = "GitLab integration for Git Extensions";
            Name = name;
        }

        public override IEnumerable<ISetting> GetSettings()
        {
            yield return new StringSetting("GitLab URL", "URL of your GitLab instance", string.Empty);
            yield return new PasswordSetting("API Token", "Your GitLab API token", string.Empty);
            yield return new StringSetting("Project ID", "Your GitLab project ID", string.Empty);
            yield return new BoolSetting("Auto Pipeline Check", "Automatically check pipeline status", false);
            yield return new BoolSetting("Show MR Notifications", "Show merge request notifications", true);
        }

        public override void Register(IGitUICommands gitUiCommands)
        {
            base.Register(gitUiCommands);

            _gitLabService.Initialize(LoadConfiguration());
            RegisterMenu(gitUiCommands);

            gitUiCommands.PostCommit += GitUICommands_PostCommit;
            gitUiCommands.PostPush += GitUICommands_PostPush;
            gitUiCommands.PostCheckoutBranch += GitUICommands_PostCheckoutBranch;
        }

        private void RegisterMenu(IGitUICommands gitUiCommands)
        {
            if (_gitLabMenu == null)
            {
                _gitLabMenu = new ToolStripMenuItem("GitLab");

                var pipelinesMenuItem = new ToolStripMenuItem(_pipelineMenuText.Text)
                {
                    Image = Properties.Resources.PipelineIcon
                };
                pipelinesMenuItem.Click += PipelinesMenuItem_Click;

                var createMergeRequestMenuItem = new ToolStripMenuItem(_createMergeRequestText.Text)
                {
                    Image = Properties.Resources.MergeRequestIcon
                };
                createMergeRequestMenuItem.Click += CreateMergeRequestMenuItem_Click;

                var viewMergeRequestsMenuItem = new ToolStripMenuItem(_viewMergeRequestsText.Text)
                {
                    Image = Properties.Resources.MergeRequestsIcon
                };
                viewMergeRequestsMenuItem.Click += ViewMergeRequestsMenuItem_Click;

                _gitLabMenu.DropDownItems.AddRange(new ToolStripItem[]
                {
                    pipelinesMenuItem,
                    new ToolStripSeparator(),
                    createMergeRequestMenuItem,
                    viewMergeRequestsMenuItem
                });
            }

            gitUiCommands.AddMenuCommand("GitLab", _gitLabMenu);
        }

        private void PipelinesMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new PipelinesDialog(_gitLabService);
            dialog.ShowDialog();
        }

        private void CreateMergeRequestMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new CreateMergeRequestDialog(_gitLabService);
            dialog.ShowDialog();
        }

        private void ViewMergeRequestsMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new MergeRequestsDialog(_gitLabService);
            dialog.ShowDialog();
        }

        private void GitUICommands_PostCommit(object sender, GitUIEventArgs e)
        {
            if (GetSetting("Auto Pipeline Check", false))
            {
                CheckPipelineStatus();
            }
        }

        private void GitUICommands_PostPush(object sender, GitUIEventArgs e)
        {
            var pushInfo = e.GitModule.GetPushInfo();
            if (pushInfo.IsNewBranch)
            {
                OfferCreateMergeRequest(pushInfo.BranchName);
            }
        }

        private void GitUICommands_PostCheckoutBranch(object sender, GitUIEventArgs e)
        {
            if (GetSetting("Show MR Notifications", true))
            {
                CheckMergeRequestStatus(e.GitModule.GetSelectedBranch());
            }
        }

        private void CheckPipelineStatus()
        {
            try
            {
                var pipelines = _gitLabService.GetPipelines().Result;
                var latestPipeline = pipelines.FirstOrDefault();

                if (latestPipeline != null && latestPipeline.Status != "success")
                {
                    MessageBox.Show(
                        $"Latest pipeline status: {latestPipeline.Status}",
                        "Pipeline Status",
                        MessageBoxButtons.OK,
                        latestPipeline.Status == "failed" ? MessageBoxIcon.Error : MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user for automated checks
                System.Diagnostics.Debug.WriteLine($"Error checking pipeline status: {ex.Message}");
            }
        }

        private void OfferCreateMergeRequest(string branchName)
        {
            var result = MessageBox.Show(
                $"Would you like to create a merge request for branch '{branchName}'?",
                "Create Merge Request",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using var dialog = new CreateMergeRequestDialog(_gitLabService, branchName);
                dialog.ShowDialog();
            }
        }

        private async void CheckMergeRequestStatus(string branchName)
        {
            try
            {
                var mergeRequests = await _gitLabService.GetMergeRequests();
                var relatedMR = mergeRequests.FirstOrDefault(mr =>
                    mr.SourceBranch == branchName || mr.TargetBranch == branchName);

                if (relatedMR != null)
                {
                    var message = relatedMR.SourceBranch == branchName
                        ? $"This branch has an open merge request: {relatedMR.Title}"
                        : $"This branch is the target of merge request: {relatedMR.Title}";

                    MessageBox.Show(
                        message,
                        "Merge Request Status",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show to user for automated checks
                System.Diagnostics.Debug.WriteLine($"Error checking merge request status: {ex.Message}");
            }
        }

        private PluginConfiguration LoadConfiguration()
        {
            return new PluginConfiguration
            {
                GitLabUrl = GetSetting("GitLab URL", string.Empty),
                ApiToken = GetSetting("API Token", string.Empty),
                DefaultProject = GetSetting("Project ID", string.Empty),
                AutoPipelineCheck = GetSetting("Auto Pipeline Check", false),
                ShowMergeRequestNotifications = GetSetting("Show MR Notifications", true)
            };
        }

        public override void Unregister(IGitUICommands gitUiCommands)
        {
            gitUiCommands.PostCommit -= GitUICommands_PostCommit;
            gitUiCommands.PostPush -= GitUICommands_PostPush;
            gitUiCommands.PostCheckoutBranch -= GitUICommands_PostCheckoutBranch;

            _gitLabService.Dispose();
            base.Unregister(gitUiCommands);
        }
    }
}
