FROM --platform=linux/amd64 ocr-docker-remote.artifactory.oci.oraclecorp.com/os/oraclelinux:8-slim

RUN microdnf install dotnet-sdk-3.1 && \
      microdnf clean all

CMD ["dotnet", "--version"]
