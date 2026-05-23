#!/usr/bin/env bash

artifacts_directory="$GITHUB_WORKSPACE/artifacts"
nuget_directory="$artifacts_directory/nuget"
source_directory="$GITHUB_WORKSPACE/src/BusinessValidation"
publish_directory_NET5="$source_directory/BusinessValidation/bin/Release/net5.0/publish"
soln_path="$source_directory/BusinessValidation.sln"
proj_path="$source_directory/BusinessValidation/BusinessValidation.csproj"
test_proj_path="$source_directory/BusinessValidation.Tests/BusinessValidation.Tests.csproj"
echo $nuget_directory
echo $publish_directory_NET5
echo $soln_path
echo $proj_path
echo $test_proj_path
dotnet restore $soln_path
dotnet publish $proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$0 --property:FileVersion=$1 --property:Version=$2 --property:InformationalVersion=$3 --framework net5.0
dotnet publish $test_proj_path --no-restore --configuration Release --property:Copyright="David Rogers 2022 - 2026" --property:AssemblyVersion=$0 --property:FileVersion=$1 --property:Version=$2 --property:InformationalVersion=$3 --framework net9.0