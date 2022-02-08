FROM oraclelinux:8-slim

RUN microdnf install dotnet-sdk-3.1 && \
      microdnf clean all

CMD ["dotnet", "--version"]
