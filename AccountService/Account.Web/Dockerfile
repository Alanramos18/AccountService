FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR AccountService

EXPOSE 80
EXPOSE 5024

COPY ./*.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -p Account -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /AccountService
COPY --from=build /AccountService/out .
ENTRYPOINT ["dotnet", "Account.Web.dll"]
