# Test Assignment

## Summary

This web application is built using Angular and ASP.NET Core, providing functionalities for loading weather archives,
viewing them, and navigating between different pages.

Frontend with 3 pages:

- Home page
- Upload page
- Archive page

Backend with 2 api endpoint:

- GET api/archives
- POST api/archives/upload

## Technologies Used

- Frontend
    - Angular 16.2.0
    - Angular CDK 16.2.0
    - Bootstrap 5.3.0 + Bootstrap Icons 1.11.3
- Backend
    - ASP.NET Core 8.0
    - Entity Framework Core 8.0
    - NPOI 2.7.0
- Database
    - PostgreSQL 16

## Installation

1. Clone repository

```
git clone https://github.com/sleeptik/DsWeatherTest.git
```

2. Install npm packages

Navigate to the frontend project directory

```
cd Client
```

```
npm ci
```

3. Install nuget packages

Navigate to the backend project directory

```
cd WebApi
```

```
dotnet restore
```

4. Migrate database

Make sure you have valid connection string in appsettings.json

```   
dotnet ef database update
```

5. Start backend

```
cd WebApi
```

```
dotnet run
```

6. Start frontend

```
cd Client
```

```
npm start
```

7. Open web application

Open your browser and navigate to http://localhost:4200 to access the application
