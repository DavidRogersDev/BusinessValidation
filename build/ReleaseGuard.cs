using Invariants;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace _build
{
    public sealed class ReleaseGuard
    {
        readonly GitVersion _gitVersion;
        readonly PackagePublishConfig _nugetOrg;
        readonly PackagePublishConfig _githubPackages;

        internal ReleaseGuard(GitVersion GitVersion, PackagePublishConfig nugetOrg, PackagePublishConfig githubPackages)
        {
            _githubPackages = githubPackages;
            _nugetOrg = nugetOrg;
            _gitVersion = GitVersion ?? throw new ArgumentNullException(nameof(GitVersion));
        }

        private bool IsPreReleaseBuild =>
            _gitVersion.PreReleaseLabel.Equals(BuildType.Ci, StringComparison.OrdinalIgnoreCase) ||
            _gitVersion.PreReleaseLabel.Equals(BuildType.Rc, StringComparison.OrdinalIgnoreCase);

        private bool IsReleaseOrMainBranch =>
            _gitVersion.BranchName.StartsWith(Branch.Release, StringComparison.OrdinalIgnoreCase) ||
            _gitVersion.BranchName.Equals(Branch.Main, StringComparison.OrdinalIgnoreCase);

        public bool IsTaggedBuild => _gitVersion.PreReleaseTag.IsEmpty();

        public bool BuildToBePacked(OverrideMode? overrideMode = null) => overrideMode switch
        {            
            OverrideMode.Allow => true,
            OverrideMode.Deny => false,
            _ => IsReleaseOrMainBranch || IsTaggedBuild
        };

        public bool BuildToBeReleasedToNugetOrg() => IsTaggedBuild;

        public bool BuildToBeReleasedToGitHub() => IsPreReleaseBuild;

        public PackagePublishConfig ResolvePublishDestinationDetails()
        {
            if (BuildToBeReleasedToNugetOrg())
            {
                return _nugetOrg;
            }
            else if (BuildToBeReleasedToGitHub())
            {
                return _githubPackages;
            }
            else
            {
                return new PackagePublishConfig(string.Empty, string.Empty);
            }
        }
    }

    public record PackagePublishConfig(string Token, string Url)
    {
        public bool HasNoValue => string.IsNullOrWhiteSpace(Token) || string.IsNullOrWhiteSpace(Url);
    }
}