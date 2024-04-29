#!/usr/bin/env bash
#
# Copyright (c) 2019, 2020 Oracle and/or its affiliates. All rights reserved.
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


set -xeuo pipefail

if [[ -z ${1:-} ]];then
  echo "Please supply dotnet version as argument to build image." >> /dev/stderr
  exit 2
fi

dotnetversion=$1
fdk_version=$2


echo $dotnetversion

if [ $dotnetversion == "3.1" ]; then
  pushd internal/images/build/${dotnetversion} && docker buildx build --push --platform linux/amd64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version}-dev . && popd
  pushd internal/images/runtime/${dotnetversion} && docker buildx build --push --platform linux/amd64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version} . && popd
elif [ $dotnetversion == "8.0" ]; then
  pushd internal/images/build/${dotnetversion} && docker buildx build --push --platform linux/amd64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version}-dev . && popd
  pushd internal/images/runtime/${dotnetversion} && docker buildx build --push --platform linux/amd64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version} . && popd
else
  pushd internal/images/build/${dotnetversion} &&
  docker buildx build --push --platform linux/amd64,linux/arm64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version}-dev .
  popd
  pushd internal/images/runtime/${dotnetversion}
  docker buildx build --push --platform linux/amd64,linux/arm64 -t ${OCIR_REGION}/${OCIR_LOC}/dotnet:${dotnetversion}-${fdk_version} .
  popd
fi
