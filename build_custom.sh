#!/usr/bin/env bash

artifacts_directory="$GITHUB_WORKSPACE/artifacts"
nuget_directory="$artifacts_directory/nuget"
source_directory="$GITHUB_WORKSPACE/src/BusinessValidation"
publish_directory_NET5="$source_directory/BusinessValidation/bin/Release/net5.0/publish"
soln_path="$source_directory/BusinessValidation.sln"
proj_path="$source_directory/BusinessValidation/BusinessValidation.csproj"
test_proj_path="$source_directory/BusinessValidation.Tests/BusinessValidation.Tests.csproj"
echo "*******************************************************************************************************************************************************************************"
echo "Print"
echo "*******************************************************************************************************************************************************************************"
echo "Root Directory: $GITHUB_WORKSPACE"
echo "Major Minor Patch = $MajorMinorPatch"
echo "PreReleaseLabel = $PreReleaseLabel"
echo $nuget_directory
echo $publish_directory_NET5
echo $soln_path
echo $proj_path
echo $test_proj_path
echo "*******************************************************************************************************************************************************************************"
echo "Clean"
echo "*******************************************************************************************************************************************************************************"
# need to test if these exist and delete them if they do.
mkdir $artifacts_directory
mkdir $nuget_directory
echo "*******************************************************************************************************************************************************************************"
echo "Restore"
echo "*******************************************************************************************************************************************************************************"
dotnet restore $soln_path
echo "*******************************************************************************************************************************************************************************"
echo "Compile"
echo "*******************************************************************************************************************************************************************************"
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net5.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net6.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net7.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net8.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net9.0
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net10.0
dotnet publish $test_proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$1 --property:FileVersion=$2 --property:Version=$3 --property:InformationalVersion=$4 --framework net9.0
echo "*******************************************************************************************************************************************************************************"
echo "Test"
echo "*******************************************************************************************************************************************************************************"
dotnet test $test_proj_path --no-restore --no-build --configuration Release