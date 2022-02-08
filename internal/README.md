# Integration test package for dotnet FDK
The internal integration test package for dotnet FDK helps to build
test images of functions and pushes them to OCIR.

## Overview of the integration test package folders

-   internal/build-scripts - Contains build scripts to build the following:
    -   Base images for fdk build and runtime
    -   Build and push test function images to OCIR.
-   internal/images - Contains dockerfiles to support the building of base fdk build and runtime images.
-   internal/tests-images - Contains source code of test functions for different dotnet runtime versions.
-   internal/build/orchestrator.sh - This script helps to run fdk-dotnet unit tests and related build pipeline.
-   internal/release - Contains scripts to support release of fdk dotnet tags and docker artifacts to remote repositories.

## Steps to generate the test function images and push them to OCIR

-   Setup below environment variables
    ```sh
    export BUILD_VERSION=1.0.0-SNAPSHOT
    export OCIR_PASSWORD=''
    export OCIR_USERNAME=bmc_operator_access/<guid>
    export OCIR_REGION=<airport_code>.ocir.io
    export OCIR_LOC=<tenancy_name>/<repo>
    
    Example -
    export BUILD_VERSION=1.0.0-SNAPSHOT
    export OCIR_PASSWORD=''
    export OCIR_USERNAME=bmc_operator_access/<guid>
    export OCIR_REGION="iad.ocir.io"
    export OCIR_LOC="oraclefunctionsdevelopm/fdk-test-functions"
    ```
-   Run the script to build all the artifacts and test images.
    ```sh
    ./internal/build/orchestrator.sh
    ```
## Cache dotnet docker images in artifactory
-   Since artifactory functions as a caching proxy for DockerHub, any image pulled from dockerhub will be cached in artifactory.
    The cached images will be removed as part of cleanup if not downloaded again within a particular time frame.
    Hence, one may encounter rate limiting issue while accessing the dotnet docker images.
    -   To resolve the rate limiting issue, execute below script locally
        ```sh
        ./internal/cache_dotnet_images.sh
        ```
        OR
    -   Edit Build Pull Request configuration in FAAS-FDK/fdk-dotnet teamcity project and enable build step - Cache dotnet docker hub images in artifactory
    
    