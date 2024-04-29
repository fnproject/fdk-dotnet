FROM --platform=$TARGETPLATFORM container-registry.oracle.com/os/oraclelinux:8-slim

RUN set -eux; microdnf -y update;
RUN microdnf install aspnetcore-runtime-8.0 && \
      microdnf clean all && \
      groupadd --system --gid 1000 fn && \
      adduser --system --uid 1000 --gid fn fn

CMD ["dotnet", "--version"]
