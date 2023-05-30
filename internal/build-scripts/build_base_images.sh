# Build base fdk build and runtime images

fdkVersion=$1

#Login to OCIR
echo ${OCIR_PASSWORD} | docker login --username "${OCIR_USERNAME}" --password-stdin ${OCIR_REGION}

./internal/build-scripts/build_base_image.sh 3.1 $fdkVersion
./internal/build-scripts/build_base_image.sh 6.0 $fdkVersion
