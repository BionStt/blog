FROM microsoft/aspnetcore-build:2.0 AS builder
WORKDIR /build
COPY *.sln ./
COPY /src/Blog.Website/Blog.Website.csproj ./src/Blog.Website/
RUN dotnet restore
COPY ./src/ ./src/
WORKDIR /build/src/Blog.Website
RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app

FROM microsoft/aspnetcore:2.0 AS final
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Blog.Website.dll"]