#!/usr/bin/env bash

set -eu

REGCTL_BIN=/Users/jkumavat/regctl
# Test regctl is on path
$REGCTL_BIN --help

BUILD_TAG=$(git describe --tags)
echo "${BUILD_TAG}"

TEMPDIR=$(mktemp -d)
cd "${TEMPDIR}"

function cleanup {
    rm -rf "${TEMPDIR}"
}
trap cleanup EXIT


{
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:8.0-$BUILD_TAG docker.io/fnproject/dotnet:8.0-$BUILD_TAG;
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:8.0-$BUILD_TAG-dev docker.io/fnproject/dotnet:8.0-$BUILD_TAG-dev;
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:6.0-$BUILD_TAG docker.io/fnproject/dotnet:6.0-$BUILD_TAG;
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:6.0-$BUILD_TAG-dev docker.io/fnproject/dotnet:6.0-$BUILD_TAG-dev;
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:3.1-$BUILD_TAG docker.io/fnproject/dotnet:3.1-$BUILD_TAG;
$REGCTL_BIN image copy iad.ocir.io/oraclefunctionsdevelopm/fnproject/dotnet:3.1-$BUILD_TAG-dev docker.io/fnproject/dotnet:3.1-$BUILD_TAG-dev;
}

