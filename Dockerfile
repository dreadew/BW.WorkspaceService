FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /

COPY . .

WORKDIR /src/WorkspaceService.Api
RUN dotnet publish WorkspaceService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001
EXPOSE 50501

ENTRYPOINT ["dotnet", "WorkspaceService.Api.dll"]