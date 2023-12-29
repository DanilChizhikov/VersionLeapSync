# VersionLeapSync
![](https://img.shields.io/badge/unity-2022.3+-000.svg)

## Description
VersionLeapSync is designed to solve migration tasks from one version to another.

## Table of Contents
- [Getting Started](#Getting-Started)
    - [Install manually (using .unitypackage)](#Install-manually-(using-.unitypackage))
    - [Install via UPM (using Git URL)](#Install-via-UPM-(using-Git-URL))
- [Project Structure](#Project-Structure)
- [Runtime](#Runtime)
- [License](#License)

## Getting Started
Prerequisites:
- [GIT](https://git-scm.com/downloads)
- [Unity](https://unity.com/releases/editor/archive) 2022.3+

### Install manually (using .unitypackage)
1. Download the .unitypackage from [releases](https://github.com/DanilChizhikov/VersionLeapSync/releases/) page.
2. Open VersionLeapSync.x.x.x.unitypackage

### Install via UPM (using Git URL)
1. Navigate to your project's Packages folder and open the manifest.json file.
2. Add this line below the "dependencies": { line
    - ```json title="Packages/manifest.json"
      "com.danilchizhikov.vls": "https://github.com/DanilChizhikov/VersionLeapSync.git",
      ```
UPM should now install the package.

## Project Structure
- IVersionService
```csharp
public interface IVersionService
{
    IVersionChangeInfo VersionChangeInfo { get; }
    int CurrentVersionNumber { get; }
    int LastVersionNumber { get; }
    VersionComparisonResult VersionComparisonResult { get; }
    
    void Initialize();
    bool TryCompare(VersionComparer comparer);
}
```

- IVersionConfig
```csharp
public interface IVersionConfig
{
    string ZeroVersion { get; }
}
```

- IVersionChangeInfo
```csharp
public interface IVersionChangeInfo
{
    int CurrentVersionNumber { get; }
    int LastVersionNumber { get; }
    VersionComparisonResult ComparisonResult { get; }
}
```

- IVersionChangeHandler
```csharp
public interface IVersionChangeHandler
{
    VersionComparer VersionComparer { get; }

    void Execute(IVersionChangeInfo info);
}
```

- VersionComparer
```csharp
public enum VersionComparer : byte
{
    None = 0,
    NotEqual = 1,
    Greater = 2,
    Less = 3,
    Always = 4,
}
```

- VersionComparisonResult
```csharp
public enum VersionComparisonResult : byte
{
    Same = 0,
    Upgrade = 1,
    Downgrade = 2,
}
```

- IVersionSaveAdapter
```csharp
public interface IVersionSaveAdapter
{
    string Load(string defaultValue);
    void Save(string value);
}
```

## Runtime
- VersionService
```csharp
public sealed class VersionService : IVersionService
{
    public IVersionChangeInfo VersionChangeInfo { get; }
    public int CurrentVersionNumber { get; }
    public int LastVersionNumber { get; }
    public VersionComparisonResult VersionComparisonResult { get; }
    
    public VersionService(IVersionConfig config, IVersionSaveAdapter saveAdapter,
                              IEnumerable<IVersionChangeHandler> changeHandlers);
    
    public void Initialize();
    public bool TryCompare(VersionComparer comparer);
}
```

## License
MIT