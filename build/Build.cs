using Nuke.Common;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Serilog;
using System;
using System.Linq;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;


    [Solution(GenerateProjects = true)]
    readonly Solution Solution;

    [Parameter]
    readonly bool IgnoreFailedSources;

    [GitVersion(NoFetch = true)]
    readonly GitVersion GitVersion;

    static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
    static AbsolutePath MainSourceDirectory => RootDirectory / "src" / "BusinessValidation";
    static AbsolutePath MainLibraryDirectory => MainSourceDirectory / "BusinessValidation";
    static AbsolutePath TestsDirectory => MainSourceDirectory / "BusinessValidation.Tests";

    Target Print => _ => _
    .Executes(() =>
    {
        Log.Information("GitVersion = {Value}", GitVersion.MajorMinorPatch);
        Log.Information("NuGetVersion = {Value}", GitVersion.NuGetVersion);
    });

    Target Clean => _ => _
        .Description("Cleaning Project.")
        .Executes(() =>
        {
            MainLibraryDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
    .Description("Restoring Project Dependencies.")
    .DependsOn(Clean)
    .Executes(() =>
        {
            DotNetRestore(_ => _
            .SetProjectFile(Solution)
            .SetIgnoreFailedSources(IgnoreFailedSources)
            );
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var publishConfiguration =
                from project in new[] { Solution.BusinessValidation }
                from framework in project.GetTargetFrameworks()
                select (project, framework);
            
            DotNetPublish(_ =>
                _.SetProject(Solution)
                .SetConfiguration(Configuration)
                .SetCopyright($"David Rogers 2022 - {DateTime.Now.Year}")
                .SetAssemblyVersion(GitVersion.AssemblySemFileVer)
                .SetFileVersion(GitVersion.MajorMinorPatch)
                .SetVersion(GitVersion.NuGetVersion)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .CombineWith(publishConfiguration, (_, v) =>
                    _.SetProject(v.project)
                    .SetFramework(v.framework))
                );
        });

    Target Pack => _ => _
      .DependsOn(Compile)
      .Executes(() => DotNetPack(s => s
            .SetProject(Solution.BusinessValidation)
            .SetConfiguration(Configuration)
            .EnableNoBuild()
            .EnableNoRestore()
            .SetNoDependencies(true)
            .SetPackageId("BusinessValidation")
            .SetTitle("BusinessValidation")      
            .SetVersion(GitVersion.NuGetVersion)
            .SetRepositoryType(AzurePipelinesRepositoryType.Git.ToString().ToLowerInvariant())
            .SetRepositoryUrl("https://github.com/DavidRogersDev/BusinessValidation.git")
            .SetPackageReleaseNotes("Awesome")
            .SetAuthors("David Rogers")
            .SetPackageIconUrl("icon.png")
            .SetProperty("PackageLicenseExpression", "MIT")
            .SetPackageTags("BusinessValidation Validation Validator Validators")      
            .SetPackageRequireLicenseAcceptance(false)
            .SetDescription("A library to perform validation in business services and give a mechanism to report failures back to the user interface.")
            .SetProperty("PackageReadmeFile", "readme.md")
            .SetProperty("RepositoryBranch", GitVersion.BranchName)
            .SetProperty("RepositoryCommit", GitVersion.Sha)
            .SetProperty("Copyright", $"David Rogers 2022 - {DateTime.Now.Year}")
            .SetProperty("PackageProjectUrl", "https://github.com/DavidRogersDev/BusinessValidation")
            .SetProperty("PackageIcon", "icon.png")
            .SetOutputDirectory(ArtifactsDirectory)
        ));
}
