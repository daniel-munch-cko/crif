FROM microsoft/dotnet:2.0.6-sdk-2.1.101 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# copy csproj and restore as distinct layers
COPY ./src/Crif.Api/Crif.Api.csproj ./Crif.Api/
RUN dotnet restore Crif.Api/Crif.Api.csproj

# copy everything else and build
COPY ./src .
WORKDIR /Crif.Api/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

#build runtime image
FROM microsoft/dotnet:2.0.6-runtime
WORKDIR /app
COPY --from=build Crif.Api/out .

EXPOSE 5000
CMD ["dotnet", "Crif.Api.dll"]