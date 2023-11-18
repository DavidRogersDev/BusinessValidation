using Invariants;
using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Octopus;
using Nuke.Common.Utilities.Collections;
using Serilog;
using System;
using System.Linq;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Push);


    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    static bool IsPublishableBranch = false;


    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter]
    readonly bool IgnoreFailedSources;

    [Parameter]
    readonly string ReleaseNotes;

    [GitVersion(NoFetch = false)]
    readonly GitVersion GitVersion;

    [Parameter]
    readonly string PackagesNugetApiUrl;

    [Parameter]
    [Secret]
    readonly string PackagesNugetApiKey;

    [Parameter]
    readonly string NugetOrgNugetApiUrl;

    [Parameter]
    [Secret]
    readonly string NugetOrgNugetApiKey;

    static AbsolutePath ArtifactsDirectory => RootDirectory / FileSystem.ArtifactsDirectory;

    Target Print => _ => _
    .Description(Descriptions.Print)
    .Executes(() =>
    {
        Log.Information(LogMessage.ReleaseNotes, ReleaseNotes ?? ProjectValues.NoValue);
        Log.Information(LogMessage.RootDirectory, RootDirectory.Name);
        Log.Information(LogMessage.MajorMinorPatch, GitVersion.MajorMinorPatch);
        Log.Information(LogMessage.NugetVersion, GitVersion.NuGetVersion);
        Log.Information(LogMessage.PreReleaseLabel, GitVersion?.PreReleaseLabel ?? ProjectValues.NoValue);
        Log.Information(LogMessage.Configuration, Configuration?.ToString() ?? ProjectValues.NoValue);

        IsPublishableBranch = GitVersion.BranchName.StartsWith(Branch.Release, StringComparison.OrdinalIgnoreCase);
    });

    Target Clean => _ => _
        .Description(Descriptions.Clean)
        .DependsOn(Print)
        .Executes(() =>
        {
            Log.Information(LogMessage.CleaningBinaryDirectories);

            Solution.BusinessValidation.Directory.GlobDirectories(FileSystem.GlobPattern.BinPattern, FileSystem.GlobPattern.ObjPattern).DeleteDirectories();
            Solution.BusinessValidation_Tests.Directory.GlobDirectories(FileSystem.GlobPattern.BinPattern, FileSystem.GlobPattern.ObjPattern).DeleteDirectories();

            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
    .Description(Descriptions.Restore)
    .DependsOn(Clean)
    .Executes(() =>
        {
            //Log.Information("IgnoreFailedSources: {IgnoreFailedSources}", IgnoreFailedSources);
            DotNetRestore(_ => _
            .SetProjectFile(Solution)
            .SetIgnoreFailedSources(IgnoreFailedSources)
            );
        });

    Target Compile => _ => _
        .Description(Descriptions.Compile)
        .DependsOn(Restore)
        .Executes(() =>
        {
            var publishConfiguration =
                from project in new[] { Solution.BusinessValidation, Solution.BusinessValidation_Tests }
                from framework in project.GetTargetFrameworks()
                select (project, framework);

            DotNetPublish(_ =>
                _.SetProject(Solution)
                .EnableNoRestore()
                .SetConfiguration(Configuration)
                .SetCopyright(PackageValues.Copyright)
                .SetAssemblyVersion(GitVersion.AssemblySemFileVer)
                .SetFileVersion(GitVersion.MajorMinorPatch)
                .SetVersion(GitVersion.NuGetVersion)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .AddNoWarns(NoWarn.Codes)
                .CombineWith(publishConfiguration, (_, v) =>
                    _.SetProject(v.project)
                    .SetFramework(v.framework))
                );
        });

    Target Test => _ => _
    .Description(Descriptions.Test)
    .DependsOn(Compile)
    .Executes(() =>
    {
        DotNetTest(_ => _
        .EnableNoRestore()
        .EnableNoBuild()
        .SetProjectFile(Solution.BusinessValidation_Tests)
        .SetConfiguration(Configuration)
        );
    });

    Target Pack => _ => _
    .Description(Descriptions.Pack)
    .DependsOn(Test)
    .Executes(() => DotNetPack(s => s
            .SetProject(Solution.BusinessValidation)
            .SetConfiguration(Configuration)
            .EnableNoBuild()
            .EnableNoRestore()
            .SetNoDependencies(true)
            .SetPackageId(ProjectValues.BusinessValidationProject)
            .SetTitle(ProjectValues.BusinessValidationProject)
            .SetVersion(GitVersion.NuGetVersion)
            .SetRepositoryType(AzurePipelinesRepositoryType.Git.ToString().ToLowerInvariant())
            .SetPackageReleaseNotes(ReleaseNotes)
            .SetPackageProjectUrl(PackageValues.ProjectUrl)
            .SetAuthors(PackageValues.Author)
            .SetProperty(PackageProperties.PackageLicenseExpression, PackageValues.MITLicence)
            .SetPackageTags(PackageValues.Tags)
            .SetPackageRequireLicenseAcceptance(false)
            .SetDescription(PackageValues.Description)
            .SetRepositoryUrl(PackageValues.RepositoryUrl)
            .SetProperty(PackageProperties.RepositoryBranch, GitVersion.BranchName)
            .SetProperty(PackageProperties.RepositoryCommit, GitVersion.Sha)
            .SetProperty(PackageProperties.Copyright, PackageValues.Copyright)
            .SetProperty(PackageProperties.PackageReadmeFile, PackageValues.Readme)
            .SetProperty(PackageProperties.PackageIcon, PackageValues.Icon)
            .SetOutputDirectory(ArtifactsDirectory)
        ));

    Target Push => _ => _
        .Description(Descriptions.Push)
        .OnlyWhenStatic(() => IsServerBuild) // checked before the build steps run.
        .OnlyWhenDynamic(() => IsPublishableBranch) // checked during run. This variable is set in the Print task.
        .Requires(() => NugetOrgNugetApiKey)
        .Requires(() => NugetOrgNugetApiUrl)
        .Requires(() => PackagesNugetApiKey)
        .Requires(() => PackagesNugetApiUrl)
        .Requires(() => Configuration.Equals(Configuration.Release))
        .DependsOn(Pack)
        .Executes(() =>
        {
            var nugetFiles = ArtifactsDirectory.GlobFiles(FileSystem.Nuget.Packages);

            Assert.NotEmpty(nugetFiles, LogMessage.NoNugetFiles);

            // if it is not a pre-release, publish to Nuget.org
            if (string.IsNullOrWhiteSpace(GitVersion.PreReleaseLabel))
            {
                // this publishes to the nuget.org package manager
                nugetFiles.Where(filePath => !filePath.Name.EndsWith(FileSystem.Nuget.SymbolsPackages, StringComparison.OrdinalIgnoreCase))
                    .ForEach(x =>
                    {
                        DotNetNuGetPush(s => s
                            .SetTargetPath(x)
                            .SetSource(NugetOrgNugetApiUrl)
                            .SetApiKey(NugetOrgNugetApiKey)
                        );
                    });
            }
            else if (GitVersion.PreReleaseLabel.Equals(BuildType.Ci, StringComparison.OrdinalIgnoreCase) ||
                GitVersion.PreReleaseLabel.Equals(BuildType.Rc, StringComparison.OrdinalIgnoreCase))
            {
                // this publishes to the github packages package manager
                nugetFiles.Where(filePath => !filePath.Name.EndsWith(FileSystem.Nuget.SymbolsPackages))
                    .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(PackagesNugetApiUrl)
                        .SetApiKey(PackagesNugetApiKey)
                    );
                });
            }
        });
}
