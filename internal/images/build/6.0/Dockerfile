FROM --platform=$TARGETPLATFORM ocr-docker-remote.artifactory.oci.oraclecorp.com/os/oraclelinux:8-slim

RUN microdnf install dotnet-sdk-6.0 && \
      microdnf clean all

CMD ["dotnet", "--version"]
