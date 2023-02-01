# Build base fdk build and runtime images

fdkVersion=$1

#Login to OCIR
echo ${OCIR_PASSWORD} | docker login --username "${OCIR_USERNAME}" --password-stdin ${OCIR_REGION}

#Create the builder instance
(
   docker buildx rm builderInstance || true
   docker buildx create --name builderInstance --driver-opt=image=docker-remote.artifactory.oci.oraclecorp.com/moby/buildkit:buildx-stable-1 --platform linux/amd64,linux/arm64
   docker buildx use builderInstance
)

./internal/build-scripts/build_base_image.sh 3.1 $fdkVersion
./internal/build-scripts/build_base_image.sh 6.0 $fdkVersion
