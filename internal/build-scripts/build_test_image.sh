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
# This scripts builds a specific test image for given dotnet version
#
set -ex

if [ -z "$1" ]; then
  echo "Please supply function directory to build test function image" >>/dev/stderr
  exit 2
fi

if [ -z "$2" ]; then
  echo "Please supply dotnet version as argument to build image." >>/dev/stderr
  exit 2
fi

fn_dir=$1
csproj_file_name=$2
dotnet_version=$3
pkg_version=${BUILD_VERSION}
target_framework="netcoreapp3.1"

( 
    #Add the fdk related source code
    #source internal/build-scripts/build_dist_pkg.sh $fn_dir

    cp -R ./internal/out/Fnproject.Fn.Fdk.${pkg_version}.nupkg ${fn_dir}/
    sed -i".bak" "s/LATEST/$pkg_version/g" "${fn_dir}/${csproj_file_name}.csproj"
    if [ $dotnet_version == "6.0" ]
      then target_framework="net6.0"
    fi

    sed -i".bak" "s/TARGET/$target_framework/g" "${fn_dir}/${csproj_file_name}.csproj"

    pushd ${fn_dir}

    name="$(awk '/^name:/ { print $2 }' func.yaml)"
    echo "name:$name"

    version="$(awk '/^runtime:/ { print $2 }' func.yaml)"
    echo "version:$version"

    image_identifier="${version}${dotnet_version}-${pkg_version}"
    echo "image_identifier:$image_identifier"

    docker build -t fnproject/${name}:${image_identifier} \
        -f Build_file \
        --build-arg DOTNET_VERSION=${dotnet_version} \
        --build-arg OCIR_REGION=${OCIR_REGION} \
        --build-arg OCIR_LOC=${OCIR_LOC} \
        --build-arg PKG_VERSION=${pkg_version} .
    rm -rf Fnproject.Fn.Fdk.${pkg_version}.nupkg

    popd

    sed -i".bak" "s/$pkg_version/LATEST/g" "${fn_dir}/${csproj_file_name}.csproj"
    sed -i".bak" "s/$target_framework/TARGET/g" "${fn_dir}/${csproj_file_name}.csproj"
    rm -rf ${fn_dir}/${csproj_file_name}.csproj.bak

    # Push to OCIR
    ocir_image="${OCIR_LOC}/${name}:${image_identifier}"

    docker image tag "fnproject/${name}:${image_identifier}" "${OCIR_REGION}/${ocir_image}"
    docker image push "${OCIR_REGION}/${ocir_image}"
)
