using System;
using System.Collections.Generic;
using System.Linq;

namespace GitExtensions.GitLab.Models
{
    public class MergeRequest
    {
        // Basic merge request information
        public string Id { get; set; }
        public string Iid { get; set; } // Internal ID within project
        public string Title { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public bool Merged { get; set; }
        public bool Closed { get; set; }

        // Branch information
        public string SourceBranch { get; set; }
        public string TargetBranch { get; set; }
        public string SourceProjectId { get; set; }
        public string TargetProjectId { get; set; }

        // User information
        public User Author { get; set; }
        public User Assignee { get; set; }
        public List<User> Reviewers { get; set; } = new List<User>();

        // Pipeline information
        public Pipeline HeadPipeline { get; set; }
        public List<Pipeline> Pipelines { get; set; } = new List<Pipeline>();

        // Timing information
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? MergedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Discussion and review information
        public List<Discussion> Discussions { get; set; } = new List<Discussion>();
        public int UserNotesCount { get; set; }
        public List<string> Labels { get; set; } = new List<string>();

        // Merge status information
        public bool MergeWhenPipelineSucceeds { get; set; }
        public bool ShouldRemoveSourceBranch { get; set; }
        public bool ForceRemoveSourceBranch { get; set; }
        public bool AllowCollaboration { get; set; }
        public bool AllowMaintainerToPush { get; set; }

        // Review status
        public Dictionary<string, ReviewState> ReviewerStates { get; set; } = new Dictionary<string, ReviewState>();
        public int ApprovalsRequired { get; set; }
        public int ApprovalsLeft { get; set; }

        // Diff information
        public int ChangesCount { get; set; }
        public bool HasConflicts { get; set; }
        public List<string> ConflictFiles { get; set; } = new List<string>();

        // Web URLs
        public string WebUrl { get; set; }
        public string DiffUrl { get; set; }

        // Status helpers
        public bool IsOpen => !Merged && !Closed;
        public bool CanBeMerged => !HasConflicts && HeadPipeline?.IsSuccess == true;
        public bool IsReadyToMerge => CanBeMerged && ApprovalsLeft == 0;
        public bool NeedsReview => ApprovalsLeft > 0;
        public bool HasFailedPipeline => HeadPipeline?.IsFailed == true;

        // Time tracking
        public TimeSpan? TimeEstimate { get; set; }
        public TimeSpan? TotalTimeSpent { get; set; }

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
            public Dictionary<string, object> Position { get; set; }
        }

        public enum ReviewState
        {
            None,
            Pending,
            Approved,
            Rejected
        }

        // Helper methods for merge request management
        public bool CanBeApprovedBy(string userId)
        {
            return !ReviewerStates.ContainsKey(userId) ||
                   ReviewerStates[userId] == ReviewState.None ||
                   ReviewerStates[userId] == ReviewState.Rejected;
        }

        public bool IsReviewedBy(string userId)
        {
            return ReviewerStates.ContainsKey(userId) &&
                   ReviewerStates[userId] != ReviewState.None &&
                   ReviewerStates[userId] != ReviewState.Pending;
        }

        public List<Note> GetUnresolvedNotes()
        {
            return Discussions
                .SelectMany(d => d.Notes)
                .Where(n => n.Resolvable && !n.Resolved)
                .ToList();
        }

        public List<Discussion> GetUnresolvedDiscussions()
        {
            return Discussions
                .Where(d => !d.Resolved && d.Notes.Any(n => n.Resolvable))
                .ToList();
        }

        // Formatting helpers
        public string GetTimeEstimateFormatted()
        {
            return FormatTimeSpan(TimeEstimate);
        }

        public string GetTotalTimeSpentFormatted()
        {
            return FormatTimeSpan(TotalTimeSpent);
        }

        private string FormatTimeSpan(TimeSpan? timeSpan)
        {
            if (!timeSpan.HasValue)
                return "-";

            var hours = timeSpan.Value.TotalHours;
            if (hours >= 8)
                return $"{hours / 8:F1}d";
            if (hours >= 1)
                return $"{hours:F1}h";
            return $"{timeSpan.Value.TotalMinutes:F0}m";
        }

        public override string ToString()
        {
            return $"!{Iid} - {Title} ({State})";
        }
    }
}
