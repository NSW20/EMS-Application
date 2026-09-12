using EMP_Infrastructure.Repositories;
using EMP_Infrastructure.SqlOperation;
using EMS_API.DTOValidations;
using EMS_API.ExceptioHandling;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.Helpers;
using EMS_Core.ServiceContracts;
using EMS_Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddIdentity<AppUser, AppRole>().
    AddEntityFrameworkStores<EMSDbContext>().
    AddUserStore<UserStore<AppUser,AppRole,EMSDbContext,string>>()
    .AddRoleStore<RoleStore<AppRole,EMSDbContext,string>>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    var tokenDetails = builder.Configuration.GetSection("JwtConfig");
    option.RequireHttpsMetadata = true;
    option.SaveToken = true;
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenDetails["issuer"],
        ValidAudience = tokenDetails["audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenDetails["key"])),
    };
});
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EMSDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(EMSAutoMapper));
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddTokenBucketLimiter("rateLimiter", options =>
    {
        options.TokenLimit = 100;
        options.TokensPerPeriod = 20;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.QueueLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("enableCors", options =>
    {
        options.WithOrigins("http://localhost:4200");
        options.AllowAnyMethod();
        options.AllowAnyHeader();
    });
});
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DesignationDTOValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<DesignationDTOUpdateValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeAddDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LeaveAddDTOValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<LeaveDTOValidators>();
builder.Services.AddValidatorsFromAssemblyContaining<AttendaceAddDTOValidations>();
builder.Services.AddValidatorsFromAssemblyContaining<AttendaceDTOValidations>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExeceptionMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}
app.UseCors("enableCors");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
