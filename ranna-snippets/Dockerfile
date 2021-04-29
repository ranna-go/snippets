FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ranna-snippets/ranna-snippets.csproj", "ranna-snippets/"]
RUN dotnet restore "ranna-snippets/ranna-snippets.csproj"
COPY . .
WORKDIR "/src/ranna-snippets"
RUN dotnet build "ranna-snippets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ranna-snippets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ranna-snippets.dll"]