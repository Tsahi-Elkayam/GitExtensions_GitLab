using GitLabApiClient;
using System;
using System.Threading.Tasks;

namespace GitExtensions.GitLab.Services
{
    public class GitLabService : IGitLabService
    {
        private IGitLabClient _client;
        private PluginConfiguration _configuration;

        public void Initialize(PluginConfiguration configuration)
        {
            _configuration = configuration;
            _client = new GitLabClient(
                configuration.GitLabUrl,
                configuration.ApiToken);
        }

        public async Task<bool> IsBranchSafe(string branchName)
        {
            var pipeline = await _client.Pipelines
                .GetLatestPipelineForBranch(_configuration.DefaultProject, branchName);
            return pipeline?.Status == "success";
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}
