# EMS-Application

Small Employee Management System (EMS) — ASP.NET Core Web API solution.

## Projects
- `EMS-API` — ASP.NET Core Web API (startup, controllers, Swagger).
- `EMS-Core` — Domain entities, service contracts, helpers and AutoMapper profiles.
- `EMP-Infrastructure` — EF Core `DbContext`, repositories and SQL operations.
- `EMS.Tests` — Unit tests.

## Implemented functionality (high level)
- Authentication & Authorization using ASP.NET Identity (`AppUser`, `AppRole`) with EF stores.
- Database access via EF Core with `EMSDbContext` (SQL Server).
- Department and Designation features (repositories + services): `IDepartmentRepository`, `IDepartmentService`, `IDesignationRepository`, `IDesignationService`.
- DTO validation with FluentValidation (`DesignationDTOValidation`, `DesignationDTOUpdateValidation`) and automatic registration.
- AutoMapper configuration via `EMSAutoMapper`.
- Global exception handling middleware (`UseGlobalExeceptionMiddleware()`).
- Serilog logging configured from configuration.
- Rate limiting (token bucket limiter named `rateLimiter`).
- CORS policy `enableCors` allowing `http://localhost:4200`.
- API Versioning and Swagger/OpenAPI.
- Memory caching and API explorer enabled.
<img width="1842" height="842" alt="image" src="https://github.com/user-attachments/assets/0826a4f2-b3f9-43c7-b6c5-45d4170480ed" />


## Getting started
1. Update connection string `DefaultConnection` in `EMS-API/appsettings.json` to point to your SQL Server.
2. From solution root:
   - Restore: `dotnet restore`
   - Build: `dotnet build`
3. Apply EF migrations (example):
   - `dotnet ef database update --project EMP-Infrastructure --startup-project EMS-API`
4. Run API:
   - `dotnet run --project EMS-API`
   - Open Swagger UI at `http://localhost:{port}/swagger` (port shown in app output).

## Tests
- Run unit tests: `dotnet test EMS.Tests` or use Visual Studio __Test Explorer__.
<img width="837" height="765" alt="image" src="https://github.com/user-attachments/assets/04c7a094-9680-4210-9149-dc604e149034" />


## Notes & next steps
- Add API controllers and DTOs for Departments/Designations if not yet exposed as endpoints.
- Ensure `appsettings.json` contains Serilog and RateLimiter settings as needed.
- Consider adding README sections for migrations, seeding, and Docker if you plan to containerize.
