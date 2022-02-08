FROM oraclelinux:8-slim

RUN microdnf install aspnetcore-runtime-3.1 && \
      microdnf clean all && \
      groupadd --system --gid 1000 fn && \
      adduser --system --uid 1000 --gid fn fn

CMD ["dotnet", "--version"]