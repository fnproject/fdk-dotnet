FROM docker-remote.artifactory.oci.oraclecorp.com/oraclelinux:8-slim

RUN microdnf install dotnet-sdk-6.0 && \
      microdnf clean all

CMD ["dotnet", "--version"]
