using System.Windows.Forms;

namespace GitExtensions.GitLab.UI
{
    public partial class CreateMergeRequestDialog : Form
    {
        private readonly IGitLabService _gitLabService;

        public CreateMergeRequestDialog(IGitLabService gitLabService)
        {
            InitializeComponent();
            _gitLabService = gitLabService;
        }

        public MergeRequest GetMergeRequest()
        {
            return new MergeRequest
            {
                SourceBranch = sourceBranchTextBox.Text,
                TargetBranch = targetBranchTextBox.Text,
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text
            };
        }
    }
}
