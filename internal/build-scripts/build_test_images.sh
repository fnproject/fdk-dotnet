#!/usr/bin/env bash

set -ex

# Login to OCIR
echo ${OCIR_PASSWORD} | docker login --username "${OCIR_USERNAME}" --password-stdin ${OCIR_REGION}

# Build and push the test function images to OCIR for integration test framework.

# dotnet 3.1
(
  source internal/build-scripts/build_test_image.sh internal/test-images/hello-world-fn hello-world-fn 3.1
  source internal/build-scripts/build_test_image.sh internal/test-images/timeout-fn timeout-fn 3.1
  source internal/build-scripts/build_test_image.sh internal/test-images/runtime-version-fn runtime-version-fn 3.1
  source internal/build-scripts/build_test_image.sh internal/test-images/oci-sdk-fn oci-sdk-fn 3.1
)

# dotnet 6
(
  source internal/build-scripts/build_test_image.sh internal/test-images/hello-world-fn hello-world-fn 6.0
  source internal/build-scripts/build_test_image.sh internal/test-images/timeout-fn timeout-fn 6.0
  source internal/build-scripts/build_test_image.sh internal/test-images/oci-sdk-fn oci-sdk-fn 6.0
  source internal/build-scripts/build_test_image.sh internal/test-images/runtime-version-fn runtime-version-fn 6.0
)