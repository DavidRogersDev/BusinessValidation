#!/usr/bin/env bash

# set variables
artifacts_directory="$GITHUB_WORKSPACE/artifacts"
nuget_directory="$artifacts_directory/nuget"
source_directory="$GITHUB_WORKSPACE/src/BusinessValidation"
publish_directory_NET5="$source_directory/BusinessValidation/bin/Release/net5.0/publish"
soln_path="$source_directory/BusinessValidation.sln"
proj_path="$source_directory/BusinessValidation/BusinessValidation.csproj"
test_proj_path="$source_directory/BusinessValidation.Tests/BusinessValidation.Tests.csproj"
isTaggedBuild=false
push_package_to_github=
push_package_to_nuget=

isTaggedBuild=false

if [[ -z "$preReleaseLabel" ]]; then
    isTaggedBuild=true
else
    isTaggedBuild=false
fi

isPreReleaseBuild=false

if [[ $preReleaseLabel == "ci" || $preReleaseLabel == "rc" ]]; then
    isPreReleaseBuild=true
else
    isPreReleaseBuild=false
fi

isReleaseOrMainBranch=false

if [[ $branchName == "release*" || $branchName == "main" ]]; then
    isReleaseOrMainBranch=true
else
    isReleaseOrMainBranch=false
fi

# run through the build steps

echo "***********************************"
echo "Print"
echo "***********************************"
echo "Root Directory: $GITHUB_WORKSPACE"
echo "AssemblySemVer: $assemblySemVer"
echo "Major Minor Patch: $majorMinorPatch"
echo "PreReleaseLabel: $preReleaseLabel"
echo "InformationalVersion: $informationalVersion"
echo "SemVer: $semVer"

echo "***********************************"
echo "Clean"
echo "***********************************"
# need to test if directories exist and delete them if they do.
if [ -d "$artifacts_directory" ]; then
  echo "$artifacts_directory exists. Cleaning it out"
  rm -rf $artifacts_directory/*
else
  mkdir $artifacts_directory
  mkdir $nuget_directory
fi

echo "***********************************"
echo "Restore"
echo "***********************************"
dotnet restore $soln_path

echo "***********************************"
echo "Compile"
echo "***********************************"
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net5.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net6.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net7.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net8.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net9.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net10.0
dotnet publish $test_proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net9.0

echo "***********************************"
echo "Test"
echo "***********************************"
dotnet test $test_proj_path --no-restore --no-build --configuration Release

# capture dotnet test exit code
exitcode=${PIPESTATUS[0]}
if [ $exitcode == 0 ]; then
	echo "All tests passed"
	exit 0
else	
	echo "Test failure"
	exit 1
fi

echo "***********************************"
echo "Pack"
echo "***********************************"

dotnet pack $proj_path --configuration Release --no-build --no-restore --no-dependencies --property:PackageId=BusinessValidation --property:Title=BusinessValidation --property:Version=$3 --property:RepositoryType=git --property:PackageProjectUrl=https://github.com/DavidRogersDev/BusinessValidation --property:Authors="David Rogers" --property:PackageLicenseExpression=MIT --property:PackageTags="BusinessValidation Business-Validation Business-Validators Business-Validator Validation Validator Validators" --property:PackageRequireLicenseAcceptance=false --property:Description="A library to perform validation in business services and give a mechanism to report failures back to the user interface." --property:RepositoryUrl=https://github.com/DavidRogersDev/BusinessValidation.git --property:RepositoryBranch=main --property:RepositoryCommit=778ffa79441eef5267ee3e8ae5b5874f315f68a7 --property:Copyright="David Rogers 2022 - 2026" --property:PackageReadmeFile=readme.md --property:PackageIcon=icon.png --output $nuget_directory
nugetfilename=$(ls $nuget_directory/*.nupkg) 
echo $nugetfilename

echo "***********************************"
echo "Push Nuget Packages"
echo "***********************************"
dotnet nuget push $nugetfilename --source https://nuget.pkg.github.com/DavidRogersDev/index.json --api-key $PackagesNugetApiKey