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
                                     Version                 = "1.0.0.0",
                                     Title                   = "BusinessValidation",
                                     Authors                 = new[] {"David Rogers"},
                                     Description             = "A library to perform validation in business services and give a mechanism to report failures back to the user interface.",
                                     ProjectUrl              = new Uri("https://github.com/DavidRogersDev/BusinessValidation"),                                     
                                     Copyright               = "David Rogers 2022",
                                     ReleaseNotes            = new [] {"Release to market."},
                                     Tags                    = new [] {"BusinessValidation", "Validation", "Validator", "Validators"},
                                     RequireLicenseAcceptance= false,
                                     Symbols                 = false,
                                     NoPackageAnalysis       = true,
                                     Files                   = new [] {
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netstandard2.1/BusinessValidation.dll", Target = "lib/netstandard2.1"},
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netstandard2.1/BusinessValidation.pdb", Target = "lib/netstandard2.1"},
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netcoreapp3.0/BusinessValidation.dll", Target = "lib/netcoreapp3.0"},												
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netcoreapp3.0/BusinessValidation.pdb", Target = "lib/netcoreapp3.0"},
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netcoreapp3.1/BusinessValidation.dll", Target = "lib/netcoreapp3.1"},												
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/netcoreapp3.1/BusinessValidation.pdb", Target = "lib/netcoreapp3.1"},											
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/net5.0/BusinessValidation.dll", Target = "lib/net5.0"},												
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/net5.0/BusinessValidation.pdb", Target = "lib/net5.0"},
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/net6.0/BusinessValidation.dll", Target = "lib/net6.0"},												
												new NuSpecContent {Source = "./BusinessValidation/bin/Release/net6.0/BusinessValidation.pdb", Target = "lib/net6.0"},
												new NuSpecContent {Source = "./icon.png"},
                                                                       },                  
                                     OutputDirectory         = "./NuGet"
                                 };	
	NuGetPack("BusinessValidation.nuspec", nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);