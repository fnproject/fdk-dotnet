#!/usr/bin/env bash
set -ex
echo "dotnet Version"
dotnet --version
echo "dotnet sdks"
dotnet --list-sdks
(
  #run unit test cases
  dotnet test -l "console;verbosity=normal"
)
