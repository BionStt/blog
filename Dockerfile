FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /build
COPY . .
RUN dotnet restore
COPY ./src/ ./src/
WORKDIR /build/src/Blog.Website
RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Blog.Website.dll"]