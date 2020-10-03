#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GroupBudget.WebApi/GroupBudget.WebApi.csproj", "GroupBudget.WebApi/"]
COPY ["account/GroupBudget.Account.Persistence/GroupBudget.Account.Persistence.csproj", "account/GroupBudget.Account.Persistence/"]
COPY ["account/GroupBudget.Account.UseCases/GroupBudget.Account.UseCases.csproj", "account/GroupBudget.Account.UseCases/"]
COPY ["account/GroupBudget.Account.Domain/GroupBudget.Account.Domain.csproj", "account/GroupBudget.Account.Domain/"]
COPY ["account/GroupBudget.Account.Messages/GroupBudget.Account.Messages.csproj", "account/GroupBudget.Account.Messages/"]
COPY ["Codefondo.DDD.Kernel/Codefondo.DDD.Kernel.csproj", "Codefondo.DDD.Kernel/"]
COPY ["GroupBudget.SharedKernel/GroupBudget.SharedKernel.csproj", "GroupBudget.SharedKernel/"]
COPY ["libraries/Codefondo.UseCase.Kernel/Codefondo.UseCase.Kernel.csproj", "libraries/Codefondo.UseCase.Kernel/"]
COPY ["account/GroupBudget.Account.Dtos/GroupBudget.Account.Dtos.csproj", "account/GroupBudget.Account.Dtos/"]
RUN dotnet restore "GroupBudget.WebApi/GroupBudget.WebApi.csproj"
COPY . .
WORKDIR "/src/GroupBudget.WebApi"
RUN dotnet build "GroupBudget.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GroupBudget.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GroupBudget.WebApi.dll"]
