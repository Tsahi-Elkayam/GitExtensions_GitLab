using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitExtensions.GitLab.Models;

namespace GitExtensions.GitLab.Services
{
    public interface IGitLabService : IDisposable
    {
        // Initialization
        void Initialize(PluginConfiguration configuration);

        // Pipeline Management
        Task<IEnumerable<Pipeline>> GetPipelines();
        Task<Pipeline> GetPipeline(string pipelineId);
        Task<string> GetPipelineUrl(string pipelineId);
        Task RetryPipeline(string pipelineId);
        Task CancelPipeline(string pipelineId);
        Task<IEnumerable<PipelineJob>> GetPipelineJobs(string pipelineId);

        // Merge Request Management
        Task<IEnumerable<MergeRequest>> GetMergeRequests();
        Task<MergeRequest> GetMergeRequest(string mergeRequestIid);
        Task<MergeRequest> CreateMergeRequest(string sourceBranch, string targetBranch, string title, string description);
        Task<MergeRequest> UpdateMergeRequest(string mergeRequestIid, Dictionary<string, object> updates);
        Task AcceptMergeRequest(string mergeRequestIid, string mergeCommitMessage = null);
        Task CloseMergeRequest(string mergeRequestIid);

        // MR Discussion Management
        Task<IEnumerable<Discussion>> GetMergeRequestDiscussions(string mergeRequestIid);
        Task AddMergeRequestComment(string mergeRequestIid, string body);
        Task ResolveMergeRequestDiscussion(string mergeRequestIid, string discussionId);

        // Review Management
        Task ApproveMergeRequest(string mergeRequestIid);
        Task RevokeMergeRequestApproval(string mergeRequestIid);
        Task<IEnumerable<User>> GetMergeRequestApprovers(string mergeRequestIid);

        // Branch Management
        Task<bool> IsBranchProtected(string branchName);
        Task<bool> HasBranchAccess(string branchName);
        Task<IEnumerable<string>> GetProtectedBranches();

        // Project Management
        Task<Project> GetProject(string projectId);
        Task<IEnumerable<ProjectMember>> GetProjectMembers();
        Task<IEnumerable<string>> GetProjectLabels();

        // Commit Management
        Task<Commit> GetCommit(string sha);
        Task<IEnumerable<Commit>> GetCommits(string branchName, int limit = 20);

        // Pipeline Artifact Management
        Task<IEnumerable<PipelineArtifact>> GetPipelineArtifacts(string pipelineId);
        Task DownloadArtifact(string pipelineId, string artifactId, string downloadPath);

        // Status Checks
        Task<bool> IsMergeAllowed(string mergeRequestIid);
        Task<bool> HasPipelinePassed(string pipelineId);
        Task<bool> IsBranchSafe(string branchName);

        // Cache Management
        void ClearCache();
        void InvalidatePipelineCache();
        void InvalidateMergeRequestCache();

        // Additional Models
        public class Project
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string DefaultBranch { get; set; }
            public string WebUrl { get; set; }
            public bool IsPublic { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class ProjectMember
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string AccessLevel { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        public class Commit
        {
            public string Id { get; set; }
            public string ShortId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string AuthorName { get; set; }
            public string AuthorEmail { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string AvatarUrl { get; set; }
            public string WebUrl { get; set; }
        }

        public class Discussion
        {
            public string Id { get; set; }
            public List<Note> Notes { get; set; } = new List<Note>();
            public bool Individual { get; set; }
            public bool Resolved { get; set; }
            public User ResolvedBy { get; set; }
        }

        public class Note
        {
            public string Id { get; set; }
            public string Body { get; set; }
            public User Author { get; set; }
            public DateTime CreatedAt { get; set; }
            public bool System { get; set; }
            public bool Resolvable { get; set; }
            public bool Resolved { get; set; }
        }
    }
}
