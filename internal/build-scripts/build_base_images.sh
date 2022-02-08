# Build base fdk build and runtime images

fdkVersion=$1

./internal/build-scripts/build_base_image.sh 3.1 $fdkVersion
