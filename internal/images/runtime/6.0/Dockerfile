FROM oraclelinux:8-slim

RUN microdnf install aspnetcore-runtime-6.0 && \
      microdnf clean all && \
      groupadd --system --gid 1000 fn && \
      adduser --system --uid 1000 --gid fn fn

CMD ["dotnet", "--version"]