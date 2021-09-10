FROM oraclelinux:8-slim

RUN microdnf install aspnetcore-runtime-3.1 && \
      microdnf clean all && \
      groupadd --system --gid 1000 fn && \
      adduser --uid 1000 --gid 1000 fn

CMD ["dotnet", "--version"]
