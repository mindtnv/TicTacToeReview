FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY "TicTacToe.sln" "TicTacToe.sln"
COPY "src/TicTacToe.Application/TicTacToe.Application.csproj" "src/TicTacToe.Application/TicTacToe.Application.csproj"
COPY "src/TicTacToe.Domain/TicTacToe.Domain.csproj" "src/TicTacToe.Domain/TicTacToe.Domain.csproj"
COPY "src/TicTacToe.Infrastructure/TicTacToe.Infrastructure.csproj" "src/TicTacToe.Infrastructure/TicTacToe.Infrastructure.csproj"
COPY "src/TicTacToe.UnitTests/TicTacToe.UnitTests.csproj" "src/TicTacToe.UnitTests/TicTacToe.UnitTests.csproj"
COPY "src/TicTacToe.FunctionalTests/TicTacToe.FunctionalTests.csproj" "src/TicTacToe.FunctionalTests/TicTacToe.FunctionalTests.csproj"

RUN dotnet restore "TicTacToe.sln"

COPY . .
WORKDIR /src/src/TicTacToe.Application
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "TicTacToe.Application.dll"]