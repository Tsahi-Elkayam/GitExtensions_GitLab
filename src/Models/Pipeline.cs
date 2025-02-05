using System;
using System.Collections.Generic;

namespace GitExtensions.GitLab.Models
{
    public class Pipeline
    {
        // Basic pipeline information
        public string Id { get; set; }
        public string Status { get; set; }
        public string Branch { get; set; }
        public string CommitSha { get; set; }
        public string CommitMessage { get; set; }
        public string CommitAuthor { get; set; }

        // Timing information
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public TimeSpan? Duration => FinishedAt - StartedAt;

        // Pipeline details
        public string WebUrl { get; set; }
        public string Ref { get; set; }
        public string Tag { get; set; }
        public string Source { get; set; }
        public bool DetailedStatus { get; set; }

        // Pipeline stages and jobs
        public List<PipelineStage> Stages { get; set; } = new List<PipelineStage>();
        public List<PipelineJob> Jobs { get; set; } = new List<PipelineJob>();

        // Coverage information
        public decimal? Coverage { get; set; }
        public string TestReport { get; set; }

        // User information
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        // Project information
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPath { get; set; }

        // Variables and configuration
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();

        // Status helper methods
        public bool IsRunning => Status?.ToLower() == "running";
        public bool IsFailed => Status?.ToLower() == "failed";
        public bool IsSuccess => Status?.ToLower() == "success";
        public bool IsCanceled => Status?.ToLower() == "canceled";
        public bool IsPending => Status?.ToLower() == "pending";
        public bool IsCompleted => IsSuccess || IsFailed || IsCanceled;

        // Duration formatting
        public string FormattedDuration
        {
            get
            {
                if (!Duration.HasValue)
                    return "-";

                if (Duration.Value.TotalHours >= 1)
                    return $"{Duration.Value.TotalHours:F1}h";
                if (Duration.Value.TotalMinutes >= 1)
                    return $"{Duration.Value.TotalMinutes:F0}m";
                return $"{Duration.Value.TotalSeconds:F0}s";
            }
        }

        // Short commit SHA
        public string ShortCommitSha => CommitSha?.Substring(0, 8);

        // Helper methods for pipeline management
        public bool CanRetry => IsFailed;
        public bool CanCancel => IsRunning || IsPending;
        public bool HasArtifacts => Jobs?.Any(j => j.HasArtifacts) ?? false;
        public bool HasTestReport => !string.IsNullOrEmpty(TestReport);

        public override string ToString()
        {
            return $"Pipeline #{Id} - {Status} ({Branch})";
        }
    }

    public class PipelineStage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public TimeSpan? Duration => FinishedAt - StartedAt;
        public List<PipelineJob> Jobs { get; set; } = new List<PipelineJob>();
    }

    public class PipelineJob
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public TimeSpan? Duration => FinishedAt - StartedAt;
        public List<PipelineArtifact> Artifacts { get; set; } = new List<PipelineArtifact>();
        public bool HasArtifacts => Artifacts?.Any() ?? false;
        public string LogPath { get; set; }
        public Dictionary<string, string> Variables { get; set; } = new Dictionary<string, string>();
    }

    public class PipelineArtifact
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string DownloadUrl { get; set; }

        public string FormattedSize
        {
            get
            {
                if (Size < 1024)
                    return $"{Size}B";
                if (Size < 1024 * 1024)
                    return $"{Size / 1024.0:F1}KB";
                if (Size < 1024 * 1024 * 1024)
                    return $"{Size / (1024.0 * 1024.0):F1}MB";
                return $"{Size / (1024.0 * 1024.0 * 1024.0):F1}GB";
            }
        }
    }
}
