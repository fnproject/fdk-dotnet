#!/usr/bin/env bash
set -ex
echo "dotnet Version"
dotnet --version

(
  #run unit test cases
  dotnet test
)