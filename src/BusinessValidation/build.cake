#tool "nuget:?package=NuGet.CommandLine&version=6.2.1"

var target = Argument("target", "NugetPackIt");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./BusinessValidation/bin/{configuration}");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./BusinessValidation.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("./BusinessValidation.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

Task("NugetPackIt")
    .IsDependentOn("Test")
    .Does(() =>
{
     var nuGetPackSettings   = new NuGetPackSettings {
                                     Id                      = "BusinessValidation",
                                     Version                 = "1.0.1.0",
                                     Title                   = "BusinessValidation",
                                     Authors                 = new[] {"David Rogers"},
                                     Description             = "A library to perform validation in business services and give a mechanism to report failures back to the user interface.",
                                     ProjectUrl              = new Uri("https://github.com/DavidRogersDev/BusinessValidation"),                                     
                                     Copyright               = "David Rogers 2022",
                                     ReleaseNotes            = new [] {"Issue #1 fix. Minor bug. Debugger display will now provide a text representation of the Validator in debugging mode."},
                                     Tags                    = new [] {"BusinessValidation", "Validation", "Validator", "Validators"},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     Files                   = new [] {
												new NuSpecContent {Source = "BusinessValidation.dll", Target = "lib/netstandard2.1"},
												new NuSpecContent {Source = "BusinessValidation.pdb", Target = "lib/netstandard2.1"},
												new NuSpecContent {Source = "./../../../../icon.png"},
                                                                       },
                                     BasePath                = "./BusinessValidation/bin/Release/netstandard2.1",
                                     OutputDirectory         = "./NuGet"
                                 };	
	NuGetPack("BusinessValidation.nuspec", nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);