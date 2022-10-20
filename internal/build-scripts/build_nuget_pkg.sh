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
# This scripts add folders/filesrelated to fdk-dotnet(that will be needed during test image build)
#

set -xe

(
  dotnet build -c Release  -p:PackageVersion=$BUILD_VERSION
  dotnet pack -o /fdk-nuget-pkg -c Release -p:packageVersion=$BUILD_VERSION
  mkdir -p ./internal/out
  cp -r /fdk-nuget-pkg/* ./internal/out
  chmod 777 ./internal/out/*
)
