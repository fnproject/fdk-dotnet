#!/usr/bin/env bash

#
# Copyright (c) 2021, 2022 Oracle and/or its affiliates. All rights reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#

#
# This script orchestrator.sh the all steps needed to build and release the fdk-go images
#
set -xe

BUILD_VERSION=${BUILD_VERSION:-1.0.0-SNAPSHOT}
LOCAL=${LOCAL:-true}

export BUILD_VERSION
export LOCAL

#Build the nuget-package out of fdk-dotnet
(
  docker build -t fdk_dotnet_build_image -f ./internal/docker-files/Dockerfile_build .
  docker run --rm -v $PWD:/build -w /build --env BUILD_VERSION=${BUILD_VERSION} fdk_dotnet_build_image ./internal/build-scripts/build_nuget_pkg.sh
)

#Run unit tests
(
  docker build -t fdk_dotnet_test_build_image -f ./internal/docker-files/Dockerfile_unit_test .
  docker run --rm -v $PWD:/build -w /build fdk_dotnet_test_build_image ./internal/build-scripts/execute_unit_tests.sh
)

# Build base fdk build and runtime docker images
(
  source internal/build-scripts/build_base_images.sh $BUILD_VERSION
)

#Build the integration test docker images
(
  source internal/build-scripts/build_test_images.sh
)

# Cleanup
# (
#  source internal/build-scripts/cleanup_nuget_pkg.sh
# )
