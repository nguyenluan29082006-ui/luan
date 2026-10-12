FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Backend/WebDocTruyen.Backend.csproj", "Backend/"]
COPY ["Frontend/WebDocTruyen.Frontend.csproj", "Frontend/"]
RUN dotnet restore "Backend/WebDocTruyen.Backend.csproj"
RUN dotnet restore "Frontend/WebDocTruyen.Frontend.csproj"
COPY . .
WORKDIR "/src/Backend"
RUN dotnet publish "WebDocTruyen.Backend.csproj" -c Release -o /app/publish/backend
WORKDIR "/src/Frontend"
RUN dotnet publish "WebDocTruyen.Frontend.csproj" -c Release -o /app/publish/frontend

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN mkdir -p /app/data
COPY --from=build /app/publish/backend ./backend
COPY --from=build /app/publish/frontend ./frontend
COPY Backend/database.db ./backend/database.db
COPY Backend/database.db /app/data/database.db

COPY entrypoint.sh .
RUN sed -i 's/\r$//' entrypoint.sh && chmod +x entrypoint.sh

ENV ConnectionStrings__DefaultConnection="Data Source=/app/data/database.db"
ENV BackendApi__BaseUrl="http://127.0.0.1:5000"
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000
ENTRYPOINT ["/app/entrypoint.sh"]
