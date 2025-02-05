using System;
using System.IO;
using System.Xml.Serialization;
using GitExtensions.Settings;
using Microsoft.Win32;

namespace GitExtensions.GitLab.Models
{
    public class PluginConfiguration
    {
        private const string RegistryPath = @"Software\GitExtensions\GitLab";
        private const string ConfigFileName = "GitLabPlugin.config";

        // GitLab Connection Settings
        public string GitLabUrl { get; set; }
        public string ApiToken { get; set; }
        public string DefaultProject { get; set; }

        // Plugin Behavior Settings
        public bool AutoPipelineCheck { get; set; }
        public bool ShowMergeRequestNotifications { get; set; }
        public bool AutoCreateMergeRequests { get; set; }
        public bool ShowPipelineStatus { get; set; }

        // UI Settings
        public bool ShowPipelineNotifications { get; set; }
        public int RefreshInterval { get; set; } = 60; // seconds
        public bool MinimizeToTray { get; set; }
        public string DefaultTargetBranch { get; set; } = "main";

        // Cache Settings
        public int CacheTimeout { get; set; } = 300; // seconds
        public bool EnableCache { get; set; } = true;
        public string CachePath { get; set; }

        // Proxy Settings
        public bool UseProxy { get; set; }
        public string ProxyServer { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        // SSL Settings
        public bool VerifySsl { get; set; } = true;
        public string SslCertPath { get; set; }

        // Merge Request Settings
        public bool RemoveSourceBranch { get; set; }
        public bool SquashCommits { get; set; }
        public string DefaultMergeRequestPrefix { get; set; } = "feature/";
        public string DefaultMergeRequestDescription { get; set; }

        // Pipeline Settings
        public int PipelineTimeout { get; set; } = 3600; // seconds
        public bool CancelConflictingPipelines { get; set; }
        public string[] ExcludedPipelineBranches { get; set; }

        public PluginConfiguration()
        {
            InitializeDefaultValues();
        }

        private void InitializeDefaultValues()
        {
            CachePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GitExtensions",
                "GitLabPlugin",
                "Cache");

            DefaultMergeRequestDescription =
                "## Changes\n" +
                "- \n\n" +
                "## Testing\n" +
                "- [ ] Unit Tests\n" +
                "- [ ] Integration Tests\n\n" +
                "## Review Checklist\n" +
                "- [ ] Code follows project guidelines\n" +
                "- [ ] Tests added/updated\n" +
                "- [ ] Documentation updated";
        }

        // Load configuration from registry or file
        public static PluginConfiguration Load()
        {
            try
            {
                // Try registry first
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryPath))
                {
                    if (key != null)
                    {
                        return LoadFromRegistry(key);
                    }
                }

                // Try config file
                var configPath = GetConfigFilePath();
                if (File.Exists(configPath))
                {
                    return LoadFromFile(configPath);
                }

                // Return default configuration
                return new PluginConfiguration();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading configuration: {ex.Message}");
                return new PluginConfiguration();
            }
        }

        // Save configuration to both registry and file
        public void Save()
        {
            try
            {
                // Save to registry
                using (var key = Registry.CurrentUser.CreateSubKey(RegistryPath))
                {
                    SaveToRegistry(key);
                }

                // Save to file
                var configPath = GetConfigFilePath();
                SaveToFile(configPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving configuration: {ex.Message}");
                throw;
            }
        }

        private static PluginConfiguration LoadFromRegistry(RegistryKey key)
        {
            var config = new PluginConfiguration
            {
                GitLabUrl = key.GetValue(nameof(GitLabUrl)) as string,
                ApiToken = key.GetValue(nameof(ApiToken)) as string,
                DefaultProject = key.GetValue(nameof(DefaultProject)) as string,
                AutoPipelineCheck = Convert.ToBoolean(key.GetValue(nameof(AutoPipelineCheck), false)),
                ShowMergeRequestNotifications = Convert.ToBoolean(key.GetValue(nameof(ShowMergeRequestNotifications), true)),
                RefreshInterval = Convert.ToInt32(key.GetValue(nameof(RefreshInterval), 60))
            };

            return config;
        }

        private void SaveToRegistry(RegistryKey key)
        {
            key.SetValue(nameof(GitLabUrl), GitLabUrl ?? string.Empty);
            key.SetValue(nameof(ApiToken), ApiToken ?? string.Empty);
            key.SetValue(nameof(DefaultProject), DefaultProject ?? string.Empty);
            key.SetValue(nameof(AutoPipelineCheck), AutoPipelineCheck);
            key.SetValue(nameof(ShowMergeRequestNotifications), ShowMergeRequestNotifications);
            key.SetValue(nameof(RefreshInterval), RefreshInterval);
        }

        private static PluginConfiguration LoadFromFile(string path)
        {
            using var stream = File.OpenRead(path);
            var serializer = new XmlSerializer(typeof(PluginConfiguration));
            return (PluginConfiguration)serializer.Deserialize(stream);
        }

        private void SaveToFile(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using var stream = File.Create(path);
            var serializer = new XmlSerializer(typeof(PluginConfiguration));
            serializer.Serialize(stream, this);
        }

        private static string GetConfigFilePath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "GitExtensions", "GitLabPlugin", ConfigFileName);
        }

        // Validate configuration
        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(GitLabUrl))
            {
                error = "GitLab URL is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ApiToken))
            {
                error = "API Token is required";
                return false;
            }

            if (string.IsNullOrWhiteSpace(DefaultProject))
            {
                error = "Default Project ID is required";
                return false;
            }

            if (RefreshInterval < 10)
            {
                error = "Refresh interval must be at least 10 seconds";
                return false;
            }

            error = null;
            return true;
        }

        // Create a copy of the configuration
        public PluginConfiguration Clone()
        {
            return new PluginConfiguration
            {
                GitLabUrl = GitLabUrl,
                ApiToken = ApiToken,
                DefaultProject = DefaultProject,
                AutoPipelineCheck = AutoPipelineCheck,
                ShowMergeRequestNotifications = ShowMergeRequestNotifications,
                AutoCreateMergeRequests = AutoCreateMergeRequests,
                ShowPipelineStatus = ShowPipelineStatus,
                ShowPipelineNotifications = ShowPipelineNotifications,
                RefreshInterval = RefreshInterval,
                MinimizeToTray = MinimizeToTray,
                DefaultTargetBranch = DefaultTargetBranch,
                CacheTimeout = CacheTimeout,
                EnableCache = EnableCache,
                CachePath = CachePath,
                UseProxy = UseProxy,
                ProxyServer = ProxyServer,
                ProxyPort = ProxyPort,
                ProxyUsername = ProxyUsername,
                ProxyPassword = ProxyPassword,
                VerifySsl = VerifySsl,
                SslCertPath = SslCertPath,
                RemoveSourceBranch = RemoveSourceBranch,
                SquashCommits = SquashCommits,
                DefaultMergeRequestPrefix = DefaultMergeRequestPrefix,
                DefaultMergeRequestDescription = DefaultMergeRequestDescription,
                PipelineTimeout = PipelineTimeout,
                CancelConflictingPipelines = CancelConflictingPipelines,
                ExcludedPipelineBranches = (string[])ExcludedPipelineBranches?.Clone()
            };
        }
    }
}
