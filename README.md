# GitLab Plugin for Git Extensions

A plugin that integrates GitLab functionality directly into Git Extensions.

## Features

- Pipeline management and monitoring
- Merge request creation and management
- Branch protection integration
- Review management
- Automated notifications
- Custom templates support

## Requirements

- Git Extensions 4.0+
- .NET 6.0 Runtime
- GitLab account with API access

## Building and Installation

### Prerequisites

- Visual Studio 2022 or later
- .NET 6.0 SDK
- Git Extensions 4.0 or later installed

### Building from Source

1. Clone the repository:

```bash
git clone https://github.com/yourusername/GitExtensions.GitLab.git
cd GitExtensions.GitLab
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Build the solution:

```bash
dotnet build -c Release
```

### Installation

#### Automatic Installation

1. Download the latest release from the releases page
2. Double-click the `.gex` file
3. Follow the installation wizard

#### Manual Installation

1. Locate your Git Extensions installation directory (typically `C:\Program Files\GitExtensions`)
2. Create plugin directory:

```bash
mkdir "C:\Program Files\GitExtensions\Plugins\GitLab"
```

3. Copy the following files from `bin\Release\net6.0-windows`:

- GitExtensions.GitLab.dll
- GitLabApiClient.dll

### Configuration

1. Start Git Extensions
2. Navigate to Tools > Options > Plugins
3. Find "GitLab Integration"
4. Configure:
   - GitLab URL
   - API Token
   - Default Project ID

### Development Setup

#### Running Tests

```bash
dotnet test
```

#### Building with Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

#### Debugging

1. Set up Visual Studio:
   - Right-click project > Properties
   - Debug tab > Start external program: select GitExtensions.exe
   - Add command line arguments: `/debug`

2. Press F5 to start debugging

## Troubleshooting

### Plugin Not Loading

- Verify DLL placement in plugins directory
- Check Git Extensions version compatibility
- Review Git Extensions logs

### Connection Issues

- Validate GitLab URL
- Verify API token permissions
- Check project ID

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

MIT License - see [LICENSE](LICENSE) for details.
