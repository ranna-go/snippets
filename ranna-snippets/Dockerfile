FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
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